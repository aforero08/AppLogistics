using AppLogistics.Objects;
using System;

namespace AppLogistics.Validators;

public interface IActivityValidator : IValidator
{
    bool CanCreate(ActivityView view);
    bool CanEdit(ActivityView view);
    bool CanDelete(int id);
}
