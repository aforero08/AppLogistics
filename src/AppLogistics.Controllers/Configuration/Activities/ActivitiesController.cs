using AppLogistics.Components.Mvc;
using AppLogistics.Objects;
using AppLogistics.Services;
using AppLogistics.Validators;
using Microsoft.AspNetCore.Mvc;

namespace AppLogistics.Controllers.Configuration;

[Area("Configuration")]
public class ActivitiesController : ValidatedController<IActivityValidator, IActivityService>
{
    public ActivitiesController(IActivityValidator validator, IActivityService service)
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
    public ActionResult Create([BindExcludeId] ActivityView activity)
    {
        if (!Validator.CanCreate(activity))
        {
            return View(activity);
        }

        Service.Create(activity);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Details(int id)
    {
        return NotEmptyView(Service.Get<ActivityView>(id));
    }

    [HttpGet]
    public ActionResult Edit(int id)
    {
        return NotEmptyView(Service.Get<ActivityView>(id));
    }

    [HttpPost]
    public ActionResult Edit(ActivityView activity)
    {
        if (!Validator.CanEdit(activity))
        {
            return View(activity);
        }

        Service.Edit(activity);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Delete(int id)
    {
        return NotEmptyView(Service.Get<ActivityView>(id));
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
