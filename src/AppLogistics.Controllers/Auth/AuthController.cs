using AppLogistics.Components.Mail;
using AppLogistics.Objects;
using AppLogistics.Resources;
using AppLogistics.Services;
using AppLogistics.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AppLogistics.Controllers;

[AllowAnonymous]
public class AuthController : ValidatedController<IAccountValidator, IAccountService>
{
    private readonly IMailClient _mailClient;

    public AuthController(IAccountValidator validator, IAccountService service, IMailClient mailClient)
        : base(validator, service)
    {
        _mailClient = mailClient;
    }

    [HttpGet]
    public ActionResult Recover()
    {
        if (Service.IsLoggedIn(User))
        {
            return RedirectToDefault();
        }

        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Recover(AccountRecoveryView account)
    {
        if (Service.IsLoggedIn(User))
        {
            return RedirectToDefault();
        }

        if (!Validator.CanRecover(account))
        {
            return View(account);
        }

        if (Service.Recover(account) is string token)
        {
            string url = Url.Action("Reset", "Auth", new { token }, Request.Scheme);

            await _mailClient.SendFromAdmin(account.Email, "",
                Message.For<AccountView>("RecoveryEmailSubject"),
                Message.For<AccountView>("RecoveryEmailBody", url));
        }

        Alerts.AddInfo(Message.For<AccountView>("RecoveryInformation"));

        return RedirectToAction("Login");
    }

    [HttpGet]
    public ActionResult Reset(string token)
    {
        if (Service.IsLoggedIn(User))
        {
            return RedirectToDefault();
        }

        if (!Validator.CanReset(new AccountResetView { Token = token }))
        {
            return RedirectToAction("Recover");
        }

        return View();
    }

    [HttpPost]
    public ActionResult Reset(AccountResetView account)
    {
        if (Service.IsLoggedIn(User))
        {
            return RedirectToDefault();
        }

        if (!Validator.CanReset(account))
        {
            return RedirectToAction("Recover");
        }

        Service.Reset(account);

        Alerts.AddSuccess(Message.For<AccountView>("SuccessfulReset"), 4000);

        return RedirectToAction("Login");
    }

    [HttpGet]
    public ActionResult Login(string returnUrl)
    {
        if (Service.IsLoggedIn(User))
        {
            return RedirectToLocal(returnUrl);
        }

        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(AccountLoginView account, string returnUrl)
    {
        if (Service.IsLoggedIn(User))
        {
            return RedirectToLocal(returnUrl);
        }

        if (!Validator.CanLogin(account))
        {
            return View(account);
        }

        await Service.Login(HttpContext, account.Username);

        return RedirectToLocal(returnUrl);
    }

    [HttpGet]
    public async Task<RedirectToActionResult> Logout()
    {
        await Service.Logout(HttpContext);

        return RedirectToAction("Login");
    }
}
