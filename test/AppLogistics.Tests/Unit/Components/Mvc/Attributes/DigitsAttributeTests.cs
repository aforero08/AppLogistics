using AppLogistics.Resources;
using Xunit;

namespace AppLogistics.Components.Mvc.Tests;

public class DigitsAttributeTests
{
    private DigitsAttribute attribute;

    public DigitsAttributeTests()
    {
        attribute = new DigitsAttribute();
    }

    #region DigitsAttribute()

    [Fact]
    public void DigitsAttribute_SetsErrorMessage()
    {
        attribute = new DigitsAttribute();

        string expected = Validation.For("Digits", "Test");
        string actual = attribute.FormatErrorMessage("Test");

        Assert.Equal(expected, actual);
    }

    #endregion DigitsAttribute()

    #region IsValid(Object value)

    [Fact]
    public void IsValid_Null()
    {
        Assert.True(attribute.IsValid(null));
    }

    [Theory]
    [InlineData(12.549)]
    [InlineData("+1402")]
    [InlineData(-2546798)]
    public void IsValid_NotDigits_ReturnsFalse(object value)
    {
        Assert.False(attribute.IsValid(value));
    }

    [Fact]
    public void IsValid_Digits()
    {
        Assert.True(attribute.IsValid("92233720368547758074878484887777"));
    }

    #endregion IsValid(Object value)
}
