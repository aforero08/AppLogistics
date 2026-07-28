using AppLogistics.Resources;
using AppLogistics.Tests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace AppLogistics.Components.Mvc.Tests;

public class MinLengthAdapterTests
{
    [Fact]
    public void GetErrorMessage_MinLength()
    {
        IModelMetadataProvider provider = new EmptyModelMetadataProvider();
        var attribute = new MinLengthAttribute(128);
        MinLengthAdapter adapter = new MinLengthAdapter(attribute);
        ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AllTypesView), "StringField");
        ModelValidationContextBase context = new ModelValidationContextBase(new ActionContext(), metadata, provider);

        string expected = Validation.For("MinLength", context.ModelMetadata.PropertyName, 128);
        string actual = adapter.GetErrorMessage(context);

        Assert.Equal(expected, actual);
    }
}
