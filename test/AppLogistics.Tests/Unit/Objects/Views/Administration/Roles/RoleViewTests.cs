using AppLogistics.Components.Extensions;
using Xunit;

namespace AppLogistics.Objects.Tests;

public class RoleViewTests
{
    #region RoleView()

    [Fact]
    public void RoleView_CreatesEmpty()
    {
        MvcTree actual = new RoleView().Permissions;

        Assert.Empty(actual.SelectedIds);
        Assert.Empty(actual.Nodes);
    }

    #endregion RoleView()
}
