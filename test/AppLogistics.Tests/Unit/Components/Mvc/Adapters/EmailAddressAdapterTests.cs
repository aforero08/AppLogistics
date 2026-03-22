using AppLogistics.Resources;
using AppLogistics.Tests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace AppLogistics.Components.Mvc.Tests;

public class EmailAddressAdapterTests
{
    private EmailAddressAdapter adapter;
    private ClientModelValidationContext context;
    private Dictionary<string, string> attributes;

    public EmailAddressAdapterTests()
    {
        attributes = new Dictionary<string, string>();
        adapter = new EmailAddressAdapter(new EmailAddressAttribute());
        IModelMetadataProvider provider = new EmptyModelMetadataProvider();
        ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AllTypesView), "StringField");
        context = new ClientModelValidationContext(new ActionContext(), metadata, provider, attributes);
    }

    #region AddValidation(ClientModelValidationContext context)

    [Fact]
    public void AddValidation_Email()
    {
        adapter.AddValidation(context);

        Assert.Equal(2, attributes.Count);
        Assert.Equal("true", attributes["data-val"]);
        Assert.Equal(Validation.For("Email", context.ModelMetadata.PropertyName), attributes["data-val-email"]);
    }

    #endregion AddValidation(ClientModelValidationContext context)

    #region GetErrorMessage(ModelValidationContextBase context)

    [Fact]
    public void GetErrorMessage_Email()
    {
        string expected = Validation.For("Email", context.ModelMetadata.PropertyName);
        string actual = adapter.GetErrorMessage(context);

        Assert.Equal(expected, actual);
    }

    #endregion GetErrorMessage(ModelValidationContextBase context)
}
