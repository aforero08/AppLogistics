using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NSubstitute;
using System;
using Xunit;

namespace AppLogistics.Components.Mvc.Tests;

public class FormLabelTagHelperTests
{
    #region Process(TagHelperContext context, TagHelperOutput output)

    [Theory]
    [InlineData(typeof(string), true, null, "*")]
    [InlineData(typeof(string), true, true, "*")]
    [InlineData(typeof(string), true, false, "")]
    [InlineData(typeof(string), false, null, "")]
    [InlineData(typeof(string), false, true, "*")]
    [InlineData(typeof(string), false, false, "")]
    [InlineData(typeof(bool), true, null, "")]
    [InlineData(typeof(bool), true, true, "*")]
    [InlineData(typeof(bool), true, false, "")]
    public void Process_Label(Type type, bool metadataRequired, bool? required, string require)
    {
        ModelMetadata metadata = Substitute.For<ModelMetadata>(ModelMetadataIdentity.ForType(type));
        TagHelperAttribute[] attributes = { new TagHelperAttribute("for", "Test") };
        FormLabelTagHelper helper = new FormLabelTagHelper();

        TagHelperOutput output = new TagHelperOutput("label", new TagHelperAttributeList(attributes), (useCache, encoder) => null);
        helper.For = new ModelExpression("Total.Sum", new ModelExplorer(new EmptyModelMetadataProvider(), metadata, null));
        metadata.IsRequired.Returns(metadataRequired);
        metadata.DisplayName.Returns("Title");
        helper.Required = required;

        helper.Process(null, output);

        Assert.Equal("Test", output.Attributes["for"].Value);
        Assert.Equal($"<span class=\"require\">{require}</span>", output.Content.GetContent());
    }

    #endregion Process(TagHelperContext context, TagHelperOutput output)
}
