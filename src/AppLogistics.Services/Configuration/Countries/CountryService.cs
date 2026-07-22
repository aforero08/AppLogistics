using AppLogistics.Data.Core;
using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public class CountryService : BaseService, ICountryService
{
    public CountryService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public TView Get<TView>(int id) where TView : BaseView
    {
        return UnitOfWork.GetAs<Country, TView>(id);
    }

    public IQueryable<CountryView> GetViews()
    {
        return UnitOfWork
            .Select<Country>()
            .To<CountryView>()
            .OrderByDescending(country => country.Id);
    }

    public void Create(CountryView view)
    {
        Country country = UnitOfWork.To<Country>(view);

        UnitOfWork.Insert(country);
        UnitOfWork.Commit();
    }

    public void Edit(CountryView view)
    {
        Country country = UnitOfWork.To<Country>(view);

        UnitOfWork.Update(country);
        UnitOfWork.Commit();
    }

    public void Delete(int id)
    {
        UnitOfWork.Delete<Country>(id);
        UnitOfWork.Commit();
    }
}
