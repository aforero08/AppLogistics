using System;
using Xunit;

namespace AppLogistics.Components.Security.Tests;

public class AuthorizeAsAttributeTests
{
    #region AuthorizeAsAttribute(String action)

    [Fact]
    public void AuthorizeAsAttribute_NullAction_Throws()
    {
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new AuthorizeAsAttribute(null));
        Assert.Equal("action", exception.ParamName);
    }

    [Fact]
    public void AuthorizeAsAttribute_SetsAction()
    {
        string actual = new AuthorizeAsAttribute("Action").Action;
        string expected = "Action";

        Assert.Equal(expected, actual);
    }

    #endregion AuthorizeAsAttribute(String action)
}
