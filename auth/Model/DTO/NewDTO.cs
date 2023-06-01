using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace auth.Model.DTO
{
    public class NewDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
}