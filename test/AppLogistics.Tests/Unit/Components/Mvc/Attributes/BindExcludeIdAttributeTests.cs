using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using NSubstitute;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace AppLogistics.Components.Mvc.Tests
{
    public class BindExcludeIdAttributeTests
    {
        private static PropertyInfo ResolveProperty(string name)
        {
            var existing = typeof(object).GetProperties().FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return existing ?? typeof(object).GetProperty("ToString");
        }

        [Theory]
        [InlineData("id", true)]
        [InlineData("iD", true)]
        [InlineData("ID", true)]
        [InlineData("Id", false)]
        [InlineData("Prop", true)]
        public void PropertyFilter_Id(string property, bool isIncluded)
        {
            PropertyInfo propInfo = ResolveProperty(property);
            ModelMetadataIdentity identity = ModelMetadataIdentity.ForProperty(propInfo, propInfo.DeclaringType, propInfo.DeclaringType);
            ModelMetadata metadata = Substitute.ForPartsOf<ModelMetadata>(identity);

            bool actual = new BindExcludeIdAttribute().PropertyFilter(metadata);
            bool expected = isIncluded;

            Assert.Equal(expected, actual);
        }
    }
}
