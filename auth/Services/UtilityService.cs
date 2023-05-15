using auth.Interfaces;
using System.Xml.Linq;

namespace auth.Services
{
    public class UtilityService : IUtilityService
    {
        public byte[] GetImage(string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName);
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
            var fileName = Path.Combine("Products", prefix, Guid.NewGuid().ToString() + fileExtension);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName);
            using (var stream = File.Create(filePath))
            {
                file.CopyTo(stream);
            }
            return fileName;
        }

        public List<string> UploadImages(List<IFormFile> files, string prefix)
        {
            if (files.Count > 0)
            {
                return null;
            }
            var acceptExtension = new List<string> { ".jpg", ".jpeg", ".png", ".gif" };
            List<string> result = new List<string>();
            foreach (var file in files)
            {
                if (file.Length > 10485760)
                {
                    throw new Exception("Vui lòng tải lên tập tin < 10MB");
                }
                var fileExtension = Path.GetExtension(file.FileName);
                if (!acceptExtension.Contains(fileExtension))
                {
                    throw new Exception("Vui lòng tải lên đúng định dạng");
                }
                var fileName = Path.Combine("Products", prefix, Guid.NewGuid().ToString() + fileExtension);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName);
                using (var stream = File.Create(filePath))
                {
                    file.CopyTo(stream);
                }
                result.Add(fileName);
            }
            return result;
        }
    }
}
