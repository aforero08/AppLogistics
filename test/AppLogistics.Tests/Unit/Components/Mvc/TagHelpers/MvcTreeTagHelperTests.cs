using AppLogistics.Components.Extensions;
using AppLogistics.Components.Extensions.Tests;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace AppLogistics.Components.Mvc.Tests;

public class MvcTreeTagHelperTests
{
    private MvcTreeTagHelper helper;
    private TagHelperOutput output;

    public MvcTreeTagHelperTests()
    {
        MvcTree tree = new MvcTree();
        tree.SelectedIds.Add(123456);
        tree.Nodes.Add(new MvcTreeNode("Test"));
        tree.Nodes[0].Children.Add(new MvcTreeNode(4567, "Test2"));
        tree.Nodes[0].Children.Add(new MvcTreeNode(123456, "Test1"));

        EmptyModelMetadataProvider provider = new EmptyModelMetadataProvider();
        ModelExplorer explorer = new ModelExplorer(provider, provider.GetMetadataForProperty(typeof(MvcTreeView), "MvcTree"), tree);

        helper = new MvcTreeTagHelper
        {
            For = new ModelExpression("MvcTree", explorer)
        };
        output = new TagHelperOutput("div", new TagHelperAttributeList(), (useCachedResult, encoder) => null);
    }

    #region Process(TagHelperContext context, TagHelperOutput output)

    [Fact]
    public void Process_AddsDataForAttribute()
    {
        helper.Process(null, output);

        object expected = "MvcTree.SelectedIds";
        object actual = output.Attributes["data-for"].Value;

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(false, "mvc-tree")]
    [InlineData(true, "mvc-tree mvc-tree-readonly")]
    public void Process_AddsClasses(bool isReadonly, string classes)
    {
        helper.Readonly = isReadonly;

        helper.Process(null, output);

        object expected = classes;
        object actual = output.Attributes["class"].Value;

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(false, "mvc-tree test")]
    [InlineData(true, "mvc-tree mvc-tree-readonly test")]
    public void Process_AppendsClasses(bool isReadonly, string classes)
    {
        output.Attributes.Add("class", "test");

        helper.Readonly = isReadonly;

        helper.Process(null, output);

        object expected = classes;
        object actual = output.Attributes["class"].Value;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Process_BuildsTree()
    {
        helper.Process(null, output);

        string actual = output.Content.GetContent();
        string expected =
            "<div class=\"mvc-tree-ids\">" +
                "<input name=\"MvcTree.SelectedIds\" type=\"hidden\" value=\"123456\" />" +
            "</div>" +
            "<ul class=\"mvc-tree-view\">" +
                "<li class=\"mvc-tree-branch\">" +
                    "<i></i><a href=\"#\">Test</a>" +
                    "<ul>" +
                        "<li data-id=\"4567\">" +
                            "<i></i><a href=\"#\">Test2</a>" +
                        "</li>" +
                        "<li class=\"mvc-tree-checked\" data-id=\"123456\">" +
                            "<i></i><a href=\"#\">Test1</a>" +
                        "</li>" +
                    "</ul>" +
                "</li>" +
            "</ul>";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Process_BuildsCollapsedTree()
    {
        helper.HideDepth = 1;

        helper.Process(null, output);

        string actual = output.Content.GetContent();
        string expected =
            "<div class=\"mvc-tree-ids\">" +
                "<input name=\"MvcTree.SelectedIds\" type=\"hidden\" value=\"123456\" />" +
            "</div>" +
            "<ul class=\"mvc-tree-view\">" +
                "<li class=\"mvc-tree-branch mvc-tree-collapsed\">" +
                    "<i></i><a href=\"#\">Test</a>" +
                    "<ul>" +
                        "<li data-id=\"4567\">" +
                            "<i></i><a href=\"#\">Test2</a>" +
                        "</li>" +
                        "<li class=\"mvc-tree-checked\" data-id=\"123456\">" +
                            "<i></i><a href=\"#\">Test1</a>" +
                        "</li>" +
                    "</ul>" +
                "</li>" +
            "</ul>";

        Assert.Equal(expected, actual);
    }

    #endregion Process(TagHelperContext context, TagHelperOutput output)
}
