using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using auth.Model;

namespace auth.Interfaces
{
    public interface ISupplierService
    {
        List<Supplier> Get();
        void Create(Supplier request);
        void Update(Supplier request, int id);
        void Delete(int id);

    }
}