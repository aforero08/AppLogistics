using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using Xunit;

namespace AppLogistics.Data.Logging.Tests;

public class LoggablePropertyTests
{
    private PropertyEntry textProperty;
    private PropertyEntry dateProperty;

    public LoggablePropertyTests()
    {
        using (DbContext context = TestingContext.Create())
        {
            TestModel model = new TestModel { Id = 1 };

            context.Set<TestModel>().Attach(model);
            context.Entry(model).State = EntityState.Modified;
            textProperty = context.Entry(model).Property(prop => prop.Title);
            dateProperty = context.Entry(model).Property(prop => prop.CreationDate);
        }
    }

    #region LoggableProperty(PropertyEntry entry, Object newValue)

    [Fact]
    public void LoggableProperty_IsNotModified()
    {
        textProperty.CurrentValue = "Original";
        textProperty.IsModified = false;

        Assert.False(new LoggableProperty(textProperty, "Original").IsModified);
    }

    [Fact]
    public void LoggableProperty_DifferentValues_IsNotModified()
    {
        textProperty.CurrentValue = "Current";
        textProperty.IsModified = false;

        Assert.False(new LoggableProperty(textProperty, "Original").IsModified);
    }

    [Fact]
    public void LoggableProperty_SameValues_IsNotModified()
    {
        textProperty.CurrentValue = "Original";
        textProperty.IsModified = true;

        Assert.False(new LoggableProperty(textProperty, "Original").IsModified);
    }

    [Fact]
    public void LoggableProperty_DifferentValues_IsModified()
    {
        textProperty.CurrentValue = "Current";
        textProperty.IsModified = true;

        Assert.True(new LoggableProperty(textProperty, "Original").IsModified);
    }

    #endregion LoggableProperty(PropertyEntry entry, Object newValue)

    #region ToString()

    [Fact]
    public void ToString_Modified_CurrentValueNull()
    {
        textProperty.CurrentValue = null;
        textProperty.IsModified = true;

        string actual = new LoggableProperty(textProperty, "Original").ToString();
        string expected = $"{textProperty.Metadata.Name}: \"Original\" => null";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToString_Modified_OriginalValueNull()
    {
        textProperty.CurrentValue = "Current";
        textProperty.IsModified = true;

        string expected = $"{textProperty.Metadata.Name}: null => \"Current\"";
        string actual = new LoggableProperty(textProperty, null).ToString();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToString_Modified_Date()
    {
        dateProperty.CurrentValue = new DateTime(2014, 6, 8, 14, 16, 19);
        dateProperty.IsModified = true;

        string expected = $"{dateProperty.Metadata.Name}: \"2010-04-03 18:33:17\" => \"2014-06-08 14:16:19\"";
        string actual = new LoggableProperty(dateProperty, new DateTime(2010, 4, 3, 18, 33, 17)).ToString();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToString_Modified_Json()
    {
        textProperty.CurrentValue = "Current\r\nValue";
        textProperty.IsModified = true;

        string expected = $"{textProperty.Metadata.Name}: 157.45 => \"Current\\r\\nValue\"";
        string actual = new LoggableProperty(textProperty, 157.45).ToString();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToString_NotModified()
    {
        textProperty.IsModified = false;

        string actual = new LoggableProperty(textProperty, "Original").ToString();
        string expected = $"{textProperty.Metadata.Name}: \"Original\"";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToString_NotModified_OriginalValueNull()
    {
        textProperty.CurrentValue = "Current";
        textProperty.IsModified = false;

        string expected = $"{textProperty.Metadata.Name}: null";
        string actual = new LoggableProperty(textProperty, null).ToString();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToString_NotModified_Date()
    {
        dateProperty.CurrentValue = new DateTime(2014, 6, 8, 14, 16, 19);
        dateProperty.IsModified = false;

        string actual = new LoggableProperty(dateProperty, new DateTime(2014, 6, 8, 14, 16, 19)).ToString();
        string expected = $"{dateProperty.Metadata.Name}: \"2014-06-08 14:16:19\"";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToString_NotModified_Json()
    {
        textProperty.CurrentValue = "Current\r\nValue";
        textProperty.IsModified = false;

        string actual = new LoggableProperty(textProperty, "Original\r\nValue").ToString();
        string expected = $"{textProperty.Metadata.Name}: \"Original\\r\\nValue\"";

        Assert.Equal(expected, actual);
    }

    #endregion ToString()
}
