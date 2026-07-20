using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using Xunit;

namespace AppLogistics.Components.Mvc.Tests;

public class AjaxOnlyAttributeTests
{
    #region IsValidForRequest(RouteContext context, ActionDescriptor action)

    [Theory]
    [InlineData("", false)]
    [InlineData("XMLHttpRequest", true)]
    public void IsValidForRequest_Ajax(string header, bool isValid)
    {
        RouteContext context = new RouteContext(Substitute.For<HttpContext>());
        context.HttpContext.Request.Headers["X-Requested-With"].Returns(new StringValues(header));

        bool actual = new AjaxOnlyAttribute().IsValidForRequest(context, null);
        bool expected = isValid;

        Assert.Equal(expected, actual);
    }

    #endregion IsValidForRequest(RouteContext context, ActionDescriptor action)
}
