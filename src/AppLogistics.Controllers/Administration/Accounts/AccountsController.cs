using AppLogistics.Components.Mvc;
using AppLogistics.Objects;
using AppLogistics.Services;
using AppLogistics.Validators;
using Microsoft.AspNetCore.Mvc;

namespace AppLogistics.Controllers.Administration;

[Area("Administration")]
public class AccountsController : ValidatedController<IAccountValidator, IAccountService>
{
    public AccountsController(IAccountValidator validator, IAccountService service)
        : base(validator, service)
    {
    }

    [HttpGet]
    public ViewResult Index()
    {
        return View(Service.GetViews());
    }

    [HttpGet]
    public ViewResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create([BindExcludeId] AccountCreateView account)
    {
        if (!Validator.CanCreate(account))
        {
            return View(account);
        }

        Service.Create(account);

        Authorization?.Refresh();

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Details(int id)
    {
        return NotEmptyView(Service.Get<AccountView>(id));
    }

    [HttpGet]
    public ActionResult Edit(int id)
    {
        return NotEmptyView(Service.Get<AccountEditView>(id));
    }

    [HttpPost]
    public ActionResult Edit(AccountEditView account)
    {
        if (!Validator.CanEdit(account))
        {
            return View(account);
        }

        Service.Edit(account);

        Authorization?.Refresh();

        return RedirectToAction("Index");
    }
}
