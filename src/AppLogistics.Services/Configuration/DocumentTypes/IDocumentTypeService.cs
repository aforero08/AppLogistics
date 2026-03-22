using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public interface IDocumentTypeService : IService
{
    TView Get<TView>(int id) where TView : BaseView;
    IQueryable<DocumentTypeView> GetViews();

    void Create(DocumentTypeView view);
    void Edit(DocumentTypeView view);
    void Delete(int id);
}
