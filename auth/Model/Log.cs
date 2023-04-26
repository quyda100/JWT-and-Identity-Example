using System.ComponentModel.DataAnnotations.Schema;
namespace auth.Model;

public class Log
{
    public int Id { get; set; }
    public string UserId {get; set;}
    public string Action {get;set;}
    public DateTime TimeLog {get;set;} = DateTime.Now;
    public User User {get; set;}
}