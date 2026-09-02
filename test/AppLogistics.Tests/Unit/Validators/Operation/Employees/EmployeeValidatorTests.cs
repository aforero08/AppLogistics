using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AppLogistics.Validators.Tests;

public class EmployeeValidatorTests
{
    private EmployeeValidator validator;
    private DbContext context;
    private Employee employee;

    public EmployeeValidatorTests()
    {
        context = TestFixture.Create().Drop();
        validator = new EmployeeValidator(new UnitOfWork(context, TestFixture.Mapper));

        context.Set<Employee>().Add(employee = ObjectsFactory.CreateEmployee());
        context.SaveChanges();
    }

    #region CanCreate(EmployeeView view)

    [Fact]
    public void CanCreate_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanCreate(ObjectsFactory.CreateEmployeeCreateEditView(1)));
    }

    [Fact]
    public void CanCreate_ValidEmployee()
    {
        Assert.True(validator.CanCreate(ObjectsFactory.CreateEmployeeCreateEditView(1)));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    #endregion CanCreate(EmployeeView view)

    #region CanEdit(EmployeeView view)

    [Fact]
    public void CanEdit_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanEdit(ObjectsFactory.CreateEmployeeCreateEditView(employee.Id)));
    }

    [Fact]
    public void CanEdit_ToSameDocumentNumberAndInternalCode()
    {
        EmployeeCreateEditView view = ObjectsFactory.CreateEmployeeCreateEditView(employee.Id);
        view.DocumentNumber = employee.DocumentNumber;
        view.InternalCode = employee.InternalCode;

        Assert.True(validator.CanEdit(view));
        Assert.Empty(validator.Alerts);
    }

    [Fact]
    public void CanEdit_UsedDocumentNumber_ReturnsFalse()
    {
        EmployeeCreateEditView view = ObjectsFactory.CreateEmployeeCreateEditView(employee.Id + 1);
        view.DocumentNumber = employee.DocumentNumber;
        view.InternalCode = "UnusedInternalCode";

        Assert.False(validator.CanEdit(view));
        Assert.NotEmpty(validator.Alerts);
    }

    [Fact]
    public void CanEdit_UsedInternalCode_ReturnsFalse()
    {
        EmployeeCreateEditView view = ObjectsFactory.CreateEmployeeCreateEditView(employee.Id + 1);
        view.DocumentNumber = "UnusedDocumentNumber";
        view.InternalCode = employee.InternalCode;

        Assert.False(validator.CanEdit(view));
        Assert.NotEmpty(validator.Alerts);
    }

    [Fact]
    public void CanEdit_ValidEmployee()
    {
        Assert.True(validator.CanEdit(ObjectsFactory.CreateEmployeeCreateEditView(employee.Id)));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    #endregion CanEdit(EmployeeView view)
}
