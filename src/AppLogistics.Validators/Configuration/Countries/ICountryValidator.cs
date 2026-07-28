using AppLogistics.Objects;

namespace AppLogistics.Validators;

public interface ICountryValidator : IValidator
{
    bool CanCreate(CountryView view);
    bool CanEdit(CountryView view);
    bool CanDelete(int id);
}
