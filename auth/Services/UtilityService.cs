using auth.Interfaces;
using System.Xml.Linq;

namespace auth.Services
{
    public class UtilityService : IUtilityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public UtilityService(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        public byte[] GetImage(string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName);
            Byte[] b = File.ReadAllBytes(path);
            return b;
        }

        public string UploadImage(IFormFile file, string prefix, string type)
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
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", type);
            var fileName = prefix + "-" + Guid.NewGuid().ToString() + fileExtension;
            var filePath = Path.Combine(folder, fileName);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            using (var stream = File.Create(filePath))
            {
                file.CopyTo(stream);
            }
            return FormatFileURL(fileName, type);
        }

        public List<string> UploadImages(List<IFormFile> files, string prefix, string type)
        {
            if (files.Count() == 0)
            {
                return null;
            }
            List<string> result = new();
            foreach (var file in files)
            {
                var fileName = UploadImage(file, prefix, type);
                result.Add(fileName);
            }
            return result;
        }
        public async Task<string> CallHttp()
        {
            string result = string.Empty;
            var request = new HttpRequestMessage(HttpMethod.Get, "https://dummyjson.com/todos");
            var client = _httpClientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            else
            {
                result = $"There is some errors : {response.ReasonPhrase}";
            }
            return result;
        }

        private string FormatFileURL(string fileName, string type)
        {
            var currentReq = _httpContextAccessor.HttpContext.Request;
            string baseURL = $"{currentReq.Scheme}://{currentReq.Host}/images/{type}/";
            return baseURL + fileName;
        }
    }
}
