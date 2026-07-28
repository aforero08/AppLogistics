using AppLogistics.Resources;
using AppLogistics.Tests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using Xunit;

namespace AppLogistics.Components.Mvc.Tests;

public class MaxValueAdapterTests
{
    private MaxValueAdapter adapter;
    private ClientModelValidationContext context;
    private Dictionary<string, string> attributes;

    public MaxValueAdapterTests()
    {
        attributes = new Dictionary<string, string>();
        adapter = new MaxValueAdapter(new MaxValueAttribute(128));
        IModelMetadataProvider provider = new EmptyModelMetadataProvider();
        ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AllTypesView), "Int32Field");
        context = new ClientModelValidationContext(new ActionContext(), metadata, provider, attributes);
    }

    #region AddValidation(ClientModelValidationContext context)

    [Fact]
    public void AddValidation_MaxValue()
    {
        adapter.AddValidation(context);

        Assert.Equal(3, attributes.Count);
        Assert.Equal("true", attributes["data-val"]);
        Assert.Equal("128", attributes["data-val-range-max"]);
        Assert.Equal(Validation.For("MaxValue", context.ModelMetadata.PropertyName, 128), attributes["data-val-range"]);
    }

    #endregion AddValidation(ClientModelValidationContext context)

    #region GetErrorMessage(ModelValidationContextBase context)

    [Fact]
    public void GetErrorMessage_MaxValue()
    {
        string expected = Validation.For("MaxValue", context.ModelMetadata.PropertyName, 128);
        string actual = adapter.GetErrorMessage(context);

        Assert.Equal(expected, actual);
    }

    #endregion GetErrorMessage(ModelValidationContextBase context)
}
