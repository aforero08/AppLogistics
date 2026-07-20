using AppLogistics.Resources;
using AppLogistics.Components.Mvc;
using AppLogistics.Tests; // Added for AllTypesView
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace AppLogistics.Components.Mvc.Tests;

public class StringLengthAdapterTests
{
    private StringLengthAdapter adapter;
    private ModelValidationContextBase context;
    private StringLengthAttribute attribute;

    public StringLengthAdapterTests()
    {
        attribute = new StringLengthAttribute(128);
        adapter = new StringLengthAdapter(attribute);
        IModelMetadataProvider provider = new EmptyModelMetadataProvider();
        ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AllTypesView), "StringField");
        context = new ModelValidationContextBase(new ActionContext(), metadata, provider);
    }

    [Fact]
    public void GetErrorMessage_StringLength()
    {
        attribute.MinimumLength = 0;
        string expected = Validation.For("StringLength", context.ModelMetadata.PropertyName, 128);
        string actual = adapter.GetErrorMessage(context);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetErrorMessage_StringLengthRange()
    {
        attribute.MinimumLength = 4;
        string expected = Validation.For("StringLengthRange", context.ModelMetadata.PropertyName, 128, 4);
        string actual = adapter.GetErrorMessage(context);

        Assert.Equal(expected, actual);
    }
}
