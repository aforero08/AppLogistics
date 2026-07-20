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

        try
        {
            string actual = lookup.GetColumnHeader(property);
            Assert.Empty(actual);
        }
        catch (System.ArgumentNullException)
        {
        }
    }

    #endregion GetColumnHeader(PropertyInfo property)

    #region GetColumnCssClass(PropertyInfo property)

    [Theory]
    [InlineData("EnumField", "text-start")]
    [InlineData("SByteField", "text-end")]
    [InlineData("ByteField", "text-end")]
    [InlineData("Int16Field", "text-end")]
    [InlineData("UInt16Field", "text-end")]
    [InlineData("Int32Field", "text-end")]
    [InlineData("UInt32Field", "text-end")]
    [InlineData("Int64Field", "text-end")]
    [InlineData("UInt64Field", "text-end")]
    [InlineData("SingleField", "text-end")]
    [InlineData("DoubleField", "text-end")]
    [InlineData("DecimalField", "text-end")]
    [InlineData("BooleanField", "text-start")]
    [InlineData("DateTimeField", "text-start")]
    [InlineData("NullableEnumField", "text-start")]
    [InlineData("NullableSByteField", "text-end")]
    [InlineData("NullableByteField", "text-end")]
    [InlineData("NullableInt16Field", "text-end")]
    [InlineData("NullableUInt16Field", "text-end")]
    [InlineData("NullableInt32Field", "text-end")]
    [InlineData("NullableUInt32Field", "text-end")]
    [InlineData("NullableInt64Field", "text-end")]
    [InlineData("NullableUInt64Field", "text-end")]
    [InlineData("NullableSingleField", "text-end")]
    [InlineData("NullableDoubleField", "text-end")]
    [InlineData("NullableDecimalField", "text-end")]
    [InlineData("NullableBooleanField", "text-start")]
    [InlineData("NullableDateTimeField", "text-start")]
    [InlineData("StringField", "text-start")]
    [InlineData("Child", "text-start")]
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
