using auth.Data;
using auth.Interfaces;
using auth.Model;
using Microsoft.EntityFrameworkCore;

namespace auth.Services
{
    public class NewService : INewService
    {
        private readonly ApplicationDBContext _context;

        public NewService(ApplicationDBContext context) { 
            _context = context;
        }
        public void addNew(New model)
        {
            try
            {
                _context.News.Add(model);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
            }
        }

        public void deleteNew(int id)
        {
            var model = findNew(id);
            model.IsDeleted = true;
            _context.News.Update(model);
            _context.SaveChangesAsync();
        }

        public New getNew(int id)
        {
            return findNew(id);
        }

        public async Task<IEnumerable<New>> getNews()
        {
            var news = await _context.News.ToListAsync();
            return news;
        }

        public async void updateNew(int id, New model)
        {
            if (model.Id != id)
                throw new Exception("Having trouble");
            var item = await _context.News.SingleOrDefaultAsync(x=>x.Id == id);
            model.UpdatedAt = DateTime.Now;
            _context.News.Update(model);
            await _context.SaveChangesAsync();
        }
        private New findNew(int id)
        {
            var item = _context.News.SingleOrDefault(p => p.Id == id);
            if (item == null)
            {
                throw new Exception("New not found");
            }
            return item;
        }
    }
}
