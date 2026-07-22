using AppLogistics.Data.Core;
using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public class BranchOfficeService : BaseService, IBranchOfficeService
{
    public BranchOfficeService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public TView Get<TView>(int id) where TView : BaseView
    {
        return UnitOfWork.GetAs<BranchOffice, TView>(id);
    }

    public IQueryable<BranchOfficeView> GetViews()
    {
        return UnitOfWork
            .Select<BranchOffice>()
            .To<BranchOfficeView>()
            .OrderByDescending(branchOffice => branchOffice.Id);
    }

    public void Create(BranchOfficeView view)
    {
        BranchOffice branchOffice = UnitOfWork.To<BranchOffice>(view);

        UnitOfWork.Insert(branchOffice);
        UnitOfWork.Commit();
    }

    public void Edit(BranchOfficeView view)
    {
        BranchOffice branchOffice = UnitOfWork.To<BranchOffice>(view);

        UnitOfWork.Update(branchOffice);
        UnitOfWork.Commit();
    }

    public void Delete(int id)
    {
        UnitOfWork.Delete<BranchOffice>(id);
        UnitOfWork.Commit();
    }
}
