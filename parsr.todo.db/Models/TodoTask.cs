using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace parsr.todo.db.Models;

[Table("TodoTasks")]
[Index(nameof(PublicId), IsUnique = true, Name = "UK_TodoTasks_PublicId")]
public class TodoTask : IAuditModified
{
	internal class EntityConfiguration : IEntityTypeConfiguration<TodoTask>
	{
		public void Configure(EntityTypeBuilder<TodoTask> builder)
		{
			builder.Property(e => e.PublicId)
				.HasDefaultValueSql("NewId()");
		}
	}

	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int TodoTaskId { get; set; }

	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public Guid PublicId { get; set; }

	[Required(AllowEmptyStrings = false)]
	[StringLength(72)]
	public required string Title { get; set; }
	
	[Required(AllowEmptyStrings = true)]
	[StringLength(4_000)]
	public required string Description { get; set; }

	public int Order { get; set; }

	public bool IsDone { get; set; } = false;

	public int TodoTaskListId { get; set; } = 0;

	[ForeignKey(nameof(TodoTaskListId))]
	public virtual TodoTaskList? TodoTaskList { get; set; } = null;

	public DateTime DateTimeCreatedUtc { get; set; }

	public DateTime DateTimeUpdatedUtc { get; set; }
}
