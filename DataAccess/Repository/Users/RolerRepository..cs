using Domain.Users;
using Domain.Users.Repository;
using EntityFramework.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace DataAccess.Repository.Common.SystemMenu
{
    public class RolerRepository : BaseRepository<Role>, IRolerRepository
    {
        public void AddRoleButton(List<RoleButton> list)
        {
            Db.RoleButton.InsertRange(list);
            Db.SaveChanges();
        }

        public void AddRoleMenu(List<RoleMenu> list)
        {
            Db.RoleMenu.InsertRange(list);
            Db.SaveChanges();
        }

        public void DeleteRoleButton(List<RoleButton> list)
        {
            Db.DeleteEntity(list);
            Db.SaveChanges();
        }

        public void DeleteRoleMenu(List<RoleMenu> list)
        {
            Db.DeleteEntity(list);
            Db.SaveChanges();
        }

        public void DeleteUserRoleByRoleId(long roleId)
        {
            Db.UserRole.Where(p => p.RoleId == roleId).Delete();
            Db.SaveChanges();
        }

        public List<RoleButton> GetRoleButtonByRoleId(long roleId)
        {
            return Db.RoleButton.AsNoTracking().Where(p => p.RoleId == roleId).ToList();
        }

        public List<RoleMenu> GetRoleMenuByRoleId(long roleId)
        {
            return Db.RoleMenu.AsNoTracking().Where(p => p.RoleId == roleId).ToList();
        }

        public void UpdateRoleButton(List<RoleButton> list)
        {
            Db.ModifiyEntity(list);
            Db.SaveChanges();
        }

        public void UpdateRoleMenu(List<RoleMenu> list)
        {
            Db.ModifiyEntity(list);
            Db.SaveChanges();
        }
    }
}
