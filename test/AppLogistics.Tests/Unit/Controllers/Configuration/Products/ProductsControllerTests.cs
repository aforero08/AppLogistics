using AppLogistics.Controllers.Tests;
using AppLogistics.Objects;
using AppLogistics.Services;
using AppLogistics.Tests;
using AppLogistics.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using System.Linq;
using Xunit;

namespace AppLogistics.Controllers.Configuration.Tests;

public class ProductsControllerTests : ControllerTests
{
    private ProductsController controller;
    private IProductValidator validator;
    private IProductService service;
    private ProductView product;

    public ProductsControllerTests()
    {
        validator = Substitute.For<IProductValidator>();
        service = Substitute.For<IProductService>();

        product = ObjectsFactory.CreateProductView();

        controller = Substitute.ForPartsOf<ProductsController>(validator, service);
        controller.ControllerContext.RouteData = new RouteData();
    }

    #region Index()

    [Fact]
    public void Index_ReturnsProductViews()
    {
        service.GetViews().Returns(new ProductView[0].AsQueryable());

        object actual = controller.Index().ViewData.Model;
        object expected = service.GetViews();

        Assert.Same(expected, actual);
    }

    #endregion Index()

    #region Create()

    [Fact]
    public void Create_ReturnsEmptyView()
    {
        ViewDataDictionary actual = controller.Create().ViewData;

        Assert.Null(actual.Model);
    }

    #endregion Create()

    #region Create(ProductView product)

    [Fact]
    public void Create_ProtectsFromOverpostingId()
    {
        ProtectsFromOverpostingId(controller, "Create");
    }

    [Fact]
    public void Create_CanNotCreate_ReturnsSameView()
    {
        validator.CanCreate(product).Returns(false);

        object actual = (controller.Create(product) as ViewResult).ViewData.Model;
        object expected = product;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Create_Product()
    {
        validator.CanCreate(product).Returns(true);

        controller.Create(product);

        service.Received().Create(product);
    }

    [Fact]
    public void Create_RedirectsToIndex()
    {
        validator.CanCreate(product).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Create(product);

        Assert.Same(expected, actual);
    }

    #endregion Create(ProductView product)

    #region Details(String id)

    [Fact]
    public void Details_ReturnsNotEmptyView()
    {
        service.Get<ProductView>(product.Id).Returns(product);

        object expected = NotEmptyView(controller, product);
        object actual = controller.Details(product.Id);

        Assert.Same(expected, actual);
    }

    #endregion Details(String id)

    #region Edit(String id)

    [Fact]
    public void Edit_ReturnsNotEmptyView()
    {
        service.Get<ProductView>(product.Id).Returns(product);

        object expected = NotEmptyView(controller, product);
        object actual = controller.Edit(product.Id);

        Assert.Same(expected, actual);
    }

    #endregion Edit(String id)

    #region Edit(ProductView product)

    [Fact]
    public void Edit_CanNotEdit_ReturnsSameView()
    {
        validator.CanEdit(product).Returns(false);

        object actual = (controller.Edit(product) as ViewResult).ViewData.Model;
        object expected = product;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Edit_Product()
    {
        validator.CanEdit(product).Returns(true);

        controller.Edit(product);

        service.Received().Edit(product);
    }

    [Fact]
    public void Edit_RedirectsToIndex()
    {
        validator.CanEdit(product).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Edit(product);

        Assert.Same(expected, actual);
    }

    #endregion Edit(ProductView product)

    #region Delete(String id)

    [Fact]
    public void Delete_ReturnsNotEmptyView()
    {
        service.Get<ProductView>(product.Id).Returns(product);

        object expected = NotEmptyView(controller, product);
        object actual = controller.Delete(product.Id);

        Assert.Same(expected, actual);
    }

    #endregion Delete(String id)

    #region DeleteConfirmed(String id)

    [Fact]
    public void DeleteConfirmed_DeletesProduct()
    {
        controller.DeleteConfirmed(product.Id);

        service.Received().Delete(product.Id);
    }

    [Fact]
    public void Delete_RedirectsToIndex()
    {
        object expected = RedirectToAction(controller, "Index");
        object actual = controller.DeleteConfirmed(product.Id);

        Assert.Same(expected, actual);
    }

    #endregion DeleteConfirmed(String id)
}
