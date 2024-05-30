using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
	public class BaseEntity
	{
		[Key]
        public int Id { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
