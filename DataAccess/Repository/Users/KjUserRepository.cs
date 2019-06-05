
using Domain;
using Domain.Users;
using Domain.Users.Repository;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Repository.Common.SystemMenu
{
    public class KjUserRepository : BaseRepository<KjUser>, IKjUserRepository
    {
        public void BatchAddUserRole(List<UserRole> userRoles)
        {
            Db.UserRole.InsertRange(userRoles);
            Db.SaveChanges();
        }

        public void BatchUpdateUserRole(List<UserRole> userRoles)
        {
            Db.ModifiyEntity(userRoles);
            Db.SaveChanges();
        }

        public void BatchDeleteUserRole(List<UserRole> userRoles)
        {
            Db.DeleteEntity(userRoles);
            Db.SaveChanges();
        }

        public KjUser GetByAccount(string account)
        {
            return Db.KjUser.AsNoTracking().FirstOrDefault(p => p.Account == account).LazyLoad();
        }

        public List<RoleButton> GetRoleButtonByUserId(long roleId)
        {
            return (from u in Db.KjUser.AsNoTracking()
                    join ur in Db.UserRole.AsNoTracking() on u.Id equals ur.UserId
                    join rb in Db.RoleButton.AsNoTracking() on ur.RoleId equals rb.ButtonId
                    where u.Id == roleId
                    select rb).Distinct().ToList();
        }

        public List<Role> GetRoleByUserId(long roleId)
        {
            return (from u in Db.KjUser.AsNoTracking()
                    join ur in Db.UserRole.AsNoTracking() on u.Id equals ur.UserId
                    join rb in Db.Role.AsNoTracking() on ur.RoleId equals rb.Id
                    where u.Id == roleId
                    select rb).Distinct().ToList();
        }

        public List<RoleMenu> GetRoleMenuByUserId(long roleId)
        {
            return (from u in Db.KjUser.AsNoTracking()
                    join ur in Db.UserRole.AsNoTracking() on u.Id equals ur.UserId
                    join rb in Db.RoleMenu.AsNoTracking() on ur.RoleId equals rb.MenuId
                    where u.Id == roleId
                    select rb).Distinct().ToList();
        }

        public List<UserRole> GetUserRoleByUserId(long userId)
        {
            return Db.UserRole.AsNoTracking().Where(p => p.UserId == userId).ToList();
        }


    }
}

