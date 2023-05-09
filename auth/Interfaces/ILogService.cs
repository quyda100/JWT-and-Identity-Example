using auth.Model;

namespace auth.Interfaces
{
    public interface ILogService
    {
        public List<Log> getLogs();
        public void saveLog(Log log);
    }
}