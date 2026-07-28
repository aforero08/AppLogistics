using AppLogistics.Resources;
using AppLogistics.Tests;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace AppLogistics.Components.Mvc.Tests;

public class EqualToAttributeTests
{
    private EqualToAttribute attribute;

    public EqualToAttributeTests()
    {
        attribute = new EqualToAttribute("StringField");
    }

    #region EqualToAttribute(String otherPropertyName)

    [Fact]
    public void EqualToAttribute_NullProperty_Throws()
    {
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new EqualToAttribute(null));
        Assert.Equal("otherPropertyName", exception.ParamName);
    }

    [Fact]
    public void EqualToAttribute_SetsOtherPropertyName()
    {
        string actual = new EqualToAttribute("Other").OtherPropertyName;
        string expected = "Other";

        Assert.Equal(expected, actual);
    }

    #endregion EqualToAttribute(String otherPropertyName)

    #region FormatErrorMessage(String name)

    [Fact]
    public void FormatErrorMessage_ForProperty()
    {
        attribute.OtherPropertyDisplayName = "Other";

        string actual = attribute.FormatErrorMessage("EqualTo");
        string expected = Validation.For("EqualTo", "EqualTo", attribute.OtherPropertyDisplayName);

        Assert.Equal(expected, actual);
    }

    #endregion FormatErrorMessage(String name)

    #region GetValidationResult(Object value, ValidationContext context)

    [Fact]
    public void GetValidationResult_EqualValue()
    {
        ValidationContext context = new ValidationContext(new AllTypesView { StringField = "Test" });

        Assert.Null(attribute.GetValidationResult("Test", context));
    }

    [Fact]
    public void GetValidationResult_Property_Error()
    {
        ValidationContext context = new ValidationContext(new AllTypesView());

        string actual = attribute.GetValidationResult("Test", context).ErrorMessage;
        string expected = Validation.For("EqualTo", context.DisplayName, attribute.OtherPropertyName);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetValidationResult_NoProperty_Error()
    {
        attribute = new EqualToAttribute("Temp");
        ValidationContext context = new ValidationContext(new AllTypesView());

        string actual = attribute.GetValidationResult("Test", context).ErrorMessage;
        string expected = Validation.For("EqualTo", context.DisplayName, "Temp");

        Assert.Equal(expected, actual);
    }

    #endregion GetValidationResult(Object value, ValidationContext context)
}
