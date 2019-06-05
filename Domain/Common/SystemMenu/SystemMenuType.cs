using Infrastructure.Extends;

namespace Domain.Common.SystemMenu
{
    public enum SystemMenuType
    {
        [EnumDescription("模板")]
        TempTable = 0,
        [EnumDescription("系统管理")]
        SystemManager = 1
    }
}
