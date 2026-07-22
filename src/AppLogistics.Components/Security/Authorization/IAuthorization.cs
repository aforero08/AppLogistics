namespace AppLogistics.Components.Security;

public interface IAuthorization
{
    bool IsGrantedFor(int? accountId, string area, string controller, string action);

    void Refresh();
}
