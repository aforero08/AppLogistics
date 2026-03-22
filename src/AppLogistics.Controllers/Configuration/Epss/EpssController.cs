using AppLogistics.Components.Mvc;
using AppLogistics.Objects;
using AppLogistics.Services;
using AppLogistics.Validators;
using Microsoft.AspNetCore.Mvc;

namespace AppLogistics.Controllers.Configuration;

[Area("Configuration")]
public class EpssController : ValidatedController<IEpsValidator, IEpsService>
{
    public EpssController(IEpsValidator validator, IEpsService service)
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
    public ActionResult Create([BindExcludeId] EpsView eps)
    {
        if (!Validator.CanCreate(eps))
        {
            return View(eps);
        }

        Service.Create(eps);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Details(int id)
    {
        return NotEmptyView(Service.Get<EpsView>(id));
    }

    [HttpGet]
    public ActionResult Edit(int id)
    {
        return NotEmptyView(Service.Get<EpsView>(id));
    }

    [HttpPost]
    public ActionResult Edit(EpsView eps)
    {
        if (!Validator.CanEdit(eps))
        {
            return View(eps);
        }

        Service.Edit(eps);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Delete(int id)
    {
        return NotEmptyView(Service.Get<EpsView>(id));
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
