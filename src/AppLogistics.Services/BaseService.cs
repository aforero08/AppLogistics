using AppLogistics.Data.Core;

namespace AppLogistics.Services;

public abstract class BaseService : IService
{
    public int CurrentAccountId { get; set; }
    protected IUnitOfWork UnitOfWork { get; }

    protected BaseService(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    public void Dispose()
    {
        UnitOfWork.Dispose();
    }
}
