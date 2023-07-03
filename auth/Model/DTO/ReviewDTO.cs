namespace auth.Model.DTO
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}