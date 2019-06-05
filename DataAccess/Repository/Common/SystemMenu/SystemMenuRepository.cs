using Domain.Common.SystemMenu;
using System.Collections.Generic;

namespace DataAccess.Repository.Common.SystemMenu
{
    public class SystemMenuRepository : BaseRepository<Domain.Common.SystemMenu.SystemMenu>, ISystemMenuRepository
    {
        public override List<Domain.Common.SystemMenu.SystemMenu> GetAll()
        {
            return MenuHelper.GetMenuList();
        }
    }
}
