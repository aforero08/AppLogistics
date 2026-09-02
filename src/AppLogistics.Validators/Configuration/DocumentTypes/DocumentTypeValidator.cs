using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Resources;
using System.Linq;

namespace AppLogistics.Validators;

public class DocumentTypeValidator : BaseValidator, IDocumentTypeValidator
{
    public DocumentTypeValidator(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public bool CanCreate(DocumentTypeView view)
    {
        var alreadyExists = UnitOfWork.Select<DocumentType>()
            .Where(c => c.Name.ToUpper().Equals(view.Name.ToUpper()))
            .Any();
        
        if (alreadyExists)
        {
            Alerts.AddError(Validation.For<DocumentTypeView>("DuplicateName"));
            return false;
        }
        
        return ModelState.IsValid;
    }

    public bool CanEdit(DocumentTypeView view)
    {
        var alreadyExists = UnitOfWork.Select<DocumentType>()
            .Where(c => c.Id != view.Id && c.Name.ToUpper().Equals(view.Name.ToUpper()))
            .Any();
        
        if (alreadyExists)
        {
            Alerts.AddError(Validation.For<DocumentTypeView>("DuplicateName"));
            return false;
        }
        
        return ModelState.IsValid;
    }

    public bool CanDelete(int id)
    {
        var hasReferencedEmployees = UnitOfWork.Select<Employee>()
            .Where(c => c.DocumentTypeId.Equals(id))
            .Any();

        if (hasReferencedEmployees)
        {
            Alerts.AddError(Validation.For<DocumentTypeView>("AssociatedEmployees"));
            return false;
        }

        return ModelState.IsValid;
    }
}
