using AppLogistics.Components.Mvc;
using AppLogistics.Objects;
using AppLogistics.Services;
using AppLogistics.Validators;
using Microsoft.AspNetCore.Mvc;

namespace AppLogistics.Controllers.Configuration;

[Area("Configuration")]
public class MaritalStatusesController : ValidatedController<IMaritalStatusValidator, IMaritalStatusService>
{
    public MaritalStatusesController(IMaritalStatusValidator validator, IMaritalStatusService service)
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
    public ActionResult Create([BindExcludeId] MaritalStatusView maritalStatus)
    {
        if (!Validator.CanCreate(maritalStatus))
        {
            return View(maritalStatus);
        }

        Service.Create(maritalStatus);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Details(int id)
    {
        return NotEmptyView(Service.Get<MaritalStatusView>(id));
    }

    [HttpGet]
    public ActionResult Edit(int id)
    {
        return NotEmptyView(Service.Get<MaritalStatusView>(id));
    }

    [HttpPost]
    public ActionResult Edit(MaritalStatusView maritalStatus)
    {
        if (!Validator.CanEdit(maritalStatus))
        {
            return View(maritalStatus);
        }

        Service.Edit(maritalStatus);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Delete(int id)
    {
        return NotEmptyView(Service.Get<MaritalStatusView>(id));
    }

    [HttpPost]
    [ActionName("Delete")]
    public RedirectToActionResult DeleteConfirmed(int id)
    {
        if (!Validator.CanDelete(id))
        {
            return RedirectToAction("Delete", new { id });
        }

        Service.Delete(id);

        return RedirectToAction("Index");
    }
}
