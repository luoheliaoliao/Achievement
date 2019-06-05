using Achievement.DataAccess;
using Domain.Common.SystemMenu;
using Domain.Users;
using Domain.Users.Repository;
using Domain.ViewModel;
using Infrastructure;
using Infrastructure.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Service.User
{
    public class RoleService
    {
        public IRolerRepository RolerRepository = IocContainer.Resolve<IRolerRepository>();
        public ISystemMenuRepository SystemMenuRepository = IocContainer.Resolve<ISystemMenuRepository>();

        public Role GetById(long id)
        {
            var result = RolerRepository.GetById(id);
            if (result == null)
                throw new AlertException("权限不存在！");
            return result;
        }

        [Transaction]
        public void Add(string name, List<long> menuIds, string desc)
        {
            var role = new Role() { Name = name, Desc = desc };
            RolerRepository.Add(role);

            if (menuIds.Any())
            {
                var menuRole = new List<RoleMenu>();
                menuIds.ForEach(p => menuRole.Add(new RoleMenu() { RoleId = role.Id, MenuId = p }));
                RolerRepository.AddRoleMenu(menuRole);
            }
        }

        [Transaction]
        public void Edit(long id, string name, List<long> menuIds, string desc)
        {
            var role = GetById(id);
            role.Name = name;
            role.Desc = desc;
            RolerRepository.Update(role);
            RolerRepository.DeleteRoleMenu(role.RoleMenuList);
            if (menuIds.Any())
            {
                var menuRole = new List<RoleMenu>();
                menuIds.ForEach(p => menuRole.Add(new RoleMenu() { RoleId = role.Id, MenuId = p }));
                RolerRepository.AddRoleMenu(menuRole);
            }
        }


        public void BatchDelete(List<long> ids)
        {
            if (!ids.Any())
                throw new AlertException("未选择任何项！");
            ids.ForEach(p => Delete(p));
        }

        public void Delete(long id)
        {
            var role = GetById(id);
            RolerRepository.Delete(role);
            RolerRepository.DeleteRoleMenu(role.RoleMenuList);
            RolerRepository.DeleteUserRoleByRoleId(id);
        }


        public List<TreeMolde> GetMenuList(long id)
        {
            var models = new List<TreeMolde>();
            var menus = SystemMenuRepository.GetAll();
            var roleMenus = RolerRepository.GetRoleMenuByRoleId(id);
            foreach (var item in menus.Where(p => p.ParenId == 0))
            {
                var model = new TreeMolde()
                {
                    id = item.Id,
                    text = item.Name
                };
                AddChildMenuList(model, item, menus, roleMenus);
                models.Add(model);
            }
            return models;
        }

        private void AddChildMenuList(TreeMolde model, SystemMenu systemMenu, List<SystemMenu> systemMenus, List<RoleMenu> roleMenus)
        {
            if(!systemMenu.GetSystemMenuList(systemMenus).Any())
            {
                if(roleMenus.Any(p=>p.MenuId==model.id))
                {
                    model.@checked = true;
                }
                return;
            }
            foreach (var item in systemMenu.GetSystemMenuList(systemMenus))
            {
                var modelChildren = new TreeMolde()
                {
                    id = item.Id,
                    text = item.Name
                };
                AddChildMenuList(modelChildren, item, systemMenus, roleMenus);
                model.children.Add(modelChildren);
            }
        }
    }
}
