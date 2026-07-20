using AppLogistics.Objects;
using AppLogistics.Resources;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System.Reflection;
using Xunit;

namespace AppLogistics.Components.Mvc.Tests;

public class DisplayMetadataProviderTests
{
    #region CreateDisplayMetadata(DisplayMetadataProviderContext context)

    [Fact]
    public void CreateDisplayMetadata_SetsDisplayName()
    {
        DisplayMetadataProvider provider = new DisplayMetadataProvider();
        PropertyInfo titleProp = typeof(RoleView).GetProperty("Title");
        DisplayMetadataProviderContext context = new DisplayMetadataProviderContext(
            ModelMetadataIdentity.ForProperty(titleProp, typeof(RoleView), typeof(RoleView)),
            ModelAttributes.GetAttributesForType(typeof(RoleView)));

        provider.CreateDisplayMetadata(context);

        string expected = Resource.ForProperty(typeof(RoleView), "Title");
        string actual = context.DisplayMetadata.DisplayName();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CreateDisplayMetadata_NullContainerType_DoesNotSetDisplayName()
    {
        DisplayMetadataProvider provider = new DisplayMetadataProvider();
        DisplayMetadataProviderContext context = new DisplayMetadataProviderContext(
               ModelMetadataIdentity.ForType(typeof(RoleView)),
               ModelAttributes.GetAttributesForType(typeof(RoleView)));

        provider.CreateDisplayMetadata(context);

        Assert.Null(context.DisplayMetadata.DisplayName);
    }

    #endregion CreateDisplayMetadata(DisplayMetadataProviderContext context)
}
