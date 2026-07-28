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

namespace AppLogistics.Controllers.Operation.Tests;

public class EmployeesControllerTests : ControllerTests
{
    private EmployeesController controller;
    private IEmployeeValidator validator;
    private IEmployeeService service;
    private EmployeeView employeeView;
    private EmployeeCreateEditView employeeCreateEditView;

    public EmployeesControllerTests()
    {
        validator = Substitute.For<IEmployeeValidator>();
        service = Substitute.For<IEmployeeService>();

        employeeView = ObjectsFactory.CreateEmployeeView();
        employeeCreateEditView = ObjectsFactory.CreateEmployeeCreateEditView();

        controller = Substitute.ForPartsOf<EmployeesController>(validator, service);
        controller.ControllerContext.RouteData = new RouteData();
    }

    #region Index()

    [Fact]
    public void Index_ReturnsEmployeeViews()
    {
        service.GetViews().Returns(new EmployeeView[0].AsQueryable());

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

    #region Create(EmployeeView employee)

    [Fact]
    public void Create_ProtectsFromOverpostingId()
    {
        ProtectsFromOverpostingId(controller, "Create");
    }

    [Fact]
    public void Create_CanNotCreate_ReturnsSameView()
    {
        validator.CanCreate(employeeCreateEditView).Returns(false);

        object actual = (controller.Create(employeeCreateEditView) as ViewResult).ViewData.Model;
        object expected = employeeCreateEditView;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Create_Employee()
    {
        validator.CanCreate(employeeCreateEditView).Returns(true);

        controller.Create(employeeCreateEditView);

        service.Received().Create(employeeCreateEditView);
    }

    [Fact]
    public void Create_RedirectsToIndex()
    {
        validator.CanCreate(employeeCreateEditView).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Create(employeeCreateEditView);

        Assert.Same(expected, actual);
    }

    #endregion Create(EmployeeView employee)

    #region Details(String id)

    [Fact]
    public void Details_ReturnsNotEmptyView()
    {
        service.Get<EmployeeView>(employeeView.Id).Returns(employeeView);

        object expected = NotEmptyView(controller, employeeView);
        object actual = controller.Details(employeeView.Id);

        Assert.Same(expected, actual);
    }

    #endregion Details(String id)

    #region Edit(String id)

    [Fact]
    public void Edit_ReturnsNotEmptyView()
    {
        service.Get<EmployeeCreateEditView>(employeeCreateEditView.Id).Returns(employeeCreateEditView);

        object expected = NotEmptyView(controller, employeeCreateEditView);
        object actual = controller.Edit(employeeCreateEditView.Id);

        Assert.Same(expected, actual);
    }

    #endregion Edit(String id)

    #region Edit(EmployeeView employee)

    [Fact]
    public void Edit_CanNotEdit_ReturnsSameView()
    {
        validator.CanEdit(employeeCreateEditView).Returns(false);

        object actual = (controller.Edit(employeeCreateEditView) as ViewResult).ViewData.Model;
        object expected = employeeCreateEditView;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Edit_Employee()
    {
        validator.CanEdit(employeeCreateEditView).Returns(true);

        controller.Edit(employeeCreateEditView);

        service.Received().Edit(employeeCreateEditView);
    }

    [Fact]
    public void Edit_RedirectsToIndex()
    {
        validator.CanEdit(employeeCreateEditView).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Edit(employeeCreateEditView);

        Assert.Same(expected, actual);
    }

    #endregion Edit(EmployeeView employee)

    #region Delete(String id)

    [Fact]
    public void Delete_ReturnsNotEmptyView()
    {
        service.Get<EmployeeView>(employeeView.Id).Returns(employeeView);

        object expected = NotEmptyView(controller, employeeView);
        object actual = controller.Delete(employeeView.Id);

        Assert.Same(expected, actual);
    }

    #endregion Delete(String id)

    #region DeleteConfirmed(String id)

    [Fact]
    public void DeleteConfirmed_DeletesEmployee()
    {
        service.Get<EmployeeCreateEditView>(employeeView.Id).Returns(employeeCreateEditView);
        validator.CanDelete(employeeCreateEditView).Returns(true);

        controller.DeleteConfirmed(employeeView.Id);

        service.Received().Delete(employeeView.Id);
    }

    [Fact]
    public void Delete_RedirectsToIndex()
    {
        service.Get<EmployeeCreateEditView>(employeeView.Id).Returns(employeeCreateEditView);
        validator.CanDelete(employeeCreateEditView).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.DeleteConfirmed(employeeView.Id);

        Assert.Same(expected, actual);
    }

    #endregion DeleteConfirmed(String id)
}
