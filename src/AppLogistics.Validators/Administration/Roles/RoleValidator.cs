using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Resources;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace AppLogistics.Validators;

public class RoleValidator : BaseValidator, IRoleValidator
{
    public RoleValidator(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public bool CanCreate(RoleView view)
    {
        bool isValid = ModelState.IsValid;
        isValid &= IsUniqueTitle(view);

        return isValid;
    }

    public bool CanEdit(RoleView view)
    {
        bool isValid = ModelState.IsValid;
        isValid &= IsUniqueTitle(view);

        return isValid;
    }

    private bool IsUniqueTitle(RoleView view)
    {
        var title = view.Title ?? string.Empty;
        bool isUnique = !UnitOfWork
            .Select<Role>()
            .Any(role =>
                role.Id != view.Id &&
                role.Title != null &&
                role.Title.ToUpper() == title.ToUpper());

        if (!isUnique)
        {
            ModelState.AddModelError<RoleView>(r => r.Title, Validation.For<RoleView>("UniqueTitle"));
        }

        return isUnique;
    }
}
