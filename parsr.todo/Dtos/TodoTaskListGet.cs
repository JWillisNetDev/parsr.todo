namespace parsr.todo.Dtos;

public class TodoTaskListGet
{
	public Guid PublicId { get; set; }
	public required string Title { get; set; }
	public string? Description { get; set; }
	public DateTime DateTimeCreatedUtc { get; set; }
	public DateTime DateTimeUpdatedUtc { get; set; }
}
