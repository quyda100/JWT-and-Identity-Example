using auth.Data;
using auth.Interfaces;
using auth.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using auth.Model.DTO;
using AutoMapper;

namespace auth.Services
{
    public class LogService : ILogService
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogService(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public List<LogDTO> GetLogs()
        {
            var logs = _context.Logs.Where(l => l.CreatedAt < DateTime.UtcNow.AddHours(7) && l.CreatedAt > DateTime.UtcNow.AddMonths(-1))
                .OrderByDescending(l => l.CreatedAt)
                .Include(l => l.User)
                .Select(l => _mapper.Map<LogDTO>(l))
                .ToList();
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
