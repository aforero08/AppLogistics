using AppLogistics.Components.Security;
using AppLogistics.Data.Migrations;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace AppLogistics.Resources.Tests
{
    public class ResourcesTests
    {
        [Fact]
        public void Resources_HasAllPageTitles()
        {
            IDictionary<string, object> values = new Dictionary<string, object>();
            IEnumerable<XElement> sitemap = XDocument
                .Load("../../../../../src/AppLogistics.Web/mvc.sitemap")
                .Descendants("siteMapNode")
                .Where(node => node.Attribute("action") != null);

            foreach (XElement node in sitemap)
            {
                values["area"] = node.Attribute("area")?.Value;
                values["action"] = node.Attribute("action").Value;
                values["controller"] = node.Attribute("controller").Value;

                string page = $"{values["area"]}{values["controller"]}{values["action"]}";

                Assert.True(!string.IsNullOrEmpty(Resource.ForPage(values)),
                    $"'{page}' page, does not have a title.");
            }
        }

        [Fact]
        public void Resources_HasAllSiteMapTitles()
        {
            IEnumerable<XElement> sitemap = XDocument
                .Load("../../../../../src/AppLogistics.Web/mvc.sitemap")
                .Descendants("siteMapNode");

            foreach (XElement node in sitemap)
            {
                Assert.True(!string.IsNullOrEmpty(Resource.ForSiteMap(node.Attribute("area")?.Value, node.Attribute("controller")?.Value, node.Attribute("action")?.Value)),
                    $"Sitemap node '{node}' page, does not have a title.");
            }
        }

        [Fact]
        public void Resources_HasAllPermissionAreaTitles()
        {
            using DbContext context = TestingContext.Create();
            using (DatabaseConfiguration configuration = new DatabaseConfiguration(context, Substitute.For<IConfiguration>(), Substitute.For<IHasher>(), TestingContext.Mapper))
            {
                configuration.SeedData();

                foreach (Permission permission in context.Set<Permission>().Where(permission => permission.Area != null))
                {
                    Assert.True(!string.IsNullOrEmpty(Resource.ForPermission(permission.Area)),
                        $"'{permission.Area}' permission, does not have a title.");
                }
            }
        }

        [Fact]
        public void Resources_HasAllPermissionControllerTitles()
        {
            using DbContext context = TestingContext.Create();
            using (DatabaseConfiguration configuration = new DatabaseConfiguration(context, Substitute.For<IConfiguration>(), Substitute.For<IHasher>(), TestingContext.Mapper))
            {
                configuration.SeedData();

                foreach (Permission permission in context.Set<Permission>())
                {
                    Assert.True(!string.IsNullOrEmpty(Resource.ForPermission(permission.Area, permission.Controller)),
                        $"'{permission.Area}{permission.Controller}' permission, does not have a title.");
                }
            }
        }

        [Fact]
        public void Resources_HasAllPermissionActionTitles()
        {
            using DbContext context = TestingContext.Create();
            using (DatabaseConfiguration configuration = new DatabaseConfiguration(context, Substitute.For<IConfiguration>(), Substitute.For<IHasher>(), TestingContext.Mapper))
            {
                configuration.SeedData();

                foreach (Permission permission in context.Set<Permission>())
                {
                    Assert.True(!string.IsNullOrEmpty(Resource.ForPermission(permission.Area, permission.Controller, permission.Action)),
                        $"'{permission.Area}{permission.Controller}{permission.Action} permission', does not have a title.");
                }
            }
        }
    }
}
