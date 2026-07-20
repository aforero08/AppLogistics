using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using System;
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
        Assert.True(validator.CanEdit(ObjectsFactory.CreateDocumentTypeView(documentType.Id)));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    #endregion CanEdit(DocumentTypeView view)
}
