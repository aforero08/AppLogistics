using AppLogistics.Components.Mvc;
using AppLogistics.Objects;
using AppLogistics.Services;
using AppLogistics.Validators;
using Microsoft.AspNetCore.Mvc;

namespace AppLogistics.Controllers.Configuration;

[Area("Configuration")]
public class EthnicGroupsController : ValidatedController<IEthnicGroupValidator, IEthnicGroupService>
{
    public EthnicGroupsController(IEthnicGroupValidator validator, IEthnicGroupService service)
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
    public ActionResult Create([BindExcludeId] EthnicGroupView ethnicGroup)
    {
        if (!Validator.CanCreate(ethnicGroup))
        {
            return View(ethnicGroup);
        }

        Service.Create(ethnicGroup);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Details(int id)
    {
        return NotEmptyView(Service.Get<EthnicGroupView>(id));
    }

    [HttpGet]
    public ActionResult Edit(int id)
    {
        return NotEmptyView(Service.Get<EthnicGroupView>(id));
    }

    [HttpPost]
    public ActionResult Edit(EthnicGroupView ethnicGroup)
    {
        if (!Validator.CanEdit(ethnicGroup))
        {
            return View(ethnicGroup);
        }

        Service.Edit(ethnicGroup);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Delete(int id)
    {
        return NotEmptyView(Service.Get<EthnicGroupView>(id));
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
