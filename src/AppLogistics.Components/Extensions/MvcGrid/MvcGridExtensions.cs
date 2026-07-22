using AppLogistics.Components.Security;
using AppLogistics.Resources;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using NonFactors.Mvc.Grid;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace AppLogistics.Components.Extensions;

public static class MvcGridExtensions
{
    public static IGridColumn<T, IHtmlContent> AddAction<T>(this IGridColumnsOf<T> columns,
                                                            string action,
                                                            string iconClass,
                                                            Expression<Func<T, bool>> condition = null) where T : class
    {
        ViewContext context = columns.Grid.ViewContext!;

        if (!IsAuthorizedFor(context, action))
            return new GridColumn<T, IHtmlContent>(columns.Grid, _ => HtmlString.Empty);

        IUrlHelperFactory factory = context.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
        IUrlHelper url = factory.GetUrlHelper(context);

        if (condition != null)
        {
            Func<T, bool> predicate = condition.Compile();
            return columns
                .Add(model => predicate(model) ? GenerateLink(model, url, action, iconClass) : HtmlString.Empty)
                .Css($"action-cell {action.ToLower()}");
        }

        return columns
            .Add(model => GenerateLink(model, url, action, iconClass))
            .Css($"action-cell {action.ToLower()}");
    }

    public static IGridColumn<T, DateTime> AddDate<T>(this IGridColumnsOf<T> columns, Expression<Func<T, DateTime>> expression)
    {
        return columns.AddProperty(expression).Formatted("{0:d}");
    }

    public static IGridColumn<T, DateTime?> AddDate<T>(this IGridColumnsOf<T> columns, Expression<Func<T, DateTime?>> expression)
    {
        return columns.AddProperty(expression).Formatted("{0:d}");
    }

    public static IGridColumn<T, bool> AddBoolean<T>(this IGridColumnsOf<T> columns, Expression<Func<T, bool>> expression)
    {
        Func<T, bool> valueFor = expression.Compile();

        return columns
            .AddProperty(expression)
            .RenderedAs(model => valueFor(model) ? Resource.ForString("Yes") : Resource.ForString("No"));
    }

    public static IGridColumn<T, bool?> AddBoolean<T>(this IGridColumnsOf<T> columns, Expression<Func<T, bool?>> expression)
    {
        Func<T, bool?> valueFor = expression.Compile();

        return columns
            .AddProperty(expression)
            .RenderedAs(model =>
                valueFor(model) != null
                    ? valueFor(model) == true
                        ? Resource.ForString("Yes")
                        : Resource.ForString("No")
                    : "");
    }

    public static IGridColumn<T, DateTime> AddDateTime<T>(this IGridColumnsOf<T> columns, Expression<Func<T, DateTime>> expression)
    {
        return columns.AddProperty(expression).Formatted("{0:g}");
    }

    public static IGridColumn<T, DateTime?> AddDateTime<T>(this IGridColumnsOf<T> columns, Expression<Func<T, DateTime?>> expression)
    {
        return columns.AddProperty(expression).Formatted("{0:g}");
    }

    public static IGridColumn<T, TProperty> AddProperty<T, TProperty>(this IGridColumnsOf<T> columns, Expression<Func<T, TProperty>> expression)
    {
        return columns
            .Add(expression)
            .Css(CssClassFor<TProperty>())
            .Titled(Resource.ForProperty(expression));
    }

    public static IHtmlGrid<T> ApplyDefaults<T>(this IHtmlGrid<T> grid)
    {
        return grid
            .Pageable(pager => { pager.RowsPerPage = 16; })
            .Empty(Resource.ForString("NoDataFound"))
            .AppendCss("table-hover")
            .Filterable()
            .Sortable();
    }

    private static IHtmlContent GenerateLink<T>(T model, IUrlHelper url, string action, string iconClass)
    {
        TagBuilder link = new("a");
        link.Attributes["href"] = url.Action(action, RouteFor(model));
        link.Attributes["title"] = Resource.ForAction(action);
        link.AddCssClass(iconClass);

        return link;
    }

    private static bool IsAuthorizedFor(ViewContext context, string action)
    {
        IAuthorization authorization = context.HttpContext.RequestServices.GetService<IAuthorization>();
        if (authorization == null)
        {
            return true;
        }

        int? account = context.HttpContext.User.Id();
        string area = context.RouteData.Values["area"] as string;
        string controller = context.RouteData.Values["controller"] as string;

        return authorization.IsGrantedFor(account, area, controller, action);
    }

    private static IDictionary<string, object> RouteFor<T>(T model)
    {
        PropertyInfo id = typeof(T).GetProperty("Id") ?? throw new Exception(typeof(T).Name + " type does not have an id.");

        return new Dictionary<string, object> { ["id"] = id.GetValue(model) };
    }

    private static string CssClassFor<TProperty>()
    {
        Type type = Nullable.GetUnderlyingType(typeof(TProperty)) ?? typeof(TProperty);
        if (type.IsEnum)
        {
            return "text-left";
        }

        switch (Type.GetTypeCode(type))
        {
            case TypeCode.SByte:
            case TypeCode.Byte:
            case TypeCode.Int16:
            case TypeCode.UInt16:
            case TypeCode.Int32:
            case TypeCode.UInt32:
            case TypeCode.Int64:
            case TypeCode.UInt64:
            case TypeCode.Single:
            case TypeCode.Double:
            case TypeCode.Decimal:
                return "text-right";

            case TypeCode.Boolean:
            case TypeCode.DateTime:
                return "text-center";

            default:
                return "text-left";
        }
    }
}
