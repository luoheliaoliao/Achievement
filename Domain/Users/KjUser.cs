using Domain.Common.SystemMenu;
using Domain.Users.Repository;
using Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace Domain.Users
{
    public class KjUser : IUser, ILazyLoad
    {

        public KjUser()
        {
            IsAdmin = false;
            IsEnable = true;
            AddTime = DateTime.Now;
        }
        public void InitLazyLoad()
        {
            KjUserRepository = IocContainer.Resolve<IKjUserRepository>();
        }

        protected IKjUserRepository KjUserRepository;

        private List<RoleMenu> _roleMenuList;

        [NotMapped, XmlIgnore, JsonIgnore]
        public List<RoleMenu> RoleMenuList
        {
            get
            {
                if (_roleMenuList != null)
                    return _roleMenuList;
                if (KjUserRepository == null)
                    return new List<RoleMenu>();
                return KjUserRepository.GetRoleMenuByUserId(this.Id);
            }
            set
            {
                _roleMenuList = value;
            }
        }

        private List<Role> _roleList;

        [NotMapped, XmlIgnore, JsonIgnore]
        public List<Role> RoleList
        {
            get
            {
                if (_roleList != null)
                    return _roleList;
                if (KjUserRepository == null)
                    return new List<Role>();
                return KjUserRepository.GetRoleByUserId(this.Id);
            }
            set
            {
                _roleList = value;
            }
        }

        private List<UserRole> _userRoleist;

        [NotMapped, XmlIgnore, JsonIgnore]
        public List<UserRole> UserRoleList
        {
            get
            {
                if (_roleList != null)
                    return _userRoleist;
                if (KjUserRepository == null)
                    return new List<UserRole>();
                return KjUserRepository.GetUserRoleByUserId(this.Id);
            }
            set
            {
                _userRoleist = value;
            }
        }

        public long Id { get; set; }

        public string Name { get; set; }
        public string Password { get; set; }
        public string Account { get; set; }
        public string Image { get; set; }
        public UserType UserType { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsEnable { get; set; }
        public DateTime AddTime { get; set; }

    }
}
