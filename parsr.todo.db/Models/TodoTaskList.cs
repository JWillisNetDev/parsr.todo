using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace parsr.todo.db.Models;

[Table("TodoTaskLists")]
[Index(nameof(PublicId), IsUnique = true, Name = "UK_TodoTaskLists_PublicId")]
public class TodoTaskList : IAuditModified
{
	internal class EntityConfiguration : IEntityTypeConfiguration<TodoTaskList>
	{
		public void Configure(EntityTypeBuilder<TodoTaskList> builder)
		{
			builder.Property(e => e.PublicId)
				.HasDefaultValueSql("NewId()");
		}
	}

	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int TodoTaskListId { get; set; }

	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public Guid PublicId { get; set; }

	[Required(AllowEmptyStrings = false)]
	[StringLength(128)]
	public required string Title { get; set; }

	[Required(AllowEmptyStrings = true)]
	[StringLength(4_000)]
	public required string Description { get; set; }

	public DateTime DateTimeCreatedUtc { get; set; }
	public DateTime DateTimeUpdatedUtc { get; set; }

	/* EF Generated References */
	public virtual ICollection<TodoTask> TodoTasks { get; set; } = new HashSet<TodoTask>();
}
