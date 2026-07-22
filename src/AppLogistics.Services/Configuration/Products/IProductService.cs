using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public interface IProductService : IService
{
    TView Get<TView>(int id) where TView : BaseView;
    IQueryable<ProductView> GetViews();

    void Create(ProductView view);
    void Edit(ProductView view);
    void Delete(int id);
}
