using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace AppLogistics.Components.Mvc;

public static class ModelStateDictionaryExtensions
{
    public static Dictionary<string, string> Errors(this ModelStateDictionary modelState)
    {
        return modelState
            .Where(state => state.Value.Errors.Count > 0)
            .ToDictionary(
                pair => pair.Key,
                pair => pair.Value.Errors
                    .Select(model => model.ErrorMessage)
                    .FirstOrDefault(error => !string.IsNullOrEmpty(error))
        );
    }
}
