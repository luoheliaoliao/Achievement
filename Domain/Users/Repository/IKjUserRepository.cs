using System.Collections.Generic;

namespace Domain.Users.Repository
{
    public interface IKjUserRepository : IRepository<KjUser>
    {
        List<RoleMenu> GetRoleMenuByUserId(long userId);

        List<RoleButton> GetRoleButtonByUserId(long userId);

        List<Role> GetRoleByUserId(long userId);
        List<UserRole> GetUserRoleByUserId(long userId);

        KjUser GetByAccount(string account);

        void BatchAddUserRole(List<UserRole> userRoles);

        void BatchUpdateUserRole(List<UserRole> userRoles);

        void BatchDeleteUserRole(List<UserRole> userRoles);
    }
}
