namespace parsr.todo.Dtos;

public sealed record class TodoTaskListCreate
{
	public required string Title { get; set; }
	public string? Description { get; set; }
}