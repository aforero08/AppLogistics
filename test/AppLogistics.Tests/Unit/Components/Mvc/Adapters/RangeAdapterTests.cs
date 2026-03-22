using AppLogistics.Resources;
using AppLogistics.Tests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace AppLogistics.Components.Mvc.Tests;

public class RangeAdapterTests
{
    [Fact]
    public void GetErrorMessage_Range()
    {
        IModelMetadataProvider provider = new EmptyModelMetadataProvider();
        var attribute = new RangeAttribute(4, 128);
        RangeAdapter adapter = new RangeAdapter(attribute);
        ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AllTypesView), "Int32Field");
        ModelValidationContextBase context = new ModelValidationContextBase(new ActionContext(), metadata, provider);

        string expected = Validation.For("Range", context.ModelMetadata.PropertyName, 4, 128);
        string actual = adapter.GetErrorMessage(context);

        Assert.Equal(expected, actual);
    }
}
