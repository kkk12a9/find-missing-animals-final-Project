using System;

namespace Domain
{
	public class Comment
	{
		public int Id { get; set; }
		public string Text { get; set; }
		public string ApplicationUserId { get; set; }
		public ApplicationUser ApplicationUser { get; set; }
		public Guid PostId { get; set; }
		public Post Post { get; set; }
		public DateTime Timestamp { get; set; } = DateTime.UtcNow;
	}
}
