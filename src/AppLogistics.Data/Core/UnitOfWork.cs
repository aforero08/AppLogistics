using AppLogistics.Data.Logging;
using AppLogistics.Objects;
using AppLogistics.Mapping; // added for LegacyMapper + ProjectTo extension
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;

namespace AppLogistics.Data.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private readonly IAuditLogger _logger;

        public UnitOfWork(DbContext context, IAuditLogger logger = null)
        {
            _context = context;
            _logger = logger;
        }

        public TDestination GetAs<TModel, TDestination>(int? id) where TModel : BaseModel
        {
            return id == null
                ? default
                : _context.Set<TModel>().Where(model => model.Id == id).ProjectTo<TDestination>().FirstOrDefault();
        }

        public TModel Get<TModel>(int? id) where TModel : BaseModel
        {
            return id == null ? null : _context.Find<TModel>(id);
        }

        public TModel GetAsNoTracking<TModel>(int? id) where TModel : BaseModel
        {
            return id == null ? null : _context.Set<TModel>().Where(model => model.Id == id).AsNoTracking().FirstOrDefault();
        }

        public TDestination To<TDestination>(object source)
        {
            return LegacyMapper.Map<TDestination>(source);
        }

        public TDestination Map<Tsource, TDestination>(Tsource source, TDestination destination)
        {
            return LegacyMapper.Instance.Map(source, destination);
        }

        public IQuery<TModel> Select<TModel>() where TModel : BaseModel
        {
            return new Query<TModel>(_context.Set<TModel>());
        }

        public void InsertRange<TModel>(IEnumerable<TModel> models) where TModel : BaseModel
        {
            _context.AddRange(models);
        }

        public void Insert<TModel>(TModel model) where TModel : BaseModel
        {
            _context.Add(model);
        }

        public void Update<TModel>(TModel model) where TModel : BaseModel
        {
            EntityEntry<TModel> entry = _context.Entry(model);
            if (entry.State != EntityState.Modified && entry.State != EntityState.Unchanged)
            {
                entry.State = EntityState.Modified;
            }

            entry.Property(property => property.CreationDate).IsModified = false;
        }

        public void DeleteRange<TModel>(IEnumerable<TModel> models) where TModel : BaseModel
        {
            _context.RemoveRange(models);
        }

        public void Delete<TModel>(TModel model) where TModel : BaseModel
        {
            _context.Remove(model);
        }

        public void Delete<TModel>(int id) where TModel : BaseModel
        {
            Delete(_context.Find<TModel>(id));
        }

        public void Commit()
        {
            _logger?.Log(_context.ChangeTracker.Entries<BaseModel>());

            _context.SaveChanges();

            _logger?.Save();
        }

        public void Dispose()
        {
            _logger?.Dispose();
            _context.Dispose();
        }
    }
}
