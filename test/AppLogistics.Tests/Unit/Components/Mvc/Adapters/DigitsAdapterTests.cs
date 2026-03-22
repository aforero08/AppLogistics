using AppLogistics.Resources;
using AppLogistics.Tests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using Xunit;

namespace AppLogistics.Components.Mvc.Tests;

public class DigitsAdapterTests
{
    private DigitsAdapter adapter;
    private ClientModelValidationContext context;
    private Dictionary<string, string> attributes;

    public DigitsAdapterTests()
    {
        attributes = new Dictionary<string, string>();
        adapter = new DigitsAdapter(new DigitsAttribute());
        IModelMetadataProvider provider = new EmptyModelMetadataProvider();
        ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AllTypesView), "StringField");
        context = new ClientModelValidationContext(new ActionContext(), metadata, provider, attributes);
    }

    #region AddValidation(ClientModelValidationContext context)

    [Fact]
    public void AddValidation_Digits()
    {
        adapter.AddValidation(context);

        Assert.Equal(2, attributes.Count);
        Assert.Equal("true", attributes["data-val"]);
        Assert.Equal(Validation.For("Digits", context.ModelMetadata.PropertyName), attributes["data-val-digits"]);
    }

    #endregion AddValidation(ClientModelValidationContext context)

    #region GetErrorMessage(ModelValidationContextBase context)

    [Fact]
    public void GetErrorMessage_Digits()
    {
        string expected = Validation.For("Digits", context.ModelMetadata.PropertyName);
        string actual = adapter.GetErrorMessage(context);

        Assert.Equal(expected, actual);
    }

    #endregion GetErrorMessage(ModelValidationContextBase context)
}
