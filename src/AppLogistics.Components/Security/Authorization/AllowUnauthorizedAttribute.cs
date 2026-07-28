using System;

namespace AppLogistics.Components.Security;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AllowUnauthorizedAttribute : Attribute
{
}
