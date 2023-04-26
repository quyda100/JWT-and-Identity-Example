using auth.Model;
using auth.Model.Request;
using Microsoft.AspNetCore.Identity;

namespace auth.Interfaces;

public interface ILogService
{
    public List<Log> getLogs();
    public void saveLog(Log log);
}