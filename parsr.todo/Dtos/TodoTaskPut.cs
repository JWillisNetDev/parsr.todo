namespace parsr.todo.Dtos;

public class TodoTaskPut
{
	public required string Title { get; set; }
	public required string Description { get; set; }
	public int Order { get; set; }
	public bool IsDone { get; set; }
}