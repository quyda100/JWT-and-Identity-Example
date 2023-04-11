using auth.Model;

namespace auth.Interfaces
{
    public interface INewService
    {
        public Task<IEnumerable<New>> getNews();
        public Task<IEnumerable<New>> getNew(int id);
        
        public void addNew(New model);
        public void updateNew(int id, New model);
        public void deleteNew(int id);
    }
}
