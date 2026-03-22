using AppLogistics.Components.Notifications;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace AppLogistics.Validators;

public interface IValidator : IDisposable
{
    ModelStateDictionary ModelState { get; set; }
    int CurrentAccountId { get; set; }
    Alerts Alerts { get; set; }
}
