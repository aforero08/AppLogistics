using AppLogistics.Components.Lookups;
using AppLogistics.Data.Core;
using AppLogistics.Objects;
using Microsoft.AspNetCore.Mvc;
using NonFactors.Mvc.Lookup;
using NSubstitute;
using Xunit;

namespace AppLogistics.Controllers.Tests;

public class LookupControllerTests
{
    private LookupController controller;
    private IUnitOfWork unitOfWork;
    private LookupFilter filter;
    private MvcLookup<Role, RoleView> lookup;

    public LookupControllerTests()
    {
        unitOfWork = Substitute.For<IUnitOfWork>();
        controller = Substitute.ForPartsOf<LookupController>(unitOfWork);

        lookup = Substitute.For<MvcLookup<Role, RoleView>>(unitOfWork);
        filter = new LookupFilter();
    }

    #region GetData(MvcLookup lookup, LookupFilter filter)

    [Fact]
    public void GetData_SetsFilter()
    {
        controller.GetData(lookup, filter);

        LookupFilter actual = lookup.Filter;
        LookupFilter expected = filter;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetData_ReturnsJsonResult()
    {
        lookup.GetData().Returns(new LookupData());

        object actual = controller.GetData(lookup, filter).Value;
        object expected = lookup.GetData();

        Assert.Same(expected, actual);
    }

    #endregion GetData(MvcLookup lookup, LookupFilter filter)

    #region Role(LookupFilter filter)

    [Fact]
    public void Role_ReturnsRolesData()
    {
        object expected = GetData<MvcLookup<Role, RoleView>>(controller);
        object actual = controller.Role(filter);

        Assert.Same(expected, actual);
    }

    #endregion Role(LookupFilter filter)

    #region Dispose()

    [Fact]
    public void Dispose_UnitOfWork()
    {
        controller.Dispose();

        unitOfWork.Received().Dispose();
    }

    [Fact]
    public void Dispose_MultipleTimes()
    {
        controller.Dispose();
        controller.Dispose();
    }

    #endregion Dispose()

    #region Test helpers

    private JsonResult GetData<TLookup>(LookupController lookupController) where TLookup : ALookup
    {
        lookupController.When(sub => sub.GetData<Role, RoleView>(Arg.Any<MvcLookup<Role, RoleView>>(), filter)).DoNotCallBase();
        lookupController.GetData<Role, RoleView>(Arg.Any<MvcLookup<Role, RoleView>>(), filter).Returns(new JsonResult("Test"));

        return lookupController.GetData<Role, RoleView>(null, filter);
    }

    #endregion Test helpers
}
