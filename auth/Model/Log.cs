using System.ComponentModel.DataAnnotations.Schema;

namespace auth.Model
{
    public class Log
    {
        public int Id { get; set; }
        public string UserId {get; set;}
        public string Action {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.UtcNow.AddHours(7);
        public User User {get; set;}
    }
}
