using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public interface ICountryService : IService
{
    TView Get<TView>(int id) where TView : BaseView;
    IQueryable<CountryView> GetViews();

    void Create(CountryView view);
    void Edit(CountryView view);
    void Delete(int id);
}
