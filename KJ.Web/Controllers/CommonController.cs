using Infrastructure;
using Infrastructure.Exceptions;
using Infrastructure.Extends;
using Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KJ.Web.Controllers
{
    public class CommonController : BaseController
    {
        public CommonReportService CommonReportService = IocContainer.Resolve<CommonReportService>();
        public FileService FileService = IocContainer.Resolve<FileService>();

        public JsonResult GetPageData(string reportName, bool isShowFooter = false, bool isSuperAdmin = false)
        {
            var pars = new SqlParams().Fill();
            //if (RuntimeHelper.CurrentUser.AchievementUserType == Achievement.Domain.User.AchievementUserType.Implement)
            //{
            //    pars["ImplementIds"] = RuntimeHelper.CurrentUser.ImplementIds;
            //}
            var result = CommonReportService.GetPageData(reportName, pars, isShowFooter);
            return Json(result);
        }

        public ActionResult GetTotalData(string reportName)
        {
            var pars = new SqlParams().Fill();
            //if (RuntimeHelper.CurrentUser.AchievementUserType == Achievement.Domain.User.AchievementUserType.Implement)
            //{
            //    pars["ImplementIds"] = RuntimeHelper.CurrentUser.ImplementIds;
            //}

            var result = CommonReportService.GetSummaryData(reportName, pars);
            if (result == null)
            {
                return Json();
            }
            return Content(result.ToJson().TrimStart('[').TrimEnd(']'), "application/json");
        }

        public JsonResult GetExportData(string reportName)
        {
            var pars = new SqlParams().Fill();


            var result = CommonReportService.GetExportData(reportName, pars);
            return Json(result);
        }

        public JsonResult GetFullOperationIds(string reportName)
        {
            var pars = new SqlParams().Fill();

            var result = CommonReportService.GetFullOperationIds(reportName, pars);
            return Json(result);
        }

        public ViewResult ExportView(string format, string detalFormat = "")
        {
            if (format.Length > 0)
            {
                var arr = format.Split(';');
                var items = arr.Select(str => str.Split('|')).ToDictionary(array => array[0], array => array[1]);
                ViewBag.Items = items;
            }

            if (detalFormat.Length <= 0) return View();
            var arrDetail = detalFormat.Split(';');
            var details = arrDetail.Select(str => str.Split('|')).ToDictionary(array => array[0], array => array[1]);
            ViewBag.Details = details;

            return View();
        }

        public ActionResult ExportData(string reportName, string format, string reportTitle)
        {
            var pars = new SqlParams().Fill();

            var data = CommonReportService.GetExportData(reportName, pars);
            reportTitle += DateTime.Now.ToString("yyyy-MM-dd");

            return ExcelExport(format, data, reportTitle);
        }


        public JsonResult UploadImage()
        {
            var files = Request.Files;
            if (files == null || files.Count == 0)
                throw new AlertException("上传错误，没找到要上传的图片！");
            var result = FileService.UploadImage(files[0]);
            return Json(result);
        }

        public JsonResult UploadImageByBase64(string image)
        {
            string ext = string.Empty;
            string base64Code = string.Empty;
            if (image.Contains("data:image/jpeg;base64,"))
            {
                ext = ".jpg";
                base64Code = image.Substring(23);
            }
            else if (image.Contains("data:image/png;base64,"))
            {
                ext = ".png";
                base64Code = image.Substring(22);
            }
            else
            {
                throw new AlertException("{\"Msg\":\"文件格式只支持JPG、PNG！\"}");
            }
            //base64string到byte[]再到图片的转换：
            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64Code);
                var result = FileService.UploadImage(imageBytes, ext);
                return Json(result);
            }
            catch (Exception ex)
            {

                throw new AlertException(ex.Message);
            }
            //读入MemoryStream对象

            //var files = Request.Files;
            //if (files == null || files.Count == 0)
            //    throw new AlertException("上传错误，没找到要上传的图片！");
        }

        public JsonResult DeleteImage(string imageUrl)
        {
            FileService.DeleteImage(imageUrl);
            return Json();
        }

        public JsonResult UploadKindEditerImage()
        {
            var files = Request.Files;
            if (files == null || files.Count == 0)
                return Json(new KindEditerResult(error: 1, message: "上传错误，没找到要上传的图片！"));
            try
            {
                var result = FileService.UploadImage(files[0]);
                var r = new KindEditerResult(result);
                return Json(r);
            }
            catch (Exception e)
            {
                var r = new KindEditerResult(error: 1, message: e.Message);
                return Json(r);
            }
        }
    }
}