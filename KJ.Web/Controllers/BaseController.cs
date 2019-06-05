using Infrastructure.Exceptions;
using Infrastructure.ImportAndExport;
using Infrastructure.MVC;
using Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace KJ.Web.Controllers
{
    public abstract class BaseController : Controller, IDisposable
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            //页面标识
            this.ViewBag.PageTag = GetPageTag();
        }

        private string GetPageTag()
        {
            string path = System.Web.HttpContext.Current.Request.Path;
            path = path.Trim('/');
            return path.Replace("/", ".");
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var actionParameters = filterContext.ActionParameters.Where(p => p.Value == null).ToList();
            if (actionParameters.Any())
            {
                var reflectedActionDescriptor = filterContext.ActionDescriptor as ReflectedActionDescriptor;
                if (reflectedActionDescriptor != null)
                {
                    var paramters = reflectedActionDescriptor.MethodInfo.GetParameters();
                    foreach (var actionParameter in actionParameters)
                    {
                        var paramter = paramters.First(p => p.Name == actionParameter.Key);
                        if (paramter.ParameterType == typeof(string) || (paramter.ParameterType.IsValueType && Nullable.GetUnderlyingType(paramter.ParameterType) == null))
                        {
                          //  LogHelper.LogException(new AlertException(string.Format("请求\"{0}\"调用失败，参数\"{1}\"不能为空或类型不正确！", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "/" + filterContext.ActionDescriptor.ActionName, actionParameter.Key)));
                            throw new AlertException(string.Format("请求\"{0}\"调用失败，参数\"{1}\"不能为空或类型不正确！", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "/" + filterContext.ActionDescriptor.ActionName, actionParameter.Key));
                        }
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// 记录页面错误日志
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            if (Request.HttpMethod.ToUpper() == "POST")
            {
                var result = AjaxError(filterContext);
                filterContext.HttpContext.Response.Clear();
                if (filterContext.Exception is AlertException)
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                else
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                filterContext.Result = result;
                filterContext.ExceptionHandled = true;
            }
            else
            {
                if (filterContext.Exception is AlertException)
                    Response.Redirect(Url.Content("~/VX/Home/AlertInfo?message=" + filterContext.Exception.Message));
            }

         //   LogHelper.LogException(filterContext.Exception);
            base.OnException(filterContext);
        }

        protected JsonResult AjaxError(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception;
            //Needed for IIS7.0
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            filterContext.ExceptionHandled = true;
            return new JsonResult
            {
                //can extend more properties 
                Data = new { Tag = "-999", Message = exception.Message, StatckTrace = exception.StackTrace },
                ContentEncoding = System.Text.Encoding.UTF8,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            ServiceBus.DisposeDb();
        }

        protected new JsonResult Json(object data = null)
        {
            return new CustomJsonResult(data ?? "");
        }

        protected FileResult ExcelExport(string format, DataTable data, string fileName)
        {
            var stream = ExcelHelp.ExportMin.ExportXlsx(format, data);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName + ".xlsx");
        }
        protected FileResult ExcelExport<T>(string format, List<T> data, string fileName)
        {
            var stream = ExcelHelp.ExportMin.ExportXlsx(format, data);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName + ".xlsx");
        }

    }
}