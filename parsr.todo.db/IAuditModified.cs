namespace parsr.todo.db;

public interface IAuditModified
{
	DateTime DateTimeCreatedUtc { get; set; }
	DateTime DateTimeUpdatedUtc { get; set; }
}
