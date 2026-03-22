using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using System;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Components.Mvc;

public class ValidationAdapterProvider : IValidationAttributeAdapterProvider
{
    public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
    {
        Type type = attribute.GetType();
        if (type == typeof(RequiredAttribute))
        {
            return new RequiredAdapter((RequiredAttribute)attribute);
        }

        if (type == typeof(StringLengthAttribute))
        {
            return new StringLengthAdapter((StringLengthAttribute)attribute);
        }

        if (type == typeof(EmailAddressAttribute))
        {
            return new EmailAddressAdapter((EmailAddressAttribute)attribute);
        }

        if (type == typeof(GreaterThanAttribute))
        {
            return new GreaterThanAdapter((GreaterThanAttribute)attribute);
        }

        if (type == typeof(AcceptFilesAttribute))
        {
            return new AcceptFilesAdapter((AcceptFilesAttribute)attribute);
        }

        if (type == typeof(MinLengthAttribute))
        {
            return new MinLengthAdapter((MinLengthAttribute)attribute);
        }

        if (type == typeof(MaxValueAttribute))
        {
            return new MaxValueAdapter((MaxValueAttribute)attribute);
        }

        if (type == typeof(MinValueAttribute))
        {
            return new MinValueAdapter((MinValueAttribute)attribute);
        }

        if (type == typeof(FileSizeAttribute))
        {
            return new FileSizeAdapter((FileSizeAttribute)attribute);
        }

        if (type == typeof(EqualToAttribute))
        {
            return new EqualToAdapter((EqualToAttribute)attribute);
        }

        if (type == typeof(IntegerAttribute))
        {
            return new IntegerAdapter((IntegerAttribute)attribute);
        }

        if (type == typeof(DigitsAttribute))
        {
            return new DigitsAdapter((DigitsAttribute)attribute);
        }

        if (type == typeof(RangeAttribute))
        {
            return new RangeAdapter((RangeAttribute)attribute);
        }

        if (type == typeof(LettersNumbersAttribute))
        {
            return new LettersNumbersAdapter((LettersNumbersAttribute)attribute);
        }

        if (type == typeof(PhoneAttribute))
        {
            return new PhoneAdapter((PhoneAttribute)attribute);
        }

        return null;
    }
}
