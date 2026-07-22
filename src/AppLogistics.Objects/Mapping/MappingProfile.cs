using AppLogistics.Objects;
using AutoMapper;
using System;
using System.Linq;
using System.Reflection;

namespace MvcTemplate.Objects.Mapping;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        object[] profile = { this };
        Type[] views = GetType()
            .Assembly
            .GetTypes()
            .Where(type =>
                type.BaseType?.IsGenericType == true &&
                type.BaseType?.GetGenericTypeDefinition() == typeof(BaseView<>))
            .ToArray();

        foreach (Type view in views)
            view.GetMethod(nameof(BaseView<BaseModel>.Map), BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(Activator.CreateInstance(view), profile);

        // Add additional mappings here
        CreateMap<ServiceCreateEditView, Service>()
                .ForMember(dest => dest.Holdings, opt => opt.Ignore())
                .ForMember(dest => dest.ServiceNovelties, opt => opt.Ignore());

        CreateMap<Service, ServiceView>()
            .ForMember(dest => dest.UnifiedVehicleTypeName, opt => opt.MapFrom(src => src.VehicleTypeId.HasValue ? src.VehicleType.Name : src.Rate.VehicleType.Name ?? null));
    }
}
