using System.Collections.Generic;
using System.Linq;

namespace Domain.Common.SystemMenu
{
    public class SystemMenu : IAggregateRoot
    {
        /// <summary>
        /// 图标来源http://fontawesome.dashgame.com
        /// </summary>
        public SystemMenu(long id, string name, string url, long parenid, SystemMenuType type, int orderIndex = 0, bool isDefault = false, string icon = "")
        {
            this.Id = id;
            this.Name = name;
            this.Url = url;
            this.ParenId = parenid;
            this.OrderIndex = orderIndex;
            this.IsDefault = isDefault;
            this.SystemMenuType = type;
            this.Icon = icon;
        }

        public SystemMenu(long id, string name, string url, long parenid, int orderIndex = 0, bool isDefault = false)
        {
            this.Id = id;
            this.Name = name;
            this.Url = url;
            this.ParenId = parenid;
            this.OrderIndex = orderIndex;
            this.IsDefault = isDefault;
        }
        public long Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public long ParenId { get; set; }

        /// <summary>
        /// 默认选中
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public int OrderIndex { set; get; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        public SystemMenuType SystemMenuType { get; set; }

        public List<SystemMenu> GetSystemMenuList(List<SystemMenu> list)
        {
            return list.Where(p => p.ParenId == this.Id).ToList();
        }
    }
}
