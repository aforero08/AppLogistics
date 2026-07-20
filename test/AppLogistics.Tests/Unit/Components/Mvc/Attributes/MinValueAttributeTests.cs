using AppLogistics.Resources;
using Xunit;

namespace AppLogistics.Components.Mvc.Tests;

public class MinValueAttributeTests
{
    private MinValueAttribute attribute;

    public MinValueAttributeTests()
    {
        attribute = new MinValueAttribute(12.56);
    }

    #region MinValueAttribute(Double minimum)

    [Fact]
    public void MinValueAttribute_SetsMinimum()
    {
        decimal actual = new MinValueAttribute(12.56).Minimum;
        decimal expected = 12.56M;

        Assert.Equal(expected, actual);
    }

    #endregion MinValueAttribute(Double minimum)

    #region FormatErrorMessage(String name)

    [Fact]
    public void FormatErrorMessage_ForName()
    {
        attribute = new MinValueAttribute(12.56);

        string actual = attribute.FormatErrorMessage("Sum");
        string expected = Validation.For("MinValue", "Sum", attribute.Minimum);

        Assert.Equal(expected, actual);
    }

    #endregion FormatErrorMessage(String name)

    #region IsValid(Object value)

    [Fact]
    public void IsValid_Null()
    {
        Assert.True(attribute.IsValid(null));
    }

    [Theory]
    [InlineData(12.56)]
    [InlineData("12.561")]
    public void IsValid_GreaterOrEqualValue(object value)
    {
        Assert.True(attribute.IsValid(value));
    }

    [Fact]
    public void IsValid_LowerValue_ReturnsFalse()
    {
        Assert.False(attribute.IsValid(12.559));
    }

    [Fact]
    public void IsValid_NotDecimalValueIsNotValid()
    {
        Assert.False(attribute.IsValid("12.56M"));
    }

    #endregion IsValid(Object value)
}
