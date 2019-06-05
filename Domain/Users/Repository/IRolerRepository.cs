using Domain.Users;
using System.Collections.Generic;

namespace Domain.Users.Repository
{
    public interface IRolerRepository : IRepository<Role>
    {
        void AddRoleButton(List<RoleButton> list);
        void DeleteRoleButton(List<RoleButton> list);
        void UpdateRoleButton(List<RoleButton> list);

        void AddRoleMenu(List<RoleMenu> list);
        void DeleteRoleMenu(List<RoleMenu> list);
        void UpdateRoleMenu(List<RoleMenu> list);
        List<RoleMenu> GetRoleMenuByRoleId(long roleId);

        List<RoleButton> GetRoleButtonByRoleId(long roleId);


        void DeleteUserRoleByRoleId(long roleId);
    }
}
