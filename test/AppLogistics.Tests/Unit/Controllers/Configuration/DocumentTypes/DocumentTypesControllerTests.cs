using AppLogistics.Controllers.Tests;
using AppLogistics.Objects;
using AppLogistics.Services;
using AppLogistics.Tests;
using AppLogistics.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using System.Linq;
using Xunit;

namespace AppLogistics.Controllers.Configuration.Tests;

public class DocumentTypesControllerTests : ControllerTests
{
    private DocumentTypesController controller;
    private IDocumentTypeValidator validator;
    private IDocumentTypeService service;
    private DocumentTypeView documentType;

    public DocumentTypesControllerTests()
    {
        validator = Substitute.For<IDocumentTypeValidator>();
        validator.CanDelete(Arg.Any<int>()).Returns(true);
        service = Substitute.For<IDocumentTypeService>();

        documentType = ObjectsFactory.CreateDocumentTypeView();

        controller = Substitute.ForPartsOf<DocumentTypesController>(validator, service);
        controller.ControllerContext.RouteData = new RouteData();
    }

    #region Index()

    [Fact]
    public void Index_ReturnsDocumentTypeViews()
    {
        service.GetViews().Returns(new DocumentTypeView[0].AsQueryable());

        object actual = controller.Index().ViewData.Model;
        object expected = service.GetViews();

        Assert.Same(expected, actual);
    }

    #endregion Index()

    #region Create()

    [Fact]
    public void Create_ReturnsEmptyView()
    {
        ViewDataDictionary actual = controller.Create().ViewData;

        Assert.Null(actual.Model);
    }

    #endregion Create()

    #region Create(DocumentTypeView type)

    [Fact]
    public void Create_ProtectsFromOverpostingId()
    {
        ProtectsFromOverpostingId(controller, "Create");
    }

    [Fact]
    public void Create_CanNotCreate_ReturnsSameView()
    {
        validator.CanCreate(documentType).Returns(false);

        object actual = (controller.Create(documentType) as ViewResult).ViewData.Model;
        object expected = documentType;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Create_DocumentType()
    {
        validator.CanCreate(documentType).Returns(true);

        controller.Create(documentType);

        service.Received().Create(documentType);
    }

    [Fact]
    public void Create_RedirectsToIndex()
    {
        validator.CanCreate(documentType).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Create(documentType);

        Assert.Same(expected, actual);
    }

    #endregion Create(DocumentTypeView type)

    #region Details(String id)

    [Fact]
    public void Details_ReturnsNotEmptyView()
    {
        service.Get<DocumentTypeView>(documentType.Id).Returns(documentType);

        object expected = NotEmptyView(controller, documentType);
        object actual = controller.Details(documentType.Id);

        Assert.Same(expected, actual);
    }

    #endregion Details(String id)

    #region Edit(String id)

    [Fact]
    public void Edit_ReturnsNotEmptyView()
    {
        service.Get<DocumentTypeView>(documentType.Id).Returns(documentType);

        object expected = NotEmptyView(controller, documentType);
        object actual = controller.Edit(documentType.Id);

        Assert.Same(expected, actual);
    }

    #endregion Edit(String id)

    #region Edit(DocumentTypeView type)

    [Fact]
    public void Edit_CanNotEdit_ReturnsSameView()
    {
        validator.CanEdit(documentType).Returns(false);

        object actual = (controller.Edit(documentType) as ViewResult).ViewData.Model;
        object expected = documentType;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Edit_DocumentType()
    {
        validator.CanEdit(documentType).Returns(true);

        controller.Edit(documentType);

        service.Received().Edit(documentType);
    }

    [Fact]
    public void Edit_RedirectsToIndex()
    {
        validator.CanEdit(documentType).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Edit(documentType);

        Assert.Same(expected, actual);
    }

    #endregion Edit(DocumentTypeView type)

    #region Delete(String id)

    [Fact]
    public void Delete_ReturnsNotEmptyView()
    {
        service.Get<DocumentTypeView>(documentType.Id).Returns(documentType);

        object expected = NotEmptyView(controller, documentType);
        object actual = controller.Delete(documentType.Id);

        Assert.Same(expected, actual);
    }

    #endregion Delete(String id)

    #region DeleteConfirmed(String id)

    [Fact]
    public void DeleteConfirmed_DeletesType()
    {
        controller.DeleteConfirmed(documentType.Id);

        service.Received().Delete(documentType.Id);
    }

    [Fact]
    public void Delete_RedirectsToIndex()
    {
        object expected = RedirectToAction(controller, "Index");
        object actual = controller.DeleteConfirmed(documentType.Id);

        Assert.Same(expected, actual);
    }

    #endregion DeleteConfirmed(String id)
}
