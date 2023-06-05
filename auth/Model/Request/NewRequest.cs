using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace auth.Model.Request
{
    public class NewRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Thumbnail { get; set; }
        public string Content { get; set; }
    }
    public class NewUpdateRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Thumbnail { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
}