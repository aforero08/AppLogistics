using AppLogistics.Components.Mvc;
using AppLogistics.Objects;
using AppLogistics.Services;
using AppLogistics.Validators;
using Microsoft.AspNetCore.Mvc;

namespace AppLogistics.Controllers.Configuration
{
    [Area("Configuration")]
    public class ProductsController : ValidatedController<IProductValidator, IProductService>
    {
        public ProductsController(IProductValidator validator, IProductService service)
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
        public ActionResult Create([BindExcludeId] ProductView product)
        {
            if (!Validator.CanCreate(product))
            {
                return View(product);
            }

            Service.Create(product);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            return NotEmptyView(Service.Get<ProductView>(id));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return NotEmptyView(Service.Get<ProductView>(id));
        }

        [HttpPost]
        public ActionResult Edit(ProductView product)
        {
            if (!Validator.CanEdit(product))
            {
                return View(product);
            }

            Service.Edit(product);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            return NotEmptyView(Service.Get<ProductView>(id));
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
}
