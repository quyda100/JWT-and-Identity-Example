using auth.Data;
using auth.Interfaces;
using auth.Model;
using auth.Model.DTO;
using auth.Model.Request;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace auth.Services
{
    public class NewService : INewService
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogService _log;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IUtilityService _utility;

        public NewService(ApplicationDBContext context, ILogService log, IHttpContextAccessor httpContextAccessor, IMapper mapper, IUtilityService utility)
        {
            _context = context;
            _log = log;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _utility = utility;
        }
        public void AddNew(NewRequest model)
        {
            var post = new New
            {
                Title = model.Title,
                Description = model.Description,
                Content = model.Content,
                UserId = GetUserId()
            };
            _log.SaveLog("Tạo bài viết mới: " + model.Title);
            _context.News.Add(post);
            var thumbnail = _utility.UploadImage(model.Thumbnail, $"{post.Id}", "Posts");
            post.Thumbnail = thumbnail;
            _context.News.Update(post);
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

        public NewDTO GetNew(int id)
        {
            var news = findNew(id);
            return new NewDTO
            {
                Id = news.Id,
                Title = news.Title,
                Thumbnail = news.Thumbnail,
                Description = news.Description,
                Content = news.Content,
                CreatedAt = news.CreatedAt,
                IsDeleted = news.IsDeleted,
                UserName = news.User.FullName
            };
        }

        public List<NewDTO> GetNews()
        {
            var news = _context.News.Include(n => n.User).Select(n => _mapper.Map<NewDTO>(n)).ToList();
            return news;
        }

        public async void UpdateNew(int id, NewUpdateRequest model)
        {
            if (model.Id != id)
                throw new Exception("Having trouble");
            var item = findNew(id);
            if (model.Thumbnail != null)
            {
                item.Thumbnail = _utility.UploadImage(model.Thumbnail, $"{item.Id}", "Posts");
            }
            item.Title = model.Title;
            item.Description = model.Description;
            item.IsDeleted = model.IsDeleted;
            item.Content = model.Content;
            item.UpdatedAt = DateTime.Now;
            _context.News.Update(item);
            _log.SaveLog("Cập nhật bài viết: " + item.Title);
            await _context.SaveChangesAsync();
        }
        private New findNew(int id)
        {
            var item = _context.News.Include(n => n.User).SingleOrDefault(p => p.Id == id);
            if (item == null)
            {
                throw new Exception("Không tìm thấy bài viết");
            }
            return item;
        }
        public List<NewDTO> GetViewPosts()
        {
            var posts = _context.News.Where(p => p.IsDeleted == false).OrderByDescending(p => p.CreatedAt).Select(p => _mapper.Map<NewDTO>(p)).ToList();
            return posts;
        }
        public List<NewDTO> GetNewestPosts()
        {
            var posts = _context.News.Where(p => p.IsDeleted == false).OrderByDescending(p => p.CreatedAt).Take(3).Select(p => _mapper.Map<NewDTO>(p)).ToList();
            return posts;
        }

        private string GetUserId() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
