using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using NSubstitute;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace AppLogistics.Components.Mvc.Tests;

public class BindExcludeIdAttributeTests
{
    private sealed class TestModel
    {
        public int Id { get; set; }
        public string Prop { get; set; }
    }

    private static PropertyInfo ResolveProperty(string name)
    {
        return typeof(TestModel)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .First(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    [Theory]
    [InlineData("id", false)]
    [InlineData("iD", false)]
    [InlineData("ID", false)]
    [InlineData("Id", false)]
    [InlineData("Prop", true)]
    public void PropertyFilter_Id(string property, bool isIncluded)
    {
        PropertyInfo propInfo = ResolveProperty(property);
        ModelMetadataIdentity identity = ModelMetadataIdentity.ForProperty(propInfo, propInfo.DeclaringType!, propInfo.DeclaringType!);
        ModelMetadata metadata = Substitute.ForPartsOf<ModelMetadata>(identity);

        bool actual = new BindExcludeIdAttribute().PropertyFilter(metadata);

        Assert.Equal(isIncluded, actual);
    }
}
