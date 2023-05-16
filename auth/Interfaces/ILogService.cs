using auth.Model;

namespace auth.Interfaces
{
    public interface ILogService
    {
        public List<Log> GetLogs();
        public void SaveLog(string content);
    }
}