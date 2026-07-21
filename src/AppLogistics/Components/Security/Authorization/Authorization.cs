using AppLogistics.Data.Core;
using AppLogistics.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AppLogistics.Components.Security;

public class Authorization : IAuthorization
{
    private IServiceScopeFactory ScopeFactory { get; }
    private Dictionary<string, string> Required { get; }
    private Dictionary<string, MethodInfo> Actions { get; }
    private Dictionary<int, HashSet<string>> Permissions { get; set; }

    public Authorization(Assembly controllers, IServiceScopeFactory scopeFactory)
    {
        BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public;
        Actions = new Dictionary<string, MethodInfo>(StringComparer.OrdinalIgnoreCase);
        Required = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        Permissions = new Dictionary<int, HashSet<string>>();
        ScopeFactory = scopeFactory;

        foreach (Type controller in controllers.GetTypes().Where(IsController))
        {
            foreach (MethodInfo method in controller.GetMethods(flags).Where(IsAction))
            {
                Actions[ActionFor(method)] = method;
            }
        }

        foreach (string action in Actions.Keys)
        {
            if (RequiredPermissionFor(action) is string permission)
            {
                Required[action] = permission;
            }
        }

        Refresh();
    }

    public bool IsGrantedFor(int? accountId, string area, string controller, string action)
    {
        string permission = (area + "/" + controller + "/" + action).ToLower();
        if (!Required.ContainsKey(permission))
        {
            return true;
        }

        return Permissions.ContainsKey(accountId ?? 0) && Permissions[accountId.Value].Contains(Required[permission]);
    }

    public void Refresh()
    {
        using IServiceScope scope = ScopeFactory.CreateScope();
        IUnitOfWork unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        Permissions = unitOfWork
            .Select<Account>()
            .Where(account =>
                !account.IsLocked
                && account.RoleId != null)
            .Select(account => new
            {
                Id = account.Id,
                Permissions = account
                    .Role
                    .Permissions
                    .Select(role => role.Permission)
                    .Select(permission => (permission.Area ?? "") + "/" + permission.Controller + "/" + permission.Action)
            })
            .ToDictionary(
                account => account.Id,
                account => new HashSet<string>(account.Permissions, StringComparer.OrdinalIgnoreCase));
    }

    private bool RequiresAuthorization(string action)
    {
        bool? isRequired = null;
        MethodInfo method = Actions[action];
        Type controller = method.DeclaringType;

        if (method.IsDefined(typeof(AuthorizeAttribute), false))
        {
            isRequired = true;
        }

        if (method.IsDefined(typeof(AllowAnonymousAttribute), false))
        {
            return false;
        }

        if (method.IsDefined(typeof(AllowUnauthorizedAttribute), false))
        {
            isRequired = isRequired ?? false;
        }

        while (controller != typeof(Controller))
        {
            if (controller.IsDefined(typeof(AuthorizeAttribute), false))
            {
                isRequired = isRequired ?? true;
            }

            if (controller.IsDefined(typeof(AllowAnonymousAttribute), false))
            {
                return false;
            }

            if (controller.IsDefined(typeof(AllowUnauthorizedAttribute), false))
            {
                isRequired = isRequired ?? false;
            }

            controller = controller.BaseType;
        }

        return isRequired == true;
    }

    private string RequiredPermissionFor(string action)
    {
        string[] path = action.Split('/');
        AuthorizeAsAttribute auth = Actions[action].GetCustomAttribute<AuthorizeAsAttribute>(false);
        string asAction = $"{auth?.Area ?? path[0]}/{auth?.Controller ?? path[1]}/{auth?.Action ?? path[2]}";

        if (action != asAction)
        {
            return RequiredPermissionFor(asAction);
        }

        return RequiresAuthorization(action) ? action : null;
    }

    private string ActionFor(MethodInfo method)
    {
        string controller = method.DeclaringType.Name.Substring(0, method.DeclaringType.Name.Length - 10);
        string action = method.GetCustomAttribute<ActionNameAttribute>(false)?.Name ?? method.Name;
        string area = method.DeclaringType.GetCustomAttribute<AreaAttribute>(false)?.RouteValue;

        return $"{area}/{controller}/{action}";
    }

    private bool IsAction(MethodInfo method)
    {
        return !method.IsSpecialName && !method.IsDefined(typeof(NonActionAttribute));
    }

    private bool IsController(Type type)
    {
        return type.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
            && !type.IsDefined(typeof(NonControllerAttribute))
            && typeof(Controller).IsAssignableFrom(type)
            && !type.ContainsGenericParameters
            && !type.IsAbstract
            && type.IsPublic;
    }
}
