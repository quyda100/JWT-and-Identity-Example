using auth.Interfaces;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using System.Xml.Linq;
using MailKit.Security;

namespace auth.Services
{
    public class UtilityService : IUtilityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public UtilityService(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
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
        private string FormatFileURL(string fileName, string type)
        {
            var currentReq = _httpContextAccessor.HttpContext.Request;
            string baseURL = $"{currentReq.Scheme}://{currentReq.Host}/images/{type}/";
            return baseURL + fileName;
        }
        public async Task SendEmailAsync(string name, string email, string token)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_configuration["Mail:DisplayName"], _configuration["Mail:Email"]));
            message.To.Add(new MailboxAddress(name, email));
            message.Subject = "Đặt Lại Mật Khẩu";
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $"<h3>Chào: {name}</h3>" +
                $"<p>Bạn vui lòng truy cập vào đường dẫn:</p>" +
                $"<a href ='{token}' target='_blank'>{token}</a>" +
                $"<p>Hoặc nhấp <a href ='{token}' target='_blank'>vào đây</a> để đặt lại mật khẩu"
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_configuration["Mail:Host"], int.Parse(_configuration["Mail:Port"]), SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_configuration["Mail:Email"], _configuration["Mail:Password"]);
                await client.SendAsync(message);
            }
        }
    }
}
