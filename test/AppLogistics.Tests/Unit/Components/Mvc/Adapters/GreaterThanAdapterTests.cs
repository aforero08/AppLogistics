using AppLogistics.Resources;
using AppLogistics.Tests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using Xunit;

namespace AppLogistics.Components.Mvc.Tests;

public class GreaterThanAdapterTests
{
    private GreaterThanAdapter adapter;
    private ClientModelValidationContext context;
    private Dictionary<string, string> attributes;

    public GreaterThanAdapterTests()
    {
        attributes = new Dictionary<string, string>();
        adapter = new GreaterThanAdapter(new GreaterThanAttribute(128));
        IModelMetadataProvider provider = new EmptyModelMetadataProvider();
        ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AllTypesView), "Int32Field");
        context = new ClientModelValidationContext(new ActionContext(), metadata, provider, attributes);
    }

    #region AddValidation(ClientModelValidationContext context)

    [Fact]
    public void AddValidation_GreaterThan()
    {
        adapter.AddValidation(context);

        Assert.Equal(3, attributes.Count);
        Assert.Equal("true", attributes["data-val"]);
        Assert.Equal("128", attributes["data-val-greater-min"]);
        Assert.Equal(Validation.For("GreaterThan", context.ModelMetadata.PropertyName, 128), attributes["data-val-greater"]);
    }

    #endregion AddValidation(ClientModelValidationContext context)

    #region GetErrorMessage(ModelValidationContextBase context)

    [Fact]
    public void GetErrorMessage_GreaterThan()
    {
        string expected = Validation.For("GreaterThan", context.ModelMetadata.PropertyName, 128);
        string actual = adapter.GetErrorMessage(context);

        Assert.Equal(expected, actual);
    }

    #endregion GetErrorMessage(ModelValidationContextBase context)
}
