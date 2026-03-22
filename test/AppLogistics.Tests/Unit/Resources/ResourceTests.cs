using AppLogistics.Objects;
using AppLogistics.Tests;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace AppLogistics.Resources.Tests;

public class ResourceTests
{
    #region Set(String type)

    [Fact]
    public void Set_Same()
    {
        object expected = Resource.Set("Test");
        object actual = Resource.Set("Test");

        Assert.Same(expected, actual);
    }

    #endregion Set(String type)

    #region ForAction(String name)

    [Fact]
    public void ForAction_IsCaseInsensitive()
    {
        string actual = Resource.ForAction("create");
        string expected = "Create";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ForAction_NotFound_ReturnsNull()
    {
        Assert.Null(Resource.ForAction("Null"));
    }

    #endregion ForAction(String name)

    #region ForLookup(String type)

    [Fact]
    public void ForLookup_IsCaseInsensitive()
    {
        string actual = Resource.ForLookup("role");
        string expected = "Roles";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ForLookup_NotFound_ReturnsNull()
    {
        Assert.Null(Resource.ForLookup("Test"));
    }

    #endregion ForLookup(String type)

    #region ForString(String value)

    [Fact]
    public void ForString_IsCaseInsensitive()
    {
        string actual = Resource.ForString("all");
        string expected = "All";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ForString_NotFound_ReturnsNull()
    {
        Assert.Null(Resource.ForString("Null"));
    }

    #endregion ForString(String value)

    #region ForHeader(String model)

    [Fact]
    public void ForHeader_IsCaseInsensitive()
    {
        string actual = Resource.ForHeader("account");
        string expected = "Account";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ForHeader_NotFound_ReturnsNull()
    {
        Assert.Null(Resource.ForHeader("Test"));
    }

    #endregion ForHeader(String model)

    #region ForPage(String path)

    [Fact]
    public void ForPage_Path_IsCaseInsensitive()
    {
        string actual = Resource.ForPage("administrationrolesdetails");
        string expected = "Role details";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ForPage_PathNotFound_ReturnsNull()
    {
        Assert.Null(Resource.ForPage("Test"));
    }

    #endregion ForPage(String path)

    #region ForPage(IDictionary<String, Object> values)

    [Fact]
    public void ForPage_IsCaseInsensitive()
    {
        IDictionary<string, object> values = new Dictionary<string, object>();
        values["area"] = "administration";
        values["controller"] = "roles";
        values["action"] = "details";

        string actual = Resource.ForPage(values);
        string expected = "Role details";

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void ForPage_WithoutArea(string area)
    {
        IDictionary<string, object> values = new Dictionary<string, object>();
        values["controller"] = "profile";
        values["action"] = "edit";
        values["area"] = area;

        string actual = Resource.ForPage(values);
        string expected = "Profile edit";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ForPage_NotFound_ReturnsNull()
    {
        IDictionary<string, object> values = new Dictionary<string, object>
        {
            ["controller"] = null,
            ["action"] = null,
            ["area"] = null
        };

        Assert.Null(Resource.ForPage(values));
    }

    #endregion ForPage(IDictionary<String, Object> values)

    #region ForSiteMap(String area, String controller, String action)

    [Fact]
    public void ForSiteMap_IsCaseInsensitive()
    {
        string actual = Resource.ForSiteMap("administration", "roles", "index");
        string expected = "Roles";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ForSiteMap_WithoutControllerAndAction()
    {
        string actual = Resource.ForSiteMap("administration", null, null);
        string expected = "Administration";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ForSiteMap_NotFound_ReturnsNull()
    {
        Assert.Null(Resource.ForSiteMap("Test", "Test", "Test"));
    }

    #endregion ForSiteMap(String area, String controller, String action)

    #region ForPermission(String area)

    [Fact]
    public void ForPermission_IsCaseInsensitive()
    {
        string actual = Resource.ForPermission("administration");
        string expected = "Administration";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ForPermission_NotFound_ReturnsNull()
    {
        Assert.Null(Resource.ForPermission("Test"));
    }

    [Fact]
    public void ForPermission_NullArea_ReturnsNull()
    {
        Assert.Null(Resource.ForPermission(null));
    }

    #endregion ForPermission(String area)

    #region ForPermission(String area, String controller)

    [Fact]
    public void ForPermission_ReturnsControllerTitle()
    {
        string actual = Resource.ForPermission("Administration", "Roles");
        string expected = "Roles";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ForPermission_NotFoundController_ReturnsNull()
    {
        Assert.Null(Resource.ForPermission("", ""));
    }

    #endregion ForPermission(String area, String controller)

    #region ForPermission(String area, String controller, String action)

    [Fact]
    public void ForPermission_ReturnsActionTitle()
    {
        string actual = Resource.ForPermission("administration", "accounts", "index");
        string expected = "Index";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ForPermission_NotFoundAction_ReturnsNull()
    {
        Assert.Null(Resource.ForPermission("", "", ""));
    }

    #endregion ForPermission(String area, String controller, String action)

    #region ForProperty<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)

    [Fact]
    public void ForProperty_NotMemberLambdaExpression_ReturnNull()
    {
        Assert.Null(Resource.ForProperty<TestView, string>(view => view.ToString()));
    }

    [Fact]
    public void ForProperty_FromLambdaExpression()
    {
        string actual = Resource.ForProperty<AccountView, string>(account => account.Username);
        string expected = "Username";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ForProperty_FromLambdaExpressionRelation()
    {
        string actual = Resource.ForProperty<AccountEditView, int?>(account => account.RoleId);
        string expected = "Role";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ForProperty_NotFoundLambdaExpression_ReturnsNull()
    {
        Assert.Null(Resource.ForProperty<AccountView, int>(account => account.Id));
    }

    [Fact]
    public void ForProperty_NotFoundLambdaType_ReturnsNull()
    {
        Assert.Null(Resource.ForProperty<TestView, string>(test => test.Title));
    }

    #endregion ForProperty<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)

    #region ForProperty(String view, String name)

    [Fact]
    public void ForProperty_View()
    {
        string actual = Resource.ForProperty(nameof(AccountView), nameof(AccountView.Username));
        string expected = "Username";

        Assert.Equal(expected, actual);
    }

    #endregion ForProperty(String view, String name)

    #region ForProperty(Type view, String name)

    [Fact]
    public void ForProperty_IsCaseInsensitive()
    {
        string actual = Resource.ForProperty(typeof(AccountView), nameof(AccountView.Username).ToLower());
        string expected = "Username";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ForProperty_FromRelation()
    {
        string actual = Resource.ForProperty(typeof(object), nameof(Account) + nameof(Account.Username));
        string expected = "Username";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ForProperty_FromMultipleRelations()
    {
        string actual = Resource.ForProperty(typeof(RoleView), nameof(Account) + nameof(Role) + nameof(Account) + nameof(Account.Username));
        string expected = "Username";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ForProperty_NotFoundProperty_ReturnsNull()
    {
        Assert.Null(Resource.ForProperty(typeof(AccountView), "Id"));
    }

    [Fact]
    public void ForProperty_NotFoundTypeProperty_ReturnsNull()
    {
        Assert.Null(Resource.ForProperty(typeof(TestView), "Title"));
    }

    [Fact]
    public void ForProperty_NullKey_ReturnsNull()
    {
        Assert.Null(Resource.ForProperty(typeof(RoleView), null));
    }

    #endregion ForProperty(Type view, String name)

    #region ForProperty(Expression expression)

    [Fact]
    public void ForProperty_NotMemberExpression_ReturnNull()
    {
        Expression<Func<TestView, string>> lambda = (view) => view.ToString();

        Assert.Null(Resource.ForProperty(lambda.Body));
    }

    [Fact]
    public void ForProperty_FromExpression()
    {
        Expression<Func<AccountView, string>> lambda = (account) => account.Username;

        string actual = Resource.ForProperty(lambda.Body);
        string expected = "Username";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ForProperty_FromExpressionRelation()
    {
        Expression<Func<AccountEditView, int?>> lambda = (account) => account.RoleId;

        string actual = Resource.ForProperty(lambda.Body);
        string expected = "Role";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ForProperty_NotFoundExpression_ReturnsNull()
    {
        Expression<Func<AccountView, int>> lambda = (account) => account.Id;

        Assert.Null(Resource.ForProperty(lambda.Body));
    }

    [Fact]
    public void ForProperty_NotFoundType_ReturnsNull()
    {
        Expression<Func<TestView, string>> lambda = (test) => test.Title;

        Assert.Null(Resource.ForProperty(lambda.Body));
    }

    #endregion ForProperty(Expression expression)
}
