using AppLogistics.Objects;

namespace AppLogistics.Validators;

public interface IEthnicGroupValidator : IValidator
{
    bool CanCreate(EthnicGroupView view);
    bool CanEdit(EthnicGroupView view);
    bool CanDelete(int id);
}
