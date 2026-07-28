using AppLogistics.Resources;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Xunit;

namespace AppLogistics.Components.Mvc.Tests;

public class FileSizeAttributeTests
{
    private FileSizeAttribute attribute;

    public FileSizeAttributeTests()
    {
        attribute = new FileSizeAttribute(12.25);
    }

    #region FileSizeAttribute(Double maximumMB)

    [Fact]
    public void FileSizeAttribute_SetsMaximumMB()
    {
        decimal actual = new FileSizeAttribute(12.25).MaximumMB;
        decimal expected = 12.25M;

        Assert.Equal(expected, actual);
    }

    #endregion FileSizeAttribute(Double maximumMB)

    #region FormatErrorMessage(String name)

    [Fact]
    public void FormatErrorMessage_ForName()
    {
        attribute = new FileSizeAttribute(12.25);

        string expected = Validation.For("FileSize", "File", attribute.MaximumMB);
        string actual = attribute.FormatErrorMessage("File");

        Assert.Equal(expected, actual);
    }

    #endregion FormatErrorMessage(String name)

    #region IsValid(Object value)

    [Fact]
    public void IsValid_Null()
    {
        Assert.True(attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_NotIFormFileValueIsValid()
    {
        Assert.True(attribute.IsValid("100"));
    }

    [Theory]
    [InlineData(240546)]
    [InlineData(12845056)]
    public void IsValid_LowerOrEqualFileSize(int size)
    {
        IFormFile file = Substitute.For<IFormFile>();
        file.Length.Returns(size);

        Assert.True(attribute.IsValid(file));
    }

    [Fact]
    public void IsValid_GreaterThanMaximumIsNotValid()
    {
        IFormFile file = Substitute.For<IFormFile>();
        file.Length.Returns(12845057);

        Assert.False(attribute.IsValid(file));
    }

    [Theory]
    [InlineData(240546, 4574)]
    [InlineData(12840000, 5056)]
    public void IsValid_LowerOrEqualFileSizes(int firstFileSize, int secondFileSize)
    {
        IFormFile[] files = { Substitute.For<IFormFile>(), Substitute.For<IFormFile>(), null };
        files[1].Length.Returns(secondFileSize);
        files[0].Length.Returns(firstFileSize);

        Assert.True(attribute.IsValid(files));
    }

    [Fact]
    public void IsValid_GreaterThanMaximumSizesAreNotValid()
    {
        IFormFile[] files = { Substitute.For<IFormFile>(), Substitute.For<IFormFile>(), null };
        files[1].Length.Returns(12840000);
        files[0].Length.Returns(5057);

        Assert.False(attribute.IsValid(files));
    }

    #endregion IsValid(Object value)
}
