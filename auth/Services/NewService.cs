﻿using auth.Data;
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

        public NewService(ApplicationDBContext context, ILogService log, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _log = log;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
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

        public List<NewDTO> GetNews()
        {
            var news = _context.News.Include(n => n.User).Select(n => _mapper.Map<NewDTO>(n)).ToList();
            return news;
        }

        public async void UpdateNew(int id, NewDTO model)
        {
            if (model.Id != id)
                throw new Exception("Having trouble");
            var item = findNew(id);
            item.Title = model.Title;
            item.Description = model.Description;
            item.Content = model.Content;
            item.UpdatedAt = DateTime.Now;
            _context.News.Update(item);
            _log.SaveLog("Cập nhật bài viết: " + item.Title);
            await _context.SaveChangesAsync();
        }
        private New findNew(int id)
        {
            var item = _context.News.SingleOrDefault(p => p.Id == id);
            if (item == null)
            {
                throw new Exception("Không tìm thấy bài viết");
            }
            return item;
        }
        private string GetUserId() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
