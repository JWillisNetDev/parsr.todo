namespace parsr.todo.Dtos;

public sealed record class TodoTaskGet
{
	public Guid PublicId { get; set; }
	public int Order { get; set; }
	public required string Title { get; set; }
	public string? Description { get; set; }
	public bool IsDone { get; set; }
	public DateTime DateTimeCreatedUtc { get; set; }
	public DateTime DateTimeUpdatedUtc { get; set; }
}
