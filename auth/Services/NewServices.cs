using auth.Data;
using auth.Interfaces;
using auth.Model;

namespace auth.Services
{
    public class NewServices : INewService
    {
        private readonly ApplicationDBContext _context;

        public NewServices(ApplicationDBContext context) { 
            _context = context;
        }
        public void addNew(New model)
        {
            
        }

        public void deleteNew(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<New>> getNew(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<New>> getNews()
        {
            var news = _context.New
        }

        public void updateNew(int id, New model)
        {
            throw new NotImplementedException();
        }
    }
}
