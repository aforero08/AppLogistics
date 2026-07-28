using AppLogistics.Resources;
using Xunit;

namespace AppLogistics.Components.Mvc.Tests;

public class IntegerAttributeTests
{
    private IntegerAttribute attribute;

    public IntegerAttributeTests()
    {
        attribute = new IntegerAttribute();
    }

    #region IntegerAttribute()

    [Fact]
    public void IntegerAttribute_SetsErrorMessage()
    {
        attribute = new IntegerAttribute();

        string expected = Validation.For("Integer", "Test");
        string actual = attribute.FormatErrorMessage("Test");

        Assert.Equal(expected, actual);
    }

    #endregion IntegerAttribute()

    #region IsValid(Object value)

    [Fact]
    public void IsValid_Null()
    {
        Assert.True(attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_RealValue_ReturnsFalse()
    {
        Assert.False(attribute.IsValid(12.549));
    }

    [Fact]
    public void IsValid_IntegerValue()
    {
        Assert.True(attribute.IsValid(-254679849874447));
    }

    [Fact]
    public void IsValid_LongIntegerValue()
    {
        Assert.True(attribute.IsValid("+92233720368547758074878484887777"));
    }

    #endregion IsValid(Object value)
}
