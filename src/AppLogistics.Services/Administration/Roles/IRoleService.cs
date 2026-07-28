using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public interface IRoleService : IService
{
    void SeedPermissions(RoleView view);

    IQueryable<RoleView> GetViews();

    RoleView GetView(int id);

    void Create(RoleView view);

    void Edit(RoleView view);

    void Delete(int id);
}
