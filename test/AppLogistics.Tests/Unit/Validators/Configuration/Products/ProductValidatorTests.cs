using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace AppLogistics.Validators.Tests;

public class ProductValidatorTests
{
    private ProductValidator validator;
    private DbContext context;
    private Product product;

    public ProductValidatorTests()
    {
        context = TestingContext.Create();
        validator = new ProductValidator(new UnitOfWork(TestingContext.Create(), TestingContext.Mapper));

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
        Assert.True(validator.CanEdit(ObjectsFactory.CreateProductView(product.Id)));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    #endregion CanEdit(ProductView view)
}
