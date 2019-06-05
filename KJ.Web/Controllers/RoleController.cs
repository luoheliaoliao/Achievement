using Domain.Users;
using Service.User;
using System.Collections.Generic;
using System.Web.Mvc;

namespace KJ.Web.Controllers
{
    public class RoleController : BaseController
    {
        public RoleService RoleService = new RoleService();
        // GET: Role
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Edit(long id = 0)
        {
            var role = id == 0 ? new Role() : RoleService.GetById(id);
            return View(role);
        }


        public JsonResult GetMenuList(long id)
        {
            return Json(RoleService.GetMenuList(id));
        }

        public JsonResult Update(long id,string name,string desc,List<long> menuIds=null)
        {
            if(id==0)
            {
                RoleService.Add(name, menuIds, desc);
            }else
            {
                RoleService.Edit(id,name, menuIds, desc);

            }
            return Json();
        }


       public JsonResult BatchDelete(List<long> ids)
        {
            RoleService.BatchDelete(ids);
            return Json();
        }


        public JsonResult Delete(long id)
        {
            RoleService.Delete(id);
            return Json();
        }

    }
}