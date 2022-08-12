using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Resources;
using System.Linq;

namespace AppLogistics.Validators
{
    public class ClientValidator : BaseValidator, IClientValidator
    {
        public ClientValidator(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public bool CanCreate(ClientCreateEditView view)
        {
            return IsUniqueNit(view.Id, view.Nit) && ModelState.IsValid;
        }

        public bool CanEdit(ClientCreateEditView view)
        {
            return IsUniqueNit(view.Id, view.Nit) && ModelState.IsValid;
        }

        public bool CanDelete(int id)
        {
            var hasReferencedRates = UnitOfWork.Select<Rate>()
                .Where(c => c.ClientId.Equals(id))
                .Any();

            if (hasReferencedRates)
            {
                Alerts.AddError(Validation.For<ClientCreateEditView>("AssociatedRates"));
                return false;
            }

            var hasReferencedSectors = UnitOfWork.Select<Sector>()
                .Where(c => c.ClientId.HasValue && c.ClientId.Value.Equals(id))
                .Any();

            if (hasReferencedSectors)
            {
                Alerts.AddError(Validation.For<ClientCreateEditView>("AssociatedSectors"));
                return false;
            }

            return ModelState.IsValid;
        }

        private bool IsUniqueNit(int clientId, string nit)
        {
            var alreadyExists = UnitOfWork.Select<Client>()
                .Where(c => c.Nit.Equals(nit) && c.Id != clientId)
                .Any();

            if (alreadyExists)
            {
                Alerts.AddError(Validation.For<ClientCreateEditView>("NotUniqueNit"));
                return false;
            }

            return true;
        }
    }
}
