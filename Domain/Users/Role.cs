using Domain.Users.Repository;
using Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace Domain.Users
{
    public class Role : IAggregateRoot,ILazyLoad
    {
        public Role()
        {
            AddTime = DateTime.Now;
        }
        public void InitLazyLoad()
        {
            RolerRepository= IocContainer.Resolve<IRolerRepository>();
        }

        private List<RoleMenu> _roleMenuList;

        [NotMapped, XmlIgnore, JsonIgnore]
        public List<RoleMenu> RoleMenuList
        {
            get
            {
                if (_roleMenuList != null)
                    return _roleMenuList;
                if (RolerRepository == null)
                    return new List<RoleMenu>();
                return RolerRepository.GetRoleMenuByRoleId(this.Id);
            }
            set
            {
                _roleMenuList = value;
            }
        }

        private List<RoleButton> _roleButtonList;

        [NotMapped, XmlIgnore, JsonIgnore]
        public List<RoleButton> RoleButtonList
        {
            get
            {
                if (_roleButtonList != null)
                    return _roleButtonList;
                if (RolerRepository == null)
                    return new List<RoleButton>();
                return RolerRepository.GetRoleButtonByRoleId(this.Id);
            }
            set
            {
                _roleButtonList = value;
            }
        }

        protected IRolerRepository RolerRepository;
        public long Id { get; set; }

        public string Name { get; set; }
        public string Desc { get; set; }
        public DateTime AddTime { get; set; }


    }
}
