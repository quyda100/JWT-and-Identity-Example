using auth.Model;
using auth.Model.DTO;

namespace auth.Interfaces
{
    public interface ILogService
    {
        public List<LogDTO> GetLogs();
        public void SaveLog(string content);
    }
}