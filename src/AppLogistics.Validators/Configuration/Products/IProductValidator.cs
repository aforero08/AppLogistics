using AppLogistics.Objects;

namespace AppLogistics.Validators;

public interface IProductValidator : IValidator
{
    bool CanCreate(ProductView view);
    bool CanEdit(ProductView view);
    bool CanDelete(int id);
}
