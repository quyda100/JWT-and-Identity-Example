using auth.Data;
using auth.Interfaces;
using auth.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace auth.Services
{
    public class LogService : ILogService
    {
        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogService(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public List<Log> GetLogs()
        {
            var logs = _context.Logs.Include(l=>l.User).ToList();
            return logs;
        }

        public void SaveLog(string content)
        {
            var log = new Log
            {
                UserId = getUserId(),
                Action = content,

            };
            _context.Logs.Add(log);
            _context.SaveChanges();
        }
        private string getUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
