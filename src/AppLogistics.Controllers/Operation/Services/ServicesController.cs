using AppLogistics.Components.Mvc;
using AppLogistics.Objects;
using AppLogistics.Services;
using AppLogistics.Validators;
using Microsoft.AspNetCore.Mvc;

namespace AppLogistics.Controllers.Operation;

[Area("Operation")]
public class ServicesController : ValidatedController<IServiceValidator, IServiceService>
{
    public ServicesController(IServiceValidator validator, IServiceService service)
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
    public ActionResult Create([BindExcludeId] ServiceCreateEditView service)
    {
        if (!Validator.CanCreate(service))
        {
            return View(service);
        }

        Service.Create(service);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Details(int id)
    {
        return NotEmptyView(Service.Get<ServiceView>(id));
    }

    [HttpGet]
    public ActionResult Edit(int id)
    {
        return NotEmptyView(Service.GetEdit<ServiceCreateEditView>(id));
    }

    [HttpPost]
    public ActionResult Edit(ServiceCreateEditView service)
    {
        if (!Validator.CanEdit(service))
        {
            return View(service);
        }

        Service.Edit(service);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Delete(int id)
    {
        return NotEmptyView(Service.Get<ServiceView>(id));
    }

    [HttpPost]
    [ActionName("Delete")]
    public RedirectToActionResult DeleteConfirmed(int id)
    {
        Service.Delete(id);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Finalize(int id)
    {
        return NotEmptyView(Service.Get<ServiceView>(id));
    }

    [HttpPost]
    [ActionName("Finalize")]
    public ActionResult FinalizeConfirmed(int id)
    {
        if (!Validator.CanFinalize(id))
        {
            return View(Service.Get<ServiceView>(id));
        }

        Service.Finalize(id);

        return RedirectToAction("Index");
    }
}
