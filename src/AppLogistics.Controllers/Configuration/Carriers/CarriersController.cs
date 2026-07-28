using AppLogistics.Components.Mvc;
using AppLogistics.Objects;
using AppLogistics.Services;
using AppLogistics.Validators;
using Microsoft.AspNetCore.Mvc;

namespace AppLogistics.Controllers.Configuration;

[Area("Configuration")]
public class CarriersController : ValidatedController<ICarrierValidator, ICarrierService>
{
    public CarriersController(ICarrierValidator validator, ICarrierService service)
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
    public ActionResult Create([BindExcludeId] CarrierView carrier)
    {
        if (!Validator.CanCreate(carrier))
        {
            return View(carrier);
        }

        Service.Create(carrier);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Details(int id)
    {
        return NotEmptyView(Service.Get<CarrierView>(id));
    }

    [HttpGet]
    public ActionResult Edit(int id)
    {
        return NotEmptyView(Service.Get<CarrierView>(id));
    }

    [HttpPost]
    public ActionResult Edit(CarrierView carrier)
    {
        if (!Validator.CanEdit(carrier))
        {
            return View(carrier);
        }

        Service.Edit(carrier);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Delete(int id)
    {
        return NotEmptyView(Service.Get<CarrierView>(id));
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
