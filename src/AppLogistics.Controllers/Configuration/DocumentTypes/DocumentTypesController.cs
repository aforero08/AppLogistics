using AppLogistics.Components.Mvc;
using AppLogistics.Objects;
using AppLogistics.Services;
using AppLogistics.Validators;
using Microsoft.AspNetCore.Mvc;

namespace AppLogistics.Controllers.Configuration;

[Area("Configuration")]
public class DocumentTypesController : ValidatedController<IDocumentTypeValidator, IDocumentTypeService>
{
    public DocumentTypesController(IDocumentTypeValidator validator, IDocumentTypeService service)
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
    public ActionResult Create([BindExcludeId] DocumentTypeView documentType)
    {
        if (!Validator.CanCreate(documentType))
        {
            return View(documentType);
        }

        Service.Create(documentType);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Details(int id)
    {
        return NotEmptyView(Service.Get<DocumentTypeView>(id));
    }

    [HttpGet]
    public ActionResult Edit(int id)
    {
        return NotEmptyView(Service.Get<DocumentTypeView>(id));
    }

    [HttpPost]
    public ActionResult Edit(DocumentTypeView documentType)
    {
        if (!Validator.CanEdit(documentType))
        {
            return View(documentType);
        }

        Service.Edit(documentType);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Delete(int id)
    {
        return NotEmptyView(Service.Get<DocumentTypeView>(id));
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
