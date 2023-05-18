using auth.Interfaces;
using System.Xml.Linq;

namespace auth.Services
{
    public class UtilityService : IUtilityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UtilityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public byte[] GetImage(string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(),"Uploads", fileName);
            Byte[] b = File.ReadAllBytes(path);
            return b;
        }

        public string UploadImage(IFormFile file, string prefix)
        {
            if (file == null)
            {
                return null;
            }
            var acceptExtension = new List<string> { ".jpg", ".jpeg", ".png", ".gif" };
            if (file.Length > 10485760)
            {
                throw new Exception("Vui lòng tải lên tập tin < 10MB");
            }
            var fileExtension = Path.GetExtension(file.FileName);
            if (!acceptExtension.Contains(fileExtension))
            {
                throw new Exception("Vui lòng tải lên đúng định dạng");
            }
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Products");
            var fileName = prefix + "_" + Guid.NewGuid().ToString() + fileExtension;
            var filePath = Path.Combine(folder, fileName);
            
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            using (var stream = File.Create(filePath))
            {
                file.CopyTo(stream);
            }
            return FormatFileURL(fileName);
        }

        public List<string> UploadImages(List<IFormFile> files, string prefix)
        {
            if (files.Count == 0)
            {
                return null;
            }
            List<string> result = new();
            foreach (var file in files)
            {
                var fileName = UploadImage(file, prefix);
                result.Add(fileName);
            }
            return result;
        }

        private string FormatFileURL(string fileName)
        {
            var currentReq = _httpContextAccessor.HttpContext.Request;
            string baseURL = $"{currentReq.Scheme}://{currentReq.Host}/images/products/";
            return baseURL + fileName;
        }
    }
}
