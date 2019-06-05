using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Common.SystemMenu
{
    public class MenuHelper
    {
        public static List<SystemMenu> GetMenuList()
        {
            var systemMenus = new List<SystemMenu>();
            systemMenus.Add(new SystemMenu(11, "权限管理", "", 0, SystemMenuType.SystemManager, icon: "cog"));
            systemMenus.Add(new SystemMenu(111, "用户管理", "~/KjUser/List", 11));
            systemMenus.Add(new SystemMenu(112, "角色管理", "~/Role/List", 11));
            systemMenus.Add(new SystemMenu(10, "样式模板", "", 0, SystemMenuType.TempTable, icon: "university"));
            systemMenus.Add(new SystemMenu(101, "报表", "~/Template/Table", 10));
            systemMenus.Add(new SystemMenu(102, "表单", "~/Template/From1", 10));
            if (systemMenus.GroupBy(p => p.Id).Any(p => p.Count() > 1))
            {
                throw new Exception("存在重复的菜单Id");
            }

            return systemMenus;
        }
    }
}
