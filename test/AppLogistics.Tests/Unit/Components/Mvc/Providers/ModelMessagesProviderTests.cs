using AppLogistics.Resources;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Xunit;

namespace AppLogistics.Components.Mvc.Tests;

public class ModelMessagesProviderTests
{
    private DefaultModelBindingMessageProvider messages;

    public ModelMessagesProviderTests()
    {
        messages = new DefaultModelBindingMessageProvider();
        ModelMessagesProvider.Set(messages);
    }

    #region Set(ModelBindingMessageProvider messages)

    [Fact]
    public void ModelMessagesProvider_SetsAttemptedValueIsInvalidAccessor()
    {
        string actual = messages.AttemptedValueIsInvalidAccessor("Test", "Property");
        string expected = Validation.For("InvalidField", "Property");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ModelMessagesProvider_SetsUnknownValueIsInvalidAccessor()
    {
        string actual = messages.UnknownValueIsInvalidAccessor("Property");
        string expected = Validation.For("InvalidField", "Property");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ModelMessagesProvider_SetsMissingBindRequiredValueAccessor()
    {
        string actual = messages.MissingBindRequiredValueAccessor("Property");
        string expected = Validation.For("Required", "Property");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ModelMessagesProvider_SetsValueMustNotBeNullAccessor()
    {
        string actual = messages.ValueMustNotBeNullAccessor("Property");
        string expected = Validation.For("Required", "Property");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ModelMessagesProvider_ValueIsInvalidAccessor()
    {
        string expected = Validation.For("InvalidValue", "Value");
        string actual = messages.ValueIsInvalidAccessor("Value");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ModelMessagesProvider_SetsValueMustBeANumberAccessor()
    {
        string actual = messages.ValueMustBeANumberAccessor("Property");
        string expected = Validation.For("Numeric", "Property");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ModelMessagesProvider_SetsMissingKeyOrValueAccessor()
    {
        string actual = messages.MissingKeyOrValueAccessor();
        string expected = Validation.For("RequiredValue");

        Assert.Equal(expected, actual);
    }

    #endregion Set(ModelBindingMessageProvider messages)
}
