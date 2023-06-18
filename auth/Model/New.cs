namespace auth.Model
{
    public class New
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; } = "https://bossluxurywatch.vn/uploads/tao/0-00/thumbs/418x0/2023-rolex-cosmograph-daytona-steel-ceramic-white-dial-126500ln-calibre-4131-hands-on-review-13-2048x1365.jpg";
        public string Content { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public User User { get; set; }
    }
}
