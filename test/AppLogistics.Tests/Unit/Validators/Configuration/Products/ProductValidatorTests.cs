using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AppLogistics.Validators.Tests;

public class ProductValidatorTests
{
    private ProductValidator validator;
    private DbContext context;
    private Product product;

    public ProductValidatorTests()
    {
        context = TestFixture.Create().Drop();
        validator = new ProductValidator(new UnitOfWork(context, TestFixture.Mapper));

        context.Set<Product>().Add(product = ObjectsFactory.CreateProduct());
        context.SaveChanges();
    }

    #region CanCreate(ProductView view)

    [Fact]
    public void CanCreate_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanCreate(ObjectsFactory.CreateProductView(1)));
    }

    [Fact]
    public void CanCreate_ValidProduct()
    {
        Assert.True(validator.CanCreate(ObjectsFactory.CreateProductView(1)));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    #endregion CanCreate(ProductView view)

    #region CanEdit(ProductView view)

    [Fact]
    public void CanEdit_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanEdit(ObjectsFactory.CreateProductView(product.Id)));
    }

    [Fact]
    public void CanEdit_ValidProduct()
    {
        ProductView view = ObjectsFactory.CreateProductView(product.Id);
        view.Name = product.Name;

        Assert.True(validator.CanEdit(view));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    [Fact]
    public void CanEdit_DuplicateName_ReturnsFalse()
    {
        Product otherProduct = ObjectsFactory.CreateProduct();
        otherProduct.Name = "OtherName";
        context.Set<Product>().Add(otherProduct);
        context.SaveChanges();

        ProductView view = ObjectsFactory.CreateProductView(product.Id);
        view.Name = otherProduct.Name.ToLowerInvariant();

        Assert.False(validator.CanEdit(view));
        Assert.NotEmpty(validator.Alerts);
    }

    #endregion CanEdit(ProductView view)
}
