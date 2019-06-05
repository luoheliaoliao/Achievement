using Domain.Users;
using Domain.Users.Repository;
using Infrastructure;
using Infrastructure.Extends;
using System.Web;
using System.Web.Security;

namespace Domain
{

    public class RuntimeHelper
    {
        public static KjUser CurrentUser
        {
            get
            {
                return IocContainer.Resolve<IKjUserRepository>().GetById(CurrentUserId);
            }
        }

        public static long CurrentUserId
        {
            get
            {
                var cookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie == null)
                {
                    FormsAuthentication.SignOut();
                    HttpContext.Current.Response.Redirect("/");
                    return 0;
                }
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                return ticket.UserData.AsLong();
            }
        }

    }

}
