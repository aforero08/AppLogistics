using AppLogistics.Objects;

namespace AppLogistics.Validators;

public interface IEducationLevelValidator : IValidator
{
    bool CanCreate(EducationLevelView view);
    bool CanEdit(EducationLevelView view);
    bool CanDelete(int id);
}
