namespace WebApplication1.Models
{
    public class User: BaseEntity
    {
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
