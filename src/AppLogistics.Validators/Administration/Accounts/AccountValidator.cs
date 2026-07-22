using AppLogistics.Components.Security;
using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Resources;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;

namespace AppLogistics.Validators;

public class AccountValidator : BaseValidator, IAccountValidator
{
    private readonly IHasher _hasher;

    public AccountValidator(IUnitOfWork unitOfWork, IHasher hasher)
        : base(unitOfWork)
    {
        _hasher = hasher;
    }

    public bool CanRecover(AccountRecoveryView view)
    {
        return ModelState.IsValid;
    }

    public bool CanReset(AccountResetView view)
    {
        bool isValid = IsValidResetToken(view.Token);
        isValid &= ModelState.IsValid;

        return isValid;
    }

    public bool CanLogin(AccountLoginView view)
    {
        bool isValid = IsAuthenticated(view.Username, view.Password);
        isValid = isValid && IsActive(view.Username);
        isValid &= ModelState.IsValid;

        return isValid;
    }

    public bool CanCreate(AccountCreateView view)
    {
        bool isValid = IsUniqueUsername(view.Id, view.Username);
        isValid &= IsUniqueEmail(view.Id, view.Email);
        isValid &= ModelState.IsValid;

        return isValid;
    }

    public bool CanEdit(AccountEditView view)
    {
        bool isValid = IsUniqueUsername(view.Id, view.Username);
        isValid &= IsUniqueEmail(view.Id, view.Email);
        isValid &= ModelState.IsValid;

        return isValid;
    }

    public bool CanEdit(ProfileEditView view)
    {
        bool isValid = IsUniqueUsername(CurrentAccountId, view.Username);
        isValid &= IsCorrectPassword(CurrentAccountId, view.Password);
        isValid &= IsUniqueEmail(CurrentAccountId, view.Email);
        isValid &= ModelState.IsValid;

        return isValid;
    }

    public bool CanDelete(ProfileDeleteView view)
    {
        bool isValid = IsCorrectPassword(CurrentAccountId, view.Password);
        isValid &= ModelState.IsValid;

        return isValid;
    }

    private bool IsUniqueUsername(int accountId, string username)
    {
        bool isUnique = !UnitOfWork
            .Select<Account>()
            .Any(account =>
                account.Id != accountId
                && account.Username == username);

        if (!isUnique)
        {
            ModelState.AddModelError<AccountView>(model => model.Username, Validation.For<AccountView>("UniqueUsername"));
        }

        return isUnique;
    }

    private bool IsUniqueEmail(int accountId, string email)
    {
        bool isUnique = !UnitOfWork
            .Select<Account>()
            .Any(account =>
                account.Id != accountId
                && account.Email == email);

        if (!isUnique)
        {
            ModelState.AddModelError<AccountView>(account => account.Email,
                Validation.For<AccountView>("UniqueEmail"));
        }

        return isUnique;
    }

    private bool IsAuthenticated(string username, string password)
    {
        string passHash = UnitOfWork
            .Select<Account>()
            .Where(account => account.Username == username)
            .Select(account => account.Passhash)
            .SingleOrDefault();

        bool isCorrect = _hasher.VerifyPassword(password, passHash);
        if (!isCorrect)
        {
            Alerts.AddError(Validation.For<AccountView>("IncorrectAuthentication"));
        }

        return isCorrect;
    }

    private bool IsCorrectPassword(int accountId, string password)
    {
        string passHash = UnitOfWork
            .Select<Account>()
            .Where(account => account.Id == accountId)
            .Select(account => account.Passhash)
            .Single();

        bool isCorrect = _hasher.VerifyPassword(password, passHash);
        if (!isCorrect)
        {
            ModelState.AddModelError<ProfileEditView>(account => account.Password,
                Validation.For<AccountView>("IncorrectPassword"));
        }

        return isCorrect;
    }

    private bool IsValidResetToken(string token)
    {
        bool isValid = UnitOfWork
            .Select<Account>()
            .Any(account =>
                account.RecoveryToken == token
                && account.RecoveryTokenExpirationDate > DateTime.Now);

        if (!isValid)
        {
            Alerts.AddError(Validation.For<AccountView>("ExpiredToken"));
        }

        return isValid;
    }

    private bool IsActive(string username)
    {
        bool isActive = UnitOfWork
            .Select<Account>()
            .Any(account =>
                !account.IsLocked
                && account.Username == username);

        if (!isActive)
        {
            Alerts.AddError(Validation.For<AccountView>("LockedAccount"));
        }

        return isActive;
    }
}
