using AppLogistics.Objects;

namespace AppLogistics.Validators;

public interface IAccountValidator : IValidator
{
    bool CanRecover(AccountRecoveryView view);

    bool CanReset(AccountResetView view);

    bool CanLogin(AccountLoginView view);

    bool CanCreate(AccountCreateView view);

    bool CanEdit(AccountEditView view);

    bool CanEdit(ProfileEditView view);

    bool CanDelete(ProfileDeleteView view);
}
