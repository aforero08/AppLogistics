using AppLogistics.Data.Core;
using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public class ProductService : BaseService, IProductService
{
    public ProductService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public TView Get<TView>(int id) where TView : BaseView
    {
        return UnitOfWork.GetAs<Product, TView>(id);
    }

    public IQueryable<ProductView> GetViews()
    {
        return UnitOfWork
            .Select<Product>()
            .To<ProductView>()
            .OrderByDescending(product => product.Id);
    }

    public void Create(ProductView view)
    {
        Product product = UnitOfWork.To<Product>(view);

        UnitOfWork.Insert(product);
        UnitOfWork.Commit();
    }

    public void Edit(ProductView view)
    {
        Product product = UnitOfWork.To<Product>(view);

        UnitOfWork.Update(product);
        UnitOfWork.Commit();
    }

    public void Delete(int id)
    {
        UnitOfWork.Delete<Product>(id);
        UnitOfWork.Commit();
    }
}
