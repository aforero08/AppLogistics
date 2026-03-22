using AppLogistics.Components.Security;
using AppLogistics.Tests;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NSubstitute;
using System.Security.Claims;
using Xunit;

namespace AppLogistics.Components.Mvc.Tests;

public class AuthorizeTagHelperTests
{
    private IAuthorization authorization;
    private AuthorizeTagHelper helper;
    private TagHelperOutput output;

    public AuthorizeTagHelperTests()
    {
        output = new TagHelperOutput("authorize", new TagHelperAttributeList(), (useCachedResult, encoder) => null);
        helper = new AuthorizeTagHelper(authorization = Substitute.For<IAuthorization>())
        {
            ViewContext = HtmlHelperFactory.CreateHtmlHelper().ViewContext
        };
    }

    #region Process(TagHelperContext context, TagHelperOutput output)

    [Fact]
    public void Process_NoAuthorization_RemovedWrappingTag()
    {
        helper = new AuthorizeTagHelper(null)
        {
            ViewContext = HtmlHelperFactory.CreateHtmlHelper().ViewContext
        };

        output.PostContent.SetContent("PostContent");
        output.PostElement.SetContent("PostElement");
        output.PreContent.SetContent("PreContent");
        output.PreElement.SetContent("PreElement");
        output.Content.SetContent("Content");
        output.TagName = "TagName";

        helper.Process(null, output);

        Assert.Equal("PostContent", output.PostContent.GetContent());
        Assert.Equal("PostElement", output.PostElement.GetContent());
        Assert.Equal("PreContent", output.PreContent.GetContent());
        Assert.Equal("PreElement", output.PreElement.GetContent());
        Assert.Equal("Content", output.Content.GetContent());
        Assert.Null(output.TagName);
    }

    [Theory]
    [InlineData("A", "B", "C", "D", "E", "F", "A", "B", "C")]
    [InlineData(null, null, null, "A", "B", "C", "A", "B", "C")]
    [InlineData(null, null, null, null, null, null, null, null, null)]
    public void Process_NotAuthorized_SurpressesOutput(
        string area, string controller, string action,
        string routeArea, string routeController, string routeAction,
        string authArea, string authController, string authAction)
    {
        authorization.IsGrantedFor(Arg.Any<int?>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(true);
        authorization.IsGrantedFor(1, authArea, authController, authAction).Returns(false);

        helper.ViewContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Returns(new Claim(ClaimTypes.NameIdentifier, "1"));
        helper.ViewContext.RouteData.Values["controller"] = routeController;
        helper.ViewContext.RouteData.Values["action"] = routeAction;
        helper.ViewContext.RouteData.Values["area"] = routeArea;

        output.PostContent.SetContent("PostContent");
        output.PostElement.SetContent("PostElement");
        output.PreContent.SetContent("PreContent");
        output.PreElement.SetContent("PreElement");
        output.Content.SetContent("Content");
        output.TagName = "TagName";

        helper.Controller = controller;
        helper.Action = action;
        helper.Area = area;

        helper.Process(null, output);

        Assert.Empty(output.PostContent.GetContent());
        Assert.Empty(output.PostElement.GetContent());
        Assert.Empty(output.PreContent.GetContent());
        Assert.Empty(output.PreElement.GetContent());
        Assert.Empty(output.Content.GetContent());
        Assert.Null(output.TagName);
    }

    [Theory]
    [InlineData("A", "B", "C", "D", "E", "F", "A", "B", "C")]
    [InlineData(null, null, null, "A", "B", "C", "A", "B", "C")]
    [InlineData(null, null, null, null, null, null, null, null, null)]
    public void Process_RemovesWrappingTag(
        string area, string controller, string action,
        string routeArea, string routeController, string routeAction,
        string authArea, string authController, string authAction)
    {
        authorization.IsGrantedFor(1, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(false);
        authorization.IsGrantedFor(1, authArea, authController, authAction).Returns(true);

        helper.ViewContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Returns(new Claim(ClaimTypes.NameIdentifier, "1"));
        helper.ViewContext.RouteData.Values["controller"] = routeController;
        helper.ViewContext.RouteData.Values["action"] = routeAction;
        helper.ViewContext.RouteData.Values["area"] = routeArea;

        output.PostContent.SetContent("PostContent");
        output.PostElement.SetContent("PostElement");
        output.PreContent.SetContent("PreContent");
        output.PreElement.SetContent("PreElement");
        output.Content.SetContent("Content");
        output.TagName = "TagName";

        helper.Controller = controller;
        helper.Action = action;
        helper.Area = area;

        helper.Process(null, output);

        Assert.Equal("PostContent", output.PostContent.GetContent());
        Assert.Equal("PostElement", output.PostElement.GetContent());
        Assert.Equal("PreContent", output.PreContent.GetContent());
        Assert.Equal("PreElement", output.PreElement.GetContent());
        Assert.Equal("Content", output.Content.GetContent());
        Assert.Null(output.TagName);
    }

    #endregion Process(TagHelperContext context, TagHelperOutput output)
}
