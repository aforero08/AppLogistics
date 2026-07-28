using AppLogistics.Components.Mvc;
using AppLogistics.Objects;
using AppLogistics.Services;
using AppLogistics.Validators;
using Microsoft.AspNetCore.Mvc;

namespace AppLogistics.Controllers.Operation;

[Area("Operation")]
public class EmployeesController : ValidatedController<IEmployeeValidator, IEmployeeService>
{
    public EmployeesController(IEmployeeValidator validator, IEmployeeService service)
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
    public ActionResult Create([BindExcludeId] EmployeeCreateEditView employee)
    {
        if (!Validator.CanCreate(employee))
        {
            return View(employee);
        }

        Service.Create(employee);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Details(int id)
    {
        return NotEmptyView(Service.Get<EmployeeView>(id));
    }

    [HttpGet]
    public ActionResult Edit(int id)
    {
        return NotEmptyView(Service.Get<EmployeeCreateEditView>(id));
    }

    [HttpPost]
    public ActionResult Edit(EmployeeCreateEditView employee)
    {
        if (!Validator.CanEdit(employee))
        {
            return View(employee);
        }

        Service.Edit(employee);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Delete(int id)
    {
        return NotEmptyView(Service.Get<EmployeeView>(id));
    }

    [HttpPost]
    [ActionName("Delete")]
    public RedirectToActionResult DeleteConfirmed(int id)
    {
        if (!Validator.CanDelete(Service.Get<EmployeeCreateEditView>(id)))
        {
            return RedirectToAction("Delete", new { id });
        }

        Service.Delete(id);

        return RedirectToAction("Index");
    }
}
