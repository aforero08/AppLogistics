using AppLogistics.Components.Mvc;
using AppLogistics.Objects;
using AppLogistics.Services;
using AppLogistics.Validators;
using Microsoft.AspNetCore.Mvc;

namespace AppLogistics.Controllers.Configuration;

[Area("Configuration")]
public class BranchOfficesController : ValidatedController<IBranchOfficeValidator, IBranchOfficeService>
{
    public BranchOfficesController(IBranchOfficeValidator validator, IBranchOfficeService service)
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
    public ActionResult Create([BindExcludeId] BranchOfficeView branchOffice)
    {
        if (!Validator.CanCreate(branchOffice))
        {
            return View(branchOffice);
        }

        Service.Create(branchOffice);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Details(int id)
    {
        return NotEmptyView(Service.Get<BranchOfficeView>(id));
    }

    [HttpGet]
    public ActionResult Edit(int id)
    {
        return NotEmptyView(Service.Get<BranchOfficeView>(id));
    }

    [HttpPost]
    public ActionResult Edit(BranchOfficeView branchOffice)
    {
        if (!Validator.CanEdit(branchOffice))
        {
            return View(branchOffice);
        }

        Service.Edit(branchOffice);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Delete(int id)
    {
        return NotEmptyView(Service.Get<BranchOfficeView>(id));
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
