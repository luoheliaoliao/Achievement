using Domain.Users;
using Domain.Users.Repository;
using Infrastructure;
using Infrastructure.Exceptions;
using Infrastructure.Security;
using System.Collections.Generic;
using System.Linq;

namespace Service.User
{
    public class UserService
    {
        public IKjUserRepository UserRepository = IocContainer.Resolve<IKjUserRepository>();

        public void Add(string name, string account, string password, string image, UserType userType, List<long> roleIds)
        {
            if (string.IsNullOrEmpty(account))
                throw new AlertException("账号不能为空！");
            if (string.IsNullOrEmpty(name))
                throw new AlertException("用户名称不能为空！");
            if (string.IsNullOrEmpty(password))
                throw new AlertException("密码不能为空！");

            if (UserRepository.GetByAccount(account) != null)
                throw new AlertException("账号已存在！");

            var user = new KjUser()
            {
                Name = name,
                Password = MD5Helper.Encrypt(password),
                Account = account,
                Image = string.IsNullOrEmpty(image) ? "" : image,
                UserType = userType
            };

            UserRepository.Add(user);
            if (!roleIds.Any())
                return;

            var userRoles = new List<UserRole>();
            roleIds.ForEach(p =>
            {
                userRoles.Add(new UserRole() { RoleId = p, UserId = user.Id });
            });
            UserRepository.BatchAddUserRole(userRoles);
        }

        public void Update(long id, string name, string account, string password, string image, UserType userType, List<long> roleIds)
        {
            if (string.IsNullOrEmpty(account))
                throw new AlertException("账号不能为空！");
            if (string.IsNullOrEmpty(name))
                throw new AlertException("用户名称不能为空！");
            if (string.IsNullOrEmpty(password))
                throw new AlertException("密码不能为空！");

            var user = UserRepository.GetById(id);
            if (user != null)
                throw new AlertException("该账号不存在！");

            if (user.Account != account && UserRepository.GetByAccount(account) != null)
                throw new AlertException("当前修改的账号已存在！");

            user.Name = name;
            user.Account = account;
            user.Image = string.IsNullOrEmpty(image) ? "" : image;
            user.UserType = userType;
            if (user.Password != password)
            {
                user.Password = MD5Helper.Encrypt(password);
            }
            UserRepository.Update(user);
            UserRepository.BatchDeleteUserRole(user.UserRoleList);

            if (!roleIds.Any())
                return;

            var userRoles = new List<UserRole>();
            roleIds.ForEach(p =>
            {
                userRoles.Add(new UserRole() { RoleId = p, UserId = user.Id });
            });
            UserRepository.BatchAddUserRole(userRoles);
        }

        public void Delete(List<long> ids)
        {
            if (!ids.Any())
                throw new AlertException("未选择任何项！");
            var users = UserRepository.GetListByIdList(ids);
            if (users.Any(p => p.IsAdmin))
                throw new AlertException("主账号不允许删除！");
            UserRepository.Delete(users);
        }

        public void Delete(long id)
        {
            var users = GetById(id);
            if (users.IsAdmin)
                throw new AlertException("主账号不允许删除！");
            UserRepository.Delete(users);
        }

        public void Forbidden(long id)
        {
            var user = GetById(id);
            user.IsEnable = false;
            UserRepository.Update(user);
        }

        public void Enable(long id)
        {
            var user = GetById(id);
            user.IsEnable = true;
            UserRepository.Update(user);
        }


        public KjUser GetById(long id)
        {
            var user = UserRepository.GetById(id);
            if (user == null)
                throw new AlertException("该账号不存在！");
            return user;
        }

        public KjUser GetByAccount(string account)
        {
            var user = UserRepository.GetByAccount(account);
            if (user == null)
                throw new AlertException("该账号不存在！");
            return user;
        }
    }
}
