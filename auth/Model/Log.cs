using System.ComponentModel.DataAnnotations.Schema;
namespace auth.Model;

class Log
{
    public int Id { get; set; }
    public int UserId {get; set;}
    public string Action {get;set;}
    public DateTime TimeLog {get;set;} = DateTime.Now;
    public User User {get; set;}
}