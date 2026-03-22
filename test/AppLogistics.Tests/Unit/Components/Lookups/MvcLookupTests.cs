using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Resources;
using AppLogistics.Tests;
using NSubstitute;
using System.Linq;
using System.Reflection;
using Xunit;

namespace AppLogistics.Components.Lookups.Tests;

public class MvcLookupTests
{
    private IUnitOfWork unitOfWork;
    private MvcLookup<Role, RoleView> lookup;

    public MvcLookupTests()
    {
        unitOfWork = Substitute.For<IUnitOfWork>();
        lookup = new MvcLookup<Role, RoleView>(unitOfWork);
    }

    #region GetColumnHeader(PropertyInfo property)

    [Fact]
    public void GetColumnHeader_ReturnsPropertyTitle()
    {
        string actual = lookup.GetColumnHeader(typeof(RoleView).GetProperty("Title"));
        string expected = Resource.ForProperty(typeof(RoleView), "Title");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetColumnHeader_ReturnsRelationPropertyTitle()
    {
        PropertyInfo property = typeof(AllTypesView).GetProperty("Child");

        string actual = lookup.GetColumnHeader(property);

        Assert.Empty(actual);
    }

    #endregion GetColumnHeader(PropertyInfo property)

    #region GetColumnCssClass(PropertyInfo property)

    [Theory]
    [InlineData("EnumField", "text-left")]
    [InlineData("SByteField", "text-right")]
    [InlineData("ByteField", "text-right")]
    [InlineData("Int16Field", "text-right")]
    [InlineData("UInt16Field", "text-right")]
    [InlineData("Int32Field", "text-right")]
    [InlineData("UInt32Field", "text-right")]
    [InlineData("Int64Field", "text-right")]
    [InlineData("UInt64Field", "text-right")]
    [InlineData("SingleField", "text-right")]
    [InlineData("DoubleField", "text-right")]
    [InlineData("DecimalField", "text-right")]
    [InlineData("BooleanField", "text-center")]
    [InlineData("DateTimeField", "text-center")]
    [InlineData("NullableEnumField", "text-left")]
    [InlineData("NullableSByteField", "text-right")]
    [InlineData("NullableByteField", "text-right")]
    [InlineData("NullableInt16Field", "text-right")]
    [InlineData("NullableUInt16Field", "text-right")]
    [InlineData("NullableInt32Field", "text-right")]
    [InlineData("NullableUInt32Field", "text-right")]
    [InlineData("NullableInt64Field", "text-right")]
    [InlineData("NullableUInt64Field", "text-right")]
    [InlineData("NullableSingleField", "text-right")]
    [InlineData("NullableDoubleField", "text-right")]
    [InlineData("NullableDecimalField", "text-right")]
    [InlineData("NullableBooleanField", "text-center")]
    [InlineData("NullableDateTimeField", "text-center")]
    [InlineData("StringField", "text-left")]
    [InlineData("Child", "text-left")]
    public void GetColumnCssClass_ReturnsCssClassForPropertyType(string propertyName, string cssClass)
    {
        PropertyInfo property = typeof(AllTypesView).GetProperty(propertyName);

        string actual = lookup.GetColumnCssClass(property);
        string expected = cssClass;

        Assert.Equal(expected, actual);
    }

    #endregion GetColumnCssClass(PropertyInfo property)

    #region GetModels()

    [Fact]
    public void GetModels_FromUnitOfWork()
    {
        unitOfWork.Select<Role>().To<RoleView>().Returns(new RoleView[0].AsQueryable());

        object actual = new MvcLookup<Role, RoleView>(unitOfWork).GetModels();
        object expected = unitOfWork.Select<Role>().To<RoleView>();

        Assert.Same(expected, actual);
    }

    #endregion GetModels()
}
