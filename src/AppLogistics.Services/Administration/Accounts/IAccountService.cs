using AppLogistics.Objects;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace AppLogistics.Services;

public interface IAccountService : IService
{
    TView Get<TView>(int id) where TView : BaseView;

    IQueryable<AccountView> GetViews();

    bool IsLoggedIn(IPrincipal user);

    bool IsActive(int id);

    string Recover(AccountRecoveryView view);

    void Reset(AccountResetView view);

    void Create(AccountCreateView view);

    void Edit(AccountEditView view);

    void Edit(ClaimsPrincipal user, ProfileEditView view);

    void Delete(int id);

    Task Login(HttpContext context, string username);

    Task Logout(HttpContext context);
}
