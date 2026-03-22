using AppLogistics.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using Xunit;

namespace AppLogistics.Components.Mvc.Tests;

public class NumberValidatorTests
{
    #region AddValidation(ClientModelValidationContext context)

    [Fact]
    public void AddValidation_Number()
    {
        IModelMetadataProvider provider = new EmptyModelMetadataProvider();
        ModelMetadata metadata = provider.GetMetadataForType(typeof(int));
        Dictionary<string, string> attributes = new Dictionary<string, string>();
        ClientModelValidationContext context = new ClientModelValidationContext(new ActionContext(), metadata, provider, attributes);

        new NumberValidator().AddValidation(context);

        Assert.Equal(2, attributes.Count);
        Assert.Equal("true", attributes["data-val"]);
        Assert.Equal(Validation.For("Numeric", "Int32"), attributes["data-val-number"]);
    }

    #endregion AddValidation(ClientModelValidationContext context)
}
