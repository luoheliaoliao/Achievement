using System.Collections.Generic;
using System.Web.Mvc;

namespace KJ.Web.Controllers
{
    public class infortest
    {
        public string name { get; set; }

        public string sex { get; set; }
        public string school { get; set; }

    }
    public class TemplateController : Controller
    {
        // GET: Template
        public ActionResult Table()
        {
            return View();
        }


        public JsonResult TableData()
        {
            var infortests = new List<infortest>();
            infortests.Add(new infortest() { name = "张三", school = "历史", sex = "男" });
            infortests.Add(new infortest() { name = "李四", school = "飒飒", sex = "男" });
            infortests.Add(new infortest() { name = "账务", school = "as", sex = "飒飒" });
            infortests.Add(new infortest() { name = "账务", school = "as", sex = "飒飒" });
            infortests.Add(new infortest() { name = "账务", school = "as", sex = "飒飒" });
            infortests.Add(new infortest() { name = "账务", school = "as", sex = "飒飒" });
            infortests.Add(new infortest() { name = "账务", school = "as", sex = "飒飒" });
            infortests.Add(new infortest() { name = "账务", school = "as", sex = "飒飒" });
            infortests.Add(new infortest() { name = "账务", school = "as", sex = "飒飒" });
            infortests.Add(new infortest() { name = "账务", school = "as", sex = "飒飒" });
            infortests.Add(new infortest() { name = "账务", school = "as", sex = "飒飒" });
            infortests.Add(new infortest() { name = "账务", school = "as", sex = "飒飒" });
            infortests.Add(new infortest() { name = "账务", school = "as", sex = "飒飒" });
            infortests.Add(new infortest() { name = "账务", school = "as", sex = "飒飒" });
            infortests.Add(new infortest() { name = "账务", school = "as", sex = "飒飒" });
            infortests.Add(new infortest() { name = "账务", school = "as", sex = "飒飒" });
            infortests.Add(new infortest() { name = "账务", school = "as", sex = "飒飒" });
            infortests.Add(new infortest() { name = "账务", school = "as", sex = "飒飒" });
            infortests.Add(new infortest() { name = "账务", school = "as", sex = "飒飒" });
            infortests.Add(new infortest() { name = "账务", school = "as", sex = "飒飒" });
            infortests.Add(new infortest() { name = "账务", school = "as", sex = "飒飒" });
            infortests.Add(new infortest() { name = "账务", school = "as", sex = "飒飒" });
            infortests.Add(new infortest() { name = "账务", school = "as", sex = "飒飒" });
            infortests.Add(new infortest() { name = "账务", school = "as", sex = "飒飒" });
            infortests.Add(new infortest() { name = "账务", school = "as", sex = "飒飒" });
            return new JsonResult() { Data = new { rows = infortests, total = 1 }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        

        public ActionResult FromData()
        {
            return View();
        }

        public ActionResult From1()
        {
            return View();
        }

        public ActionResult Files()
        {
            return View();
        }
    }
}