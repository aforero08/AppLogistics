using AppLogistics.Components.Security;
using AppLogistics.Resources;
using AppLogistics.Tests;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using NonFactors.Mvc.Grid;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.IO;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using Xunit;

namespace AppLogistics.Components.Extensions.Tests
{
    public class MvcGridExtensionsTests
    {
        private IGridColumnsOf<AllTypesView> columns;
        private IHtmlGrid<AllTypesView> html;

        public MvcGridExtensionsTests()
        {
            Grid<AllTypesView> grid = new Grid<AllTypesView>(new AllTypesView[0]);
            IHtmlHelper htmlHelper = HtmlHelperFactory.CreateHtmlHelper();
            html = new HtmlGrid<AllTypesView>(htmlHelper, grid);
            columns = new GridColumns<AllTypesView>(grid);
        }

        #region AddAction<T>(this IGridColumnsOf<T> columns, String action, String iconClass)

        [Fact]
        public void AddAction_Unauthorized_Empty()
        {
            IGridColumn<AllTypesView, IHtmlContent> actual = columns.AddAction("Edit", "fa fa-pencil-alt");

            Assert.Empty(actual.ValueFor(null).ToString());
            Assert.Empty(columns);
        }

        [Fact]
        public void AddAction_Authorized_Renders()
        {
            AllTypesView view = new AllTypesView();
            StringWriter writer = new StringWriter();
            IRouter router = Substitute.For<IRouter>();
            IAuthorization authorization = columns.Grid.ViewContext.HttpContext.RequestServices.GetService<IAuthorization>();

            authorization.IsGrantedFor(Arg.Any<int?>(), Arg.Any<string>(), Arg.Any<string>(), "Details").Returns(true);
            router.GetVirtualPath(Arg.Any<VirtualPathContext>()).Returns(new VirtualPathData(router, "/test"));
            columns.Grid.ViewContext.RouteData.Routers.Add(router);

            IGridColumn<AllTypesView, IHtmlContent> column = columns.AddAction("Details", "fa fa-info");
            column.ValueFor(new GridRow<AllTypesView>(new AllTypesView(), 0)).WriteTo(writer, HtmlEncoder.Default);

            string expected = "<a class=\"fa fa-info details-action\" href=\"/test\"></a>";
            string actual = writer.ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddAction_NoAuthorization_Renders()
        {
            AllTypesView view = new AllTypesView();
            StringWriter writer = new StringWriter();
            IRouter router = Substitute.For<IRouter>();

            columns.Grid.ViewContext.HttpContext.RequestServices.GetService(typeof(IAuthorization)).ReturnsNull();
            router.GetVirtualPath(Arg.Any<VirtualPathContext>()).Returns(new VirtualPathData(router, "/test"));
            columns.Grid.ViewContext.RouteData.Routers.Add(router);

            IGridColumn<AllTypesView, IHtmlContent> column = columns.AddAction("Details", "fa fa-info");
            column.ValueFor(new GridRow<AllTypesView>(new AllTypesView(), 0)).WriteTo(writer, HtmlEncoder.Default);

            string expected = "<a class=\"fa fa-info details-action\" href=\"/test\"></a>";
            string actual = writer.ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddAction_NoId_Throws()
        {
            html.Grid.ViewContext.HttpContext.RequestServices.GetService(typeof(IAuthorization)).ReturnsNull();
            IGridColumnsOf<object> gridColumns = new GridColumns<object>(new Grid<object>(new object[0]));
            gridColumns.Grid.ViewContext = html.Grid.ViewContext;

            IGridColumn<object, IHtmlContent> column = gridColumns.AddAction("Delete", "fa fa-times");

            String actual = Assert.Throws<Exception>(() => column.ValueFor(new GridRow<Object>(new Object(), 0))).Message;
            string expected = "Object type does not have an id.";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddAction_Column()
        {
            columns.Grid.ViewContext.HttpContext.RequestServices.GetService(typeof(IAuthorization)).ReturnsNull();

            IGridColumn<AllTypesView, IHtmlContent> actual = columns.AddAction("Edit", "fa fa-pencil-alt");

            Assert.Equal("action-cell edit", actual.CssClasses);
            Assert.Single(columns);
        }

        #endregion AddAction<T>(this IGridColumnsOf<T> columns, String action, String iconClass)

        #region AddDate<T>(this IGridColumnsOf<T> columns, Expression<Func<T, DateTime>> expression)

        [Fact]
        public void AddDate_Column()
        {
            Expression<Func<AllTypesView, DateTime>> expression = (model) => model.DateTimeField;

            IGridColumn<AllTypesView, DateTime> actual = columns.AddDate(expression);

            Assert.Equal("text-center", actual.CssClasses);
            Assert.Equal(expression, actual.Expression);
            Assert.Equal("{0:d}", actual.Format);
            Assert.Empty(actual.Title.ToString());
            Assert.Single(columns);
        }

        #endregion AddDate<T>(this IGridColumnsOf<T> columns, Expression<Func<T, DateTime>> expression)

        #region AddDate<T>(this IGridColumnsOf<T> columns, Expression<Func<T, DateTime?>> expression)

        [Fact]
        public void AddDate_Nullable_Column()
        {
            Expression<Func<AllTypesView, DateTime?>> expression = (model) => model.NullableDateTimeField;

            IGridColumn<AllTypesView, DateTime?> actual = columns.AddDate(expression);

            Assert.Equal("text-center", actual.CssClasses);
            Assert.Equal(expression, actual.Expression);
            Assert.Empty(actual.Title.ToString());
            Assert.Equal("{0:d}", actual.Format);
            Assert.Single(columns);
        }

        #endregion AddDate<T>(this IGridColumnsOf<T> columns, Expression<Func<T, DateTime?>> expression)

        #region AddBoolean<T>(this IGridColumnsOf<T> columns, Expression<Func<T, Boolean>> expression)

        [Fact]
        public void AddBoolean_Column()
        {
            Expression<Func<AllTypesView, bool>> expression = (model) => model.BooleanField;

            IGridColumn<AllTypesView, bool> actual = columns.AddBoolean(expression);

            Assert.Equal("text-center", actual.CssClasses);
            Assert.Equal(expression, actual.Expression);
            Assert.Empty(actual.Title.ToString());
            Assert.Single(columns);
        }

        [Fact]
        public void AddBoolean_True()
        {
            IGridColumn<AllTypesView, bool> column = columns.AddBoolean(model => model.BooleanField);
            GridRow<AllTypesView> row = new GridRow<AllTypesView>(new AllTypesView { BooleanField = true }, 0);

            string actual = column.ValueFor(row).ToString();
            string expected = Resource.ForString("Yes");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddBoolean_False()
        {
            GridRow<AllTypesView> row = new GridRow<AllTypesView>(new AllTypesView { BooleanField = false }, 0);
            IGridColumn<AllTypesView, bool> column = columns.AddBoolean(model => model.BooleanField);

            string actual = column.ValueFor(row).ToString();
            string expected = Resource.ForString("No");

            Assert.Equal(expected, actual);
        }

        #endregion AddBoolean<T>(this IGridColumnsOf<T> columns, Expression<Func<T, Boolean>> expression)

        #region AddBoolean<T>(this IGridColumnsOf<T> columns, Expression<Func<T, Boolean?>> expression)

        [Fact]
        public void AddBoolean_Nullable_Column()
        {
            Expression<Func<AllTypesView, bool?>> expression = (model) => model.NullableBooleanField;

            IGridColumn<AllTypesView, bool?> actual = columns.AddBoolean(expression);

            Assert.Equal("text-center", actual.CssClasses);
            Assert.Equal(expression, actual.Expression);
            Assert.Empty(actual.Title.ToString());
            Assert.Single(columns);
        }

        [Fact]
        public void AddBoolean_Nullable()
        {
            IGridColumn<AllTypesView, bool?> column = columns.AddBoolean(model => model.NullableBooleanField);
            GridRow<AllTypesView> row = new GridRow<AllTypesView>(new AllTypesView { NullableBooleanField = null }, 0);

            string actual = column.ValueFor(row).ToString();

            Assert.Empty(actual);
        }

        [Fact]
        public void AddBoolean_Nullable_True()
        {
            IGridColumn<AllTypesView, bool?> column = columns.AddBoolean(model => model.NullableBooleanField);
            GridRow<AllTypesView> row = new GridRow<AllTypesView>(new AllTypesView { NullableBooleanField = true }, 0);

            string actual = column.ValueFor(row).ToString();
            string expected = Resource.ForString("Yes");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddBoolean_Nullable_False()
        {
            IGridColumn<AllTypesView, bool?> column = columns.AddBoolean(model => model.NullableBooleanField);
            GridRow<AllTypesView> row = new GridRow<AllTypesView>(new AllTypesView { NullableBooleanField = false }, 0);

            string actual = column.ValueFor(row).ToString();
            string expected = Resource.ForString("No");

            Assert.Equal(expected, actual);
        }

        #endregion AddBoolean<T>(this IGridColumnsOf<T> columns, Expression<Func<T, Boolean?>> expression)

        #region AddDateTime<T>(this IGridColumnsOf<T> columns, Expression<Func<T, DateTime>> expression)

        [Fact]
        public void AddDateTime_Column()
        {
            Expression<Func<AllTypesView, DateTime>> expression = (model) => model.DateTimeField;

            IGridColumn<AllTypesView, DateTime> actual = columns.AddDateTime(expression);

            Assert.Equal("text-center", actual.CssClasses);
            Assert.Equal(expression, actual.Expression);
            Assert.Empty(actual.Title.ToString());
            Assert.Equal("{0:g}", actual.Format);
            Assert.Single(columns);
        }

        #endregion AddDateTime<T>(this IGridColumnsOf<T> columns, Expression<Func<T, DateTime>> expression)

        #region AddDateTime<T>(this IGridColumnsOf<T> columns, Expression<Func<T, DateTime?>> expression)

        [Fact]
        public void AddDateTime_Nullable_Column()
        {
            Expression<Func<AllTypesView, DateTime?>> expression = (model) => model.NullableDateTimeField;

            IGridColumn<AllTypesView, DateTime?> actual = columns.AddDateTime(expression);

            Assert.Equal("text-center", actual.CssClasses);
            Assert.Equal(expression, actual.Expression);
            Assert.Empty(actual.Title.ToString());
            Assert.Equal("{0:g}", actual.Format);
            Assert.Single(columns);
        }

        #endregion AddDateTime<T>(this IGridColumnsOf<T> columns, Expression<Func<T, DateTime?>> expression)

        #region AddProperty<T, TProperty>(this IGridColumnsOf<T> columns, Expression<Func<T, TProperty>> expression)

        [Fact]
        public void AddProperty_Column()
        {
            Expression<Func<AllTypesView, AllTypesView>> expression = (model) => model;

            IGridColumn<AllTypesView, AllTypesView> actual = columns.AddProperty(expression);

            Assert.Equal("text-left", actual.CssClasses);
            Assert.Equal(expression, actual.Expression);
            Assert.Empty(actual.Title.ToString());
            Assert.Single(columns);
        }

        [Fact]
        public void AddProperty_SetsCssClassForEnum()
        {
            AssertCssClassFor(model => model.EnumField, "text-left");
        }

        [Fact]
        public void AddProperty_SetsCssClassForSByte()
        {
            AssertCssClassFor(model => model.SByteField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForByte()
        {
            AssertCssClassFor(model => model.ByteField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForInt16()
        {
            AssertCssClassFor(model => model.Int16Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForUInt16()
        {
            AssertCssClassFor(model => model.UInt16Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForInt32()
        {
            AssertCssClassFor(model => model.Int32Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForUInt32()
        {
            AssertCssClassFor(model => model.UInt32Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForInt64()
        {
            AssertCssClassFor(model => model.Int64Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForUInt64()
        {
            AssertCssClassFor(model => model.UInt64Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForSingle()
        {
            AssertCssClassFor(model => model.SingleField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForDouble()
        {
            AssertCssClassFor(model => model.DoubleField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForDecimal()
        {
            AssertCssClassFor(model => model.DecimalField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForBoolean()
        {
            AssertCssClassFor(model => model.BooleanField, "text-center");
        }

        [Fact]
        public void AddProperty_SetsCssClassForDateTime()
        {
            AssertCssClassFor(model => model.DateTimeField, "text-center");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableEnum()
        {
            AssertCssClassFor(model => model.NullableEnumField, "text-left");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableSByte()
        {
            AssertCssClassFor(model => model.NullableSByteField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableByte()
        {
            AssertCssClassFor(model => model.NullableByteField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableInt16()
        {
            AssertCssClassFor(model => model.NullableInt16Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableUInt16()
        {
            AssertCssClassFor(model => model.NullableUInt16Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableInt32()
        {
            AssertCssClassFor(model => model.NullableInt32Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableUInt32()
        {
            AssertCssClassFor(model => model.NullableUInt32Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableInt64()
        {
            AssertCssClassFor(model => model.NullableInt64Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableUInt64()
        {
            AssertCssClassFor(model => model.NullableUInt64Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableSingle()
        {
            AssertCssClassFor(model => model.NullableSingleField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableDouble()
        {
            AssertCssClassFor(model => model.NullableDoubleField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableDecimal()
        {
            AssertCssClassFor(model => model.NullableDecimalField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableBoolean()
        {
            AssertCssClassFor(model => model.NullableBooleanField, "text-center");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableDateTime()
        {
            AssertCssClassFor(model => model.NullableDateTimeField, "text-center");
        }

        [Fact]
        public void AddProperty_SetsCssClassForOtherTypes()
        {
            AssertCssClassFor(model => model.StringField, "text-left");
        }

        #endregion AddProperty<T, TProperty>(this IGridColumnsOf<T> columns, Expression<Func<T, TProperty>> expression)

        #region ApplyDefaults<T>(this IHtmlGrid<T> grid)

        [Theory]
        [InlineData("", "table-hover")]
        [InlineData(" ", "table-hover")]
        [InlineData(null, "table-hover")]
        [InlineData("test", "test table-hover")]
        [InlineData(" test", "test table-hover")]
        [InlineData("test ", "test  table-hover")]
        [InlineData(" test ", "test  table-hover")]
        public void ApplyDefaults_Values(string cssClasses, string expectedClasses)
        {
            IGridColumn column = html.Grid.Columns.Add(model => model.ByteField);
            html.Grid.Attributes["class"] = cssClasses;
            column.Filter.IsEnabled = null;
            column.Sort.IsEnabled = null;

            IGrid actual = html.ApplyDefaults().Grid;

            Assert.Equal(Resource.ForString("NoDataFound"), actual.EmptyText);
            Assert.Equal(expectedClasses, html.Grid.Attributes["class"]);
            Assert.True(column.Filter.IsEnabled);
            Assert.True(column.Sort.IsEnabled);
            Assert.NotEmpty(actual.Columns);
        }

        #endregion ApplyDefaults<T>(this IHtmlGrid<T> grid)

        #region Test helpers

        private void AssertCssClassFor<TProperty>(Expression<Func<AllTypesView, TProperty>> property, string cssClasses)
        {
            IGridColumn<AllTypesView, TProperty> column = columns.AddProperty(property);

            string actual = column.CssClasses;
            string expected = cssClasses;

            Assert.Equal(expected, actual);
        }

        #endregion Test helpers
    }
}
