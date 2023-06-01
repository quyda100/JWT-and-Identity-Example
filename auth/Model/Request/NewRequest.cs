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
        public string Content { get; set; }
    }
}