using AppLogistics.Components.Mvc;
using AppLogistics.Objects;
using AppLogistics.Services;
using AppLogistics.Validators;
using Microsoft.AspNetCore.Mvc;

namespace AppLogistics.Controllers.Configuration;

[Area("Configuration")]
public class VehicleTypesController : ValidatedController<IVehicleTypeValidator, IVehicleTypeService>
{
    public VehicleTypesController(IVehicleTypeValidator validator, IVehicleTypeService service)
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
    public ActionResult Create([BindExcludeId] VehicleTypeView vehicleType)
    {
        if (!Validator.CanCreate(vehicleType))
        {
            return View(vehicleType);
        }

        Service.Create(vehicleType);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Details(int id)
    {
        return NotEmptyView(Service.Get<VehicleTypeView>(id));
    }

    [HttpGet]
    public ActionResult Edit(int id)
    {
        return NotEmptyView(Service.Get<VehicleTypeView>(id));
    }

    [HttpPost]
    public ActionResult Edit(VehicleTypeView vehicleType)
    {
        if (!Validator.CanEdit(vehicleType))
        {
            return View(vehicleType);
        }

        Service.Edit(vehicleType);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Delete(int id)
    {
        return NotEmptyView(Service.Get<VehicleTypeView>(id));
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
