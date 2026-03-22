using AppLogistics.Components.Extensions;
using AutoMapper;
using NonFactors.Mvc.Lookup;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AppLogistics.Objects;

public class RoleView : BaseView<Role>
{
    [Required]
    [LookupColumn]
    [StringLength(128)]
    public string Title { get; set; }

    public MvcTree Permissions { get; set; }

    public RoleView()
    {
        Permissions = new MvcTree();
    }

    internal override void Map(Profile profile)
    {
        profile.CreateMap<Role, RoleView>().ForMember(role => role.Permissions, member => member.MapFrom(role =>
            new MvcTree { SelectedIds = new HashSet<int>(role.Permissions.Select(role => role.PermissionId)) }));
        profile.CreateMap<RoleView, Role>().ForMember(role => role.Permissions, member => member.MapFrom(role =>
            role.Permissions.SelectedIds.Select(permission => new RolePermission { PermissionId = permission }).ToList()));
    }
}
