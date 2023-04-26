using auth.Data;
using auth.Interfaces;
using auth.Model;
using Microsoft.EntityFrameworkCore;

namespace auth.Services;

public class LogService : ILogService
{
    private readonly ApplicationDBContext _context;
    LogService(ApplicationDBContext context){
        _context = context;
    }
    public List<Log> getLogs()
    {
        var logs = _context.Logs.ToList();
        return logs;
    }

    public void saveLog(Log log)
    {
        _context.Logs.Add(log);
        _context.SaveChanges();
    }
}