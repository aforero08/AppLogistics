using AppLogistics.Tests;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace AppLogistics.Components.Mvc.Tests;

public class TrimmingModelBinderTests
{
    private TrimmingModelBinder binder;
    private ModelBindingContext context;

    public TrimmingModelBinderTests()
    {
        binder = new TrimmingModelBinder();
        context = new DefaultModelBindingContext
        {
            ModelState = new ModelStateDictionary(),
            ValueProvider = Substitute.For<IValueProvider>()
        };
    }

    #region BindModelAsync(ModelBindingContext context)

    [Fact]
    public async Task BindModelAsync_NoValue()
    {
        ModelMetadata metadata = new EmptyModelMetadataProvider().GetMetadataForType(typeof(string));
        context.ValueProvider.GetValue(context.ModelName).Returns(ValueProviderResult.None);
        context.ModelMetadata = metadata;

        await binder.BindModelAsync(context);

        ModelBindingResult expected = new ModelBindingResult();
        ModelBindingResult actual = context.Result;

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public async Task BindModelAsync_Null(string value)
    {
        ModelMetadata metadata = new EmptyModelMetadataProvider().GetMetadataForProperty(typeof(AllTypesView), "StringField");
        context.ValueProvider.GetValue("StringField").Returns(new ValueProviderResult(value));
        context.ModelName = "StringField";
        context.ModelMetadata = metadata;

        await binder.BindModelAsync(context);

        ModelBindingResult expected = ModelBindingResult.Success(null);
        ModelBindingResult actual = context.Result;

        Assert.Equal(expected.IsModelSet, actual.IsModelSet);
        Assert.Equal(expected.Model, actual.Model);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public async Task BindModelAsync_Empty(string value)
    {
        ModelMetadata metadata = Substitute.For<ModelMetadata>(ModelMetadataIdentity.ForType(typeof(string)));
        context.ValueProvider.GetValue("StringField").Returns(new ValueProviderResult(value));
        metadata.ConvertEmptyStringToNull.Returns(false);
        context.ModelName = "StringField";
        context.ModelMetadata = metadata;

        await binder.BindModelAsync(context);

        ModelBindingResult expected = ModelBindingResult.Success("");
        ModelBindingResult actual = context.Result;

        Assert.Equal(expected.IsModelSet, actual.IsModelSet);
        Assert.Equal(expected.Model, actual.Model);
    }

    [Fact]
    public async Task BindModelAsync_Trimmed()
    {
        ModelMetadata metadata = new EmptyModelMetadataProvider().GetMetadataForProperty(typeof(AllTypesView), "StringField");
        context.ValueProvider.GetValue("StringField").Returns(new ValueProviderResult(" Value "));
        context.ModelName = "StringField";
        context.ModelMetadata = metadata;

        await binder.BindModelAsync(context);

        ModelBindingResult expected = ModelBindingResult.Success("Value");
        ModelBindingResult actual = context.Result;

        Assert.Equal(expected.IsModelSet, actual.IsModelSet);
        Assert.Equal(expected.Model, actual.Model);
    }

    #endregion BindModelAsync(ModelBindingContext context)
}
