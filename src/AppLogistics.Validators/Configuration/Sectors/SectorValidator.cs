using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Resources;
using System.Linq;

namespace AppLogistics.Validators
{
    public class SectorValidator : BaseValidator, ISectorValidator
    {
        public SectorValidator(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public bool CanCreate(SectorCreateEditView view)
        {
            return ModelState.IsValid;
        }

        public bool CanEdit(SectorCreateEditView view)
        {
            return ModelState.IsValid;
        }

        public bool CanDelete(int id)
        {
            var hasReferencedServices = UnitOfWork.Select<Service>()
                .Where(c => c.SectorId.Equals(id))
                .Any();

            if (hasReferencedServices)
            {
                Alerts.AddError(Validation.For<SectorView>("AssociatedServices"));
                return false;
            }

            return ModelState.IsValid;
        }
    }
}
