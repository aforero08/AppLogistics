using AppLogistics.Data.Core;
using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services
{
    public class SectorService : BaseService, ISectorService
    {
        public SectorService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public TView Get<TView>(int id) where TView : BaseView
        {
            return UnitOfWork.GetAs<Sector, TView>(id);
        }

        public IQueryable<SectorView> GetViews()
        {
            return UnitOfWork
                .Select<Sector>()
                .To<SectorView>()
                .OrderByDescending(sector => sector.Id);
        }

        public void Create(SectorCreateEditView view)
        {
            Sector sector = UnitOfWork.To<Sector>(view);

            UnitOfWork.Insert(sector);
            UnitOfWork.Commit();
        }

        public void Edit(SectorCreateEditView view)
        {
            Sector sector = UnitOfWork.To<Sector>(view);

            UnitOfWork.Update(sector);
            UnitOfWork.Commit();
        }

        public void Delete(int id)
        {
            UnitOfWork.Delete<Sector>(id);
            UnitOfWork.Commit();
        }
    }
}
