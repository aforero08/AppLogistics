using AppLogistics.Objects;

namespace AppLogistics.Validators;

public interface IDocumentTypeValidator : IValidator
{
    bool CanCreate(DocumentTypeView view);
    bool CanEdit(DocumentTypeView view);
    bool CanDelete(int id);
}
