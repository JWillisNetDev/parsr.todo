using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using parsr.todo.db;
using parsr.todo.Dtos;
using System.Data;

namespace parsr.todo.Controllers;

[ApiController]
[Route("api/tasks")]
public class TodoTasksController : Controller
{
	private readonly ILogger<TodoListsController> _logger;
	private readonly IDbContextFactory<TodoDbContext> _dbContextFactory;
	private readonly IMapper _mapper;

	public TodoTasksController(ILogger<TodoListsController> logger, IDbContextFactory<TodoDbContext> dbContextFactory, IMapper mapper)
	{
		_logger = logger;
		_dbContextFactory = dbContextFactory;
		_mapper = mapper;
	}

	[HttpGet("{publicId:guid}", Name = nameof(GetTask))]
	[ProducesResponseType(typeof(TodoTaskGet), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetTask([FromRoute] Guid publicId)
	{
		using var context = await _dbContextFactory.CreateDbContextAsync();
		var task = await context
			.TodoTasks
			.SingleOrDefaultAsync(t => t.PublicId == publicId);
		
		if (task is null)
		{
			return NotFound();
		}
		return Ok(_mapper.Map<TodoTaskGet>(task));
	}

	[HttpPut("{publicId:guid}", Name = nameof(PutTask))]
	[ProducesResponseType(typeof(TodoTaskGet), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> PutTask([FromRoute] Guid publicId, [FromBody] TodoTaskPut put)
	{
		using var context = await _dbContextFactory.CreateDbContextAsync();
		var task = await context
			.TodoTasks
			.SingleOrDefaultAsync(t => t.PublicId == publicId);

		if (task is null)
		{
			return NotFound();
		}

		_mapper.Map(put, task);
		try
		{
			await context.SaveChangesAsync();
		}
		catch (Exception ex) when (ex is DbUpdateException or DbUpdateConcurrencyException)
		{
			_logger.LogError(exception: ex, message: "Failed to update the TodoTask Uid `{uid}`", task.TodoTaskId);
			throw;
		}
		return Ok(_mapper.Map<TodoTaskGet>(task));
	}

	[HttpPut("{publicId:guid}/done/{value:bool}", Name = nameof(SetTaskIsDone))]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> SetTaskIsDone([FromRoute] Guid publicId, [FromRoute] bool value)
	{
		using var context = await _dbContextFactory.CreateDbContextAsync();
		var task = await context
			.TodoTasks
			.SingleOrDefaultAsync(t => t.PublicId == publicId);

		if (task is null)
		{
			return NotFound();
		}

		task.IsDone = value;
		try
		{
			await context.SaveChangesAsync();
		}
		catch (Exception ex) when (ex is DbUpdateException or DbUpdateConcurrencyException)
		{
			_logger.LogError(exception: ex, message: "Failed to update the TodoTask Uid `{uid}` to `{value}`", task.TodoTaskId, value);
			throw;
		}
		return NoContent();
	}

	[HttpPut("{publicId:guid}/done/toggle", Name = nameof(ToggleTaskIsDone))]
	[ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> ToggleTaskIsDone([FromRoute] Guid publicId)
	{
		using var context = await _dbContextFactory.CreateDbContextAsync();
		var task = await context
			.TodoTasks
			.SingleOrDefaultAsync(t => t.PublicId == publicId);

		if (task is null)
		{
			return NotFound();
		}

		task.IsDone = !task.IsDone;
		try
		{
			await context.SaveChangesAsync();
		}
		catch (Exception ex) when (ex is DbUpdateException or DbUpdateConcurrencyException)
		{
			_logger.LogError(exception: ex, message: "Failed to update the TodoTask Uid `{uid}` to `{value}`", task.TodoTaskId, task.IsDone);
			throw;
		}
		return Ok(task.IsDone);
	}
}
