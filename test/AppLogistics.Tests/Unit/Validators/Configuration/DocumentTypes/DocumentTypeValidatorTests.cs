using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AppLogistics.Validators.Tests;

public class DocumentTypeValidatorTests
{
    private DocumentTypeValidator validator;
    private DbContext context;
    private DocumentType documentType;

    public DocumentTypeValidatorTests()
    {
        context = TestFixture.Create().Drop();
        validator = new DocumentTypeValidator(new UnitOfWork(context, TestFixture.Mapper));

        context.Set<DocumentType>().Add(documentType = ObjectsFactory.CreateDocumentType());
        context.SaveChanges();
    }

    #region CanCreate(DocumentTypeView view)

    [Fact]
    public void CanCreate_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanCreate(ObjectsFactory.CreateDocumentTypeView(1)));
    }

    [Fact]
    public void CanCreate_ValidDocumentType()
    {
        Assert.True(validator.CanCreate(ObjectsFactory.CreateDocumentTypeView(1)));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    #endregion CanCreate(DocumentTypeView view)

    #region CanEdit(DocumentTypeView view)

    [Fact]
    public void CanEdit_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanEdit(ObjectsFactory.CreateDocumentTypeView(documentType.Id)));
    }

    [Fact]
    public void CanEdit_ValidDocumentType()
    {
        DocumentTypeView view = ObjectsFactory.CreateDocumentTypeView(documentType.Id);
        view.Name = documentType.Name;

        Assert.True(validator.CanEdit(view));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    [Fact]
    public void CanEdit_DuplicateName_ReturnsFalse()
    {
        DocumentType otherDocumentType = ObjectsFactory.CreateDocumentType();
        otherDocumentType.Name = "OtherName";
        context.Set<DocumentType>().Add(otherDocumentType);
        context.SaveChanges();

        DocumentTypeView view = ObjectsFactory.CreateDocumentTypeView(documentType.Id);
        view.Name = otherDocumentType.Name.ToLowerInvariant();

        Assert.False(validator.CanEdit(view));
        Assert.NotEmpty(validator.Alerts);
    }

    #endregion CanEdit(DocumentTypeView view)
}
