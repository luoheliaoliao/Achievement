using Infrastructure;
using Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Service.Common
{
    public class FileService : BaseService
    {
        public string UploadImage(HttpPostedFileBase file)
        {
            var maxSize = 1024 * 1024 * 1;
            if (file.ContentLength > maxSize)
            {
                throw new AlertException("上传的文件不允许大于" + (maxSize / 1024) + "KB，请调整！");
            }

            var result = UploadImageNoSizeLimit(file);
            return result;
        }

        public string UploadImage(byte[] imageBytes, string fileType)
        {
            string result = "";
            var fileName = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(DateTime.Now.Ticks.ToString(), "MD5");
            var baichuanBasePath = "upos/";
            var virtualPath = HttpContext.Current.Request.ApplicationPath != null &&
                              HttpContext.Current.Request.ApplicationPath.Length > 1
                ? HttpContext.Current.Request.ApplicationPath.Replace("/", "-")
                : "";
            string uploadDir = baichuanBasePath.TrimEnd('/') + "/" + HttpContext.Current.Request.Url.DnsSafeHost.Replace(':', '.') + virtualPath;
            MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
            memoryStream.Write(imageBytes, 0, imageBytes.Length);
            return result;
        }

        public string UploadImageLimitTenM(HttpPostedFileBase file)
        {
            var maxSize = 1024 * 1024 * 10;
            if (file.ContentLength > maxSize)
            {
                throw new AlertException("上传的文件不允许大于10M，请调整！");
            }

            var result = UploadImageNoSizeLimit(file);
            return result;
        }

        public string UploadImageNoSizeLimit(HttpPostedFileBase file)
        {
            if (file == null)
                throw new AlertException("上传错误，没找到要上传的图片！");
            var localFolder = "Upload/Images/";
            var baichuanBasePath = "upos/";

            var fileName = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(file.FileName + DateTime.Now.Ticks, "MD5");
            var fileType = file.FileName.Substring(file.FileName.LastIndexOf('.'));
            string result;

            var directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, localFolder);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            file.SaveAs(directory + fileName + fileType);
            result = WebUtils.HostUrl.TrimEnd('/') + "/" + localFolder + fileName + fileType;
            return result;
        }

        public void DeleteImage(string imageUrl)
        {
        }
    }
}
