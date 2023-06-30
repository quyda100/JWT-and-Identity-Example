using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using auth.Data;
using auth.Interfaces;
using auth.Model;

namespace auth.Services
{
    public class SupplierService : ISupplierService
    {
        private ApplicationDBContext _context;
        private ILogService _log;

        public SupplierService(ApplicationDBContext context, ILogService log)
        {
            _context = context;
            _log = log;
        }

        public  void Create(Supplier request)
        {
            if (_context.Suppliers.Any(s => s.Name == request.Name))
            {
                throw new Exception("Nhà cung cấp đã tồn tại");
            }
            _log.SaveLog("Tạo nhà cung cấp mới: " + request.Name);
            _context.Add(request);
            _context.SaveChanges();
        }

        public  void Delete(int id)
        {
            var supplier = Find(id);
            supplier.IsDeleted = true;
            _log.SaveLog("Xóa nhà cung cấp: " + supplier.Name);
            _context.Update(supplier);
            _context.SaveChanges();
        }

        public List<Supplier> Get()
        {
            return _context.Suppliers.Where(s=>s.IsDeleted == false).ToList();
        }

        public  void Update(Supplier request, int id)
        {
            if (request.Id != id)
            {
                throw new Exception("Có lỗi xảy ra");
            }
            var supplier = Find(id);
            if (request.Name != supplier.Name && _context.Suppliers.Any(s => s.Name == request.Name))
            {
                throw new Exception("Tên đã tồn tại");
            }
            supplier.Name = request.Name;
            supplier.Address = request.Address;
            supplier.UpdatedAt = DateTime.UtcNow.AddHours(7);
            _log.SaveLog("Cập nhật nhà cung cấp: " + request.Name);
            _context.Suppliers.Update(supplier);
            _context.SaveChanges();
        }

        private Supplier Find(int id)
        {
            var supplier = _context.Suppliers.FirstOrDefault(s => s.Id == id);
            return supplier ?? throw new Exception("Nhà cung cấp không tồn tại");
        }
    }
}