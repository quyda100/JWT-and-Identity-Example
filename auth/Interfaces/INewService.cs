using auth.Model;

namespace auth.Interfaces
{
    public interface INewService
    {
        public Task<IEnumerable<New>> GetNews();
        public New GetNew(int id);
        public void AddNew(New model);
        public void UpdateNew(int id, New model);
        public void DeleteNew(int id);
    }
}
