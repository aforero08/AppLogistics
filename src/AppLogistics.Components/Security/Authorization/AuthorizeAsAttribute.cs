using System;

namespace AppLogistics.Components.Security;

[AttributeUsage(AttributeTargets.Method)]
public class AuthorizeAsAttribute : Attribute
{
    public string Action { get; }
    public string Area { get; set; }
    public string Controller { get; set; }

    public AuthorizeAsAttribute(string action)
    {
        Action = action ?? throw new ArgumentNullException(nameof(action));
    }
}
