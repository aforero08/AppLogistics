using AppLogistics.Components.Extensions;
using AppLogistics.Components.Security;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AppLogistics.Components.Mvc;

public class SiteMap : ISiteMap
{
    private readonly IAuthorization _authorization;
    private IEnumerable<SiteMapNode> Tree { get; }
    private IEnumerable<SiteMapNode> Nodes { get; }

    public SiteMap(string map, IAuthorization authorization)
    {
        _authorization = authorization;

        XElement siteMap = XElement.Parse(map);
        Tree = Parse(siteMap);
        Nodes = Flatten(Tree);
    }

    public IEnumerable<SiteMapNode> For(ViewContext context)
    {
        int? account = context.HttpContext.User.Id();
        string area = context.RouteData.Values["area"] as string;
        string action = context.RouteData.Values["action"] as string;
        string controller = context.RouteData.Values["controller"] as string;
        IEnumerable<SiteMapNode> nodes = SetState(Tree, area, controller, action);

        return Authorize(account, nodes);
    }

    public IEnumerable<SiteMapNode> BreadcrumbFor(ViewContext context)
    {
        string area = context.RouteData.Values["area"] as string;
        string action = context.RouteData.Values["action"] as string;
        string controller = context.RouteData.Values["controller"] as string;

        SiteMapNode current = Nodes.SingleOrDefault(node =>
            string.Equals(node.Area, area, StringComparison.OrdinalIgnoreCase)
            && string.Equals(node.Action, action, StringComparison.OrdinalIgnoreCase)
            && string.Equals(node.Controller, controller, StringComparison.OrdinalIgnoreCase));

        List<SiteMapNode> breadcrumb = new List<SiteMapNode>();
        while (current != null)
        {
            breadcrumb.Insert(0, new SiteMapNode
            {
                IconClass = current.IconClass,

                Controller = current.Controller,
                Action = current.Action,
                Area = current.Area
            });

            current = current.Parent;
        }

        return breadcrumb;
    }

    private IEnumerable<SiteMapNode> SetState(IEnumerable<SiteMapNode> nodes, string area, string controller, string action)
    {
        List<SiteMapNode> copies = new List<SiteMapNode>();
        foreach (SiteMapNode node in nodes)
        {
            SiteMapNode copy = new SiteMapNode
            {
                IconClass = node.IconClass,
                IsMenu = node.IsMenu,

                Controller = node.Controller,
                Action = node.Action,
                Area = node.Area,

                Children = SetState(node.Children, area, controller, action)
            };
            copy.HasActiveChildren = copy.Children.Any(child => child.IsActive || child.HasActiveChildren);
            copy.IsActive =
                copy.Children.Any(child => child.IsActive && !child.IsMenu)
                || string.Equals(node.Area, area, StringComparison.OrdinalIgnoreCase)
                && string.Equals(node.Action, action, StringComparison.OrdinalIgnoreCase)
                && string.Equals(node.Controller, controller, StringComparison.OrdinalIgnoreCase);

            copies.Add(copy);
        }

        return copies;
    }

    private IEnumerable<SiteMapNode> Authorize(int? accountId, IEnumerable<SiteMapNode> nodes)
    {
        List<SiteMapNode> authorized = new List<SiteMapNode>();
        foreach (SiteMapNode node in nodes)
        {
            node.Children = Authorize(accountId, node.Children);

            if (node.IsMenu && IsAuthorizedFor(accountId, node.Area, node.Controller, node.Action) && !IsEmpty(node))
            {
                authorized.Add(node);
            }
            else
            {
                authorized.AddRange(node.Children);
            }
        }

        return authorized;
    }

    private bool IsAuthorizedFor(int? accountId, string area, string controller, string action)
    {
        return action == null || _authorization?.IsGrantedFor(accountId, area, controller, action) != false;
    }

    private IEnumerable<SiteMapNode> Parse(XElement root, SiteMapNode parent = null)
    {
        List<SiteMapNode> nodes = new List<SiteMapNode>();
        foreach (XElement element in root.Elements("siteMapNode"))
        {
            SiteMapNode node = new SiteMapNode
            {
                IsMenu = (bool?)element.Attribute("menu") == true,
                Controller = (string)element.Attribute("controller"),
                IconClass = (string)element.Attribute("icon"),
                Action = (string)element.Attribute("action"),
                Area = (string)element.Attribute("area")
            };
            node.Children = Parse(element, node);
            node.Parent = parent;

            nodes.Add(node);
        }

        return nodes;
    }

    private IEnumerable<SiteMapNode> Flatten(IEnumerable<SiteMapNode> branches)
    {
        List<SiteMapNode> list = new List<SiteMapNode>();
        foreach (SiteMapNode branch in branches)
        {
            list.Add(branch);
            list.AddRange(Flatten(branch.Children));
        }

        return list;
    }

    private bool IsEmpty(SiteMapNode node)
    {
        return node.Action == null && !node.Children.Any();
    }
}
