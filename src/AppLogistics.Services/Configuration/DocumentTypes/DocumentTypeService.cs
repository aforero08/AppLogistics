using AppLogistics.Data.Core;
using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public class DocumentTypeService : BaseService, IDocumentTypeService
{
    public DocumentTypeService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public TView Get<TView>(int id) where TView : BaseView
    {
        return UnitOfWork.GetAs<DocumentType, TView>(id);
    }

    public IQueryable<DocumentTypeView> GetViews()
    {
        return UnitOfWork
            .Select<DocumentType>()
            .To<DocumentTypeView>()
            .OrderByDescending(documentType => documentType.Id);
    }

    public void Create(DocumentTypeView view)
    {
        DocumentType documentType = UnitOfWork.To<DocumentType>(view);

        UnitOfWork.Insert(documentType);
        UnitOfWork.Commit();
    }

    public void Edit(DocumentTypeView view)
    {
        DocumentType documentType = UnitOfWork.To<DocumentType>(view);

        UnitOfWork.Update(documentType);
        UnitOfWork.Commit();
    }

    public void Delete(int id)
    {
        UnitOfWork.Delete<DocumentType>(id);
        UnitOfWork.Commit();
    }
}
