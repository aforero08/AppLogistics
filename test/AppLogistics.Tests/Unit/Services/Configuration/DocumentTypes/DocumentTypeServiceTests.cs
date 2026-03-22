using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace AppLogistics.Services.Tests;

public class DocumentTypeServiceTests
{
    private DocumentTypeService service;
    private DbContext context;
    private DocumentType documentType;

    public DocumentTypeServiceTests()
    {
        context = TestingContext.Create();
        service = new DocumentTypeService(new UnitOfWork(TestingContext.Create(), TestingContext.Mapper));

        context.Set<DocumentType>().Add(documentType = ObjectsFactory.CreateDocumentType());
        context.SaveChanges();
    }

    #region Get<TView>(String id)

    [Fact]
    public void Get_ReturnsViewById()
    {
        DocumentTypeView actual = service.Get<DocumentTypeView>(documentType.Id);
        DocumentTypeView expected = TestingContext.Mapper.Map<DocumentTypeView>(documentType);

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.ShortName, actual.ShortName);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Id, actual.Id);
    }

    #endregion Get<TView>(String id)

    #region GetViews()

    [Fact]
    public void GetViews_ReturnsDocumentTypeViews()
    {
        DocumentTypeView[] actual = service.GetViews().ToArray();
        DocumentTypeView[] expected = context
            .Set<DocumentType>()
            .ProjectTo<DocumentTypeView>(TestingContext.Mapper.ConfigurationProvider)
            .OrderByDescending(view => view.CreationDate)
            .ToArray();

        for (int i = 0; i < expected.Length || i < actual.Length; i++)
        {
            Assert.Equal(expected[i].CreationDate, actual[i].CreationDate);
            Assert.Equal(expected[i].ShortName, actual[i].ShortName);
            Assert.Equal(expected[i].Name, actual[i].Name);
            Assert.Equal(expected[i].Id, actual[i].Id);
        }
    }

    #endregion GetViews()

    #region Create(DocumentTypeView view)

    [Fact]
    public void Create_DocumentType()
    {
        DocumentTypeView view = ObjectsFactory.CreateDocumentTypeView(1);
        view.Id = 0;

        service.Create(view);

        DocumentType actual = context.Set<DocumentType>().AsNoTracking().Single(model => model.Id != documentType.Id);
        DocumentTypeView expected = view;

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.ShortName, actual.ShortName);
        Assert.Equal(expected.Name, actual.Name);
    }

    #endregion Create(DocumentTypeView view)

    #region Edit(DocumentTypeView view)

    [Fact]
    public void Edit_DocumentType()
    {
        DocumentTypeView view = ObjectsFactory.CreateDocumentTypeView(documentType.Id);
        view.Name = "Name0";
        view.ShortName = "ShortName0";

        service.Edit(view);

        DocumentType actual = context.Set<DocumentType>().AsNoTracking().Single();
        DocumentType expected = documentType;

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.ShortName, actual.ShortName);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Id, actual.Id);
    }

    #endregion Edit(DocumentTypeView view)

    #region Delete(String id)

    [Fact]
    public void Delete_DocumentType()
    {
        service.Delete(documentType.Id);

        Assert.Empty(context.Set<DocumentType>());
    }

    #endregion Delete(String id)
}
