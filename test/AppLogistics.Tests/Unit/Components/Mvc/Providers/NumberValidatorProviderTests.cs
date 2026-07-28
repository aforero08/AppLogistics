using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AppLogistics.Components.Mvc.Tests;

public class NumberValidatorProviderTests
{
    #region CreateValidators(ClientValidatorProviderContext context)

    [Theory]
    [InlineData(typeof(byte))]
    [InlineData(typeof(byte?))]
    [InlineData(typeof(sbyte))]
    [InlineData(typeof(sbyte?))]
    [InlineData(typeof(short))]
    [InlineData(typeof(short?))]
    [InlineData(typeof(ushort))]
    [InlineData(typeof(ushort?))]
    [InlineData(typeof(int))]
    [InlineData(typeof(int?))]
    [InlineData(typeof(uint))]
    [InlineData(typeof(uint?))]
    [InlineData(typeof(long))]
    [InlineData(typeof(long?))]
    [InlineData(typeof(ulong))]
    [InlineData(typeof(ulong?))]
    [InlineData(typeof(float))]
    [InlineData(typeof(float?))]
    [InlineData(typeof(double))]
    [InlineData(typeof(double?))]
    [InlineData(typeof(decimal))]
    [InlineData(typeof(decimal?))]
    public void CreateValidators_ForNumber(Type type)
    {
        ModelMetadata metadata = new EmptyModelMetadataProvider().GetMetadataForType(type);
        ClientValidatorProviderContext context = new ClientValidatorProviderContext(metadata, new List<ClientValidatorItem>());

        new NumberValidatorProvider().CreateValidators(context);

        ClientValidatorItem actual = context.Results.Single();

        Assert.IsType<NumberValidator>(actual.Validator);
        Assert.Null(actual.ValidatorMetadata);
        Assert.True(actual.IsReusable);
    }

    [Fact]
    public void CreateValidators_DoesNotCreate()
    {
        ModelMetadata metadata = new EmptyModelMetadataProvider().GetMetadataForType(typeof(string));
        ClientValidatorProviderContext context = new ClientValidatorProviderContext(metadata, new List<ClientValidatorItem>());

        new NumberValidatorProvider().CreateValidators(context);

        Assert.Empty(context.Results);
    }

    #endregion CreateValidators(ClientValidatorProviderContext context)
}
