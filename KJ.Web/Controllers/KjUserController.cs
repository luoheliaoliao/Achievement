using Domain.Users;
using Infrastructure.Extends;
using Service.User;
using System.Web.Mvc;

namespace KJ.Web.Controllers
{
    public class KjUserController : BaseController
    {
        public UserService UserService = new UserService();
        public ActionResult List()
        {
   
            return View();
        }

        public ActionResult Edit(long id = 0)
        {
            var user = id != 0 ? UserService.GetById(id) : new KjUser();
            return View(user);
        }
    }
}