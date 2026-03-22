using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Resources;
using System.Linq;

namespace AppLogistics.Validators;

public class ProductValidator : BaseValidator, IProductValidator
{
    public ProductValidator(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public bool CanCreate(ProductView view)
    {
        var alreadyExists = UnitOfWork.Select<Product>()
            .Where(c => c.Name.ToUpper().Equals(view.Name.ToUpper()))
            .Any();
        
        if (alreadyExists)
        {
            Alerts.AddError(Validation.For<ProductView>("DuplicateName"));
            return false;
        }
        
        return ModelState.IsValid;
    }

    public bool CanEdit(ProductView view)
    {
        var alreadyExists = UnitOfWork.Select<Product>()
            .Where(c => c.Name.ToUpper().Equals(view.Name.ToUpper()))
            .Any();
        
        if (alreadyExists)
        {
            Alerts.AddError(Validation.For<ProductView>("DuplicateName"));
            return false;
        }
        
        return ModelState.IsValid;
    }

    public bool CanDelete(int id)
    {
        var hasReferencedRates = UnitOfWork.Select<Rate>()
            .Where(c => c.ProductId.Equals(id))
            .Any();

        if (hasReferencedRates)
        {
            Alerts.AddError(Validation.For<ProductView>("AssociatedRates"));
            return false;
        }

        return ModelState.IsValid;
    }
}
