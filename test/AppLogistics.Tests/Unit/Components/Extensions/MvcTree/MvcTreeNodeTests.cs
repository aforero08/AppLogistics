using Xunit;

namespace AppLogistics.Components.Extensions.Tests;

public class MvcTreeNodeTests
{
    #region MvcTreeNode(String title)

    [Fact]
    public void MvcTreeNode_SetsTitle()
    {
        MvcTreeNode actual = new MvcTreeNode("Title");

        Assert.Equal("Title", actual.Title);
        Assert.Empty(actual.Children);
        Assert.Null(actual.Id);
    }

    #endregion MvcTreeNode(String title)

    #region MvcTreeNode(Int32? id, String title)

    [Fact]
    public void MvcTreeNode_SetsIdAndTitle()
    {
        MvcTreeNode actual = new MvcTreeNode(1, "Title");

        Assert.Equal("Title", actual.Title);
        Assert.Empty(actual.Children);
        Assert.Equal(1, actual.Id);
    }

    #endregion MvcTreeNode(Int32? id, String title)
}
