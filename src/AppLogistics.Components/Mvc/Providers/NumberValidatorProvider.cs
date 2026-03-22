using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace AppLogistics.Components.Mvc;

public class NumberValidatorProvider : IClientModelValidatorProvider
{
    private HashSet<Type> NumericTypes { get; }

    public NumberValidatorProvider()
    {
        NumericTypes = new HashSet<Type>
        {
          typeof(byte),
          typeof(sbyte),
          typeof(short),
          typeof(ushort),
          typeof(int),
          typeof(uint),
          typeof(long),
          typeof(ulong),
          typeof(float),
          typeof(double),
          typeof(decimal)
        };
    }

    public void CreateValidators(ClientValidatorProviderContext context)
    {
        Type type = Nullable.GetUnderlyingType(context.ModelMetadata.ModelType) ?? context.ModelMetadata.ModelType;

        if (NumericTypes.Contains(type))
        {
            context.Results.Add(new ClientValidatorItem
            {
                Validator = new NumberValidator(),
                IsReusable = true
            });
        }
    }
}
