using auth.Data;
using auth.Interfaces;
using auth.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace auth.Services
{
    public class NewService : INewService
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogService _log;
        private readonly HttpContextAccessor _httpContextAccessor;

        public NewService(ApplicationDBContext context, ILogService log, HttpContextAccessor httpContextAccessor) { 
            _context = context;
            _log = log;
            _httpContextAccessor = httpContextAccessor;
        }
        public void AddNew(New model)
        {
            model.UserId = GetUserId();
            _log.SaveLog("Tạo bài viết mới: " + model.Title);
                _context.News.Add(model);
                _context.SaveChanges();

        }

        public void DeleteNew(int id)
        {
            var model = findNew(id);
            model.IsDeleted = true;
            _log.SaveLog("Xóa bài viết: " + model.Title);
            _context.News.Update(model);
            _context.SaveChangesAsync();
        }

        public New GetNew(int id)
        {
            return findNew(id);
        }

        public async Task<IEnumerable<New>> GetNews()
        {
            var news = await _context.News.ToListAsync();
            return news;
        }

        public async void UpdateNew(int id, New model)
        {
            if (model.Id != id)
                throw new Exception("Having trouble");
            var item = await _context.News.SingleOrDefaultAsync(x=>x.Id == id);
            model.UpdatedAt = DateTime.Now;
            _context.News.Update(model);
            _log.SaveLog("Cập nhât bài viết: " + item.Title);
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
        private string GetUserId() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
