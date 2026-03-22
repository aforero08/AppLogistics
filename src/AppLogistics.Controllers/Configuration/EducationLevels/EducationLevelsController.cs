using AppLogistics.Components.Mvc;
using AppLogistics.Objects;
using AppLogistics.Services;
using AppLogistics.Validators;
using Microsoft.AspNetCore.Mvc;

namespace AppLogistics.Controllers.Configuration;

[Area("Configuration")]
public class EducationLevelsController : ValidatedController<IEducationLevelValidator, IEducationLevelService>
{
    public EducationLevelsController(IEducationLevelValidator validator, IEducationLevelService service)
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
    public ActionResult Create([BindExcludeId] EducationLevelView educationLevel)
    {
        if (!Validator.CanCreate(educationLevel))
        {
            return View(educationLevel);
        }

        Service.Create(educationLevel);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Details(int id)
    {
        return NotEmptyView(Service.Get<EducationLevelView>(id));
    }

    [HttpGet]
    public ActionResult Edit(int id)
    {
        return NotEmptyView(Service.Get<EducationLevelView>(id));
    }

    [HttpPost]
    public ActionResult Edit(EducationLevelView educationLevel)
    {
        if (!Validator.CanEdit(educationLevel))
        {
            return View(educationLevel);
        }

        Service.Edit(educationLevel);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Delete(int id)
    {
        return NotEmptyView(Service.Get<EducationLevelView>(id));
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
