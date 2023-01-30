namespace parsr.todo.Dtos;

public sealed record class TodoTaskCreate
{
	public required string Title { get; set; }
	public string? Description { get; set; }
	public int Order { get; set; }
}
