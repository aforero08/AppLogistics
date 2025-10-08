using System;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace AppLogistics.Mapping
{
    // Provides a central AutoMapper configuration accessible via static methods.
    // Migration shim replacing legacy global Mapper usage.
    public static class LegacyMapper
    {
        private static readonly Lazy<IMapper> _mapper = new Lazy<IMapper>(Configure);
        public static IMapper Instance => _mapper.Value;
        public static IConfigurationProvider ConfigurationProvider => Instance.ConfigurationProvider;

        public static TDestination Map<TDestination>(object source) => Instance.Map<TDestination>(source);

        private static IMapper Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // Dynamically map between model and view classes in AppLogistics.Objects assembly
                // without creating compile-time dependency (reflection only).
                Assembly objectsAsm;
                try { objectsAsm = Assembly.Load("AppLogistics.Objects"); }
                catch { return; }

                var types = objectsAsm.GetTypes().Where(t => t.IsClass && !t.IsAbstract).ToList();
                var viewTypes = types.Where(t => t.Name.EndsWith("View", StringComparison.Ordinal));
                string[] suffixes = { "CreateEditView", "CreateView", "EditView", "View" };

                foreach (var view in viewTypes)
                {
                    Type model = null;
                    foreach (var suf in suffixes)
                    {
                        if (!view.Name.EndsWith(suf, StringComparison.Ordinal)) continue;
                        var candidate = view.Name[..^suf.Length];
                        model = types.FirstOrDefault(t => t.Name == candidate);
                        if (model != null) break;
                    }

                    if (model != null)
                    {
                        try { cfg.CreateMap(model, view).ReverseMap(); } catch { }
                    }
                }
            });

            // Do not AssertConfigurationIsValid here because some dynamic pairs may be partial.
            return config.CreateMapper();
        }
    }
}

namespace AppLogistics.Mapping
{
    using AutoMapper.QueryableExtensions;
    using System.Linq;

    public static class LegacyProjectionExtensions
    {
        public static IQueryable<TDestination> ProjectToLegacy<TDestination>(this IQueryable source)
        {
            return global::AutoMapper.QueryableExtensions.Extensions.ProjectTo<TDestination>(source, LegacyMapper.ConfigurationProvider);
        }

        // Legacy alias to keep existing .ProjectTo<T>() calls working after upgrade.
        public static IQueryable<TDestination> ProjectTo<TDestination>(this IQueryable source)
        {
            return global::AutoMapper.QueryableExtensions.Extensions.ProjectTo<TDestination>(source, LegacyMapper.ConfigurationProvider);
        }
    }
}
