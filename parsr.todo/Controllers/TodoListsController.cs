using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using parsr.todo.Dtos;
using parsr.todo.db;
using parsr.todo.db.Models;

using System.Data;
using System.Threading.Tasks;

namespace parsr.todo.Controllers;

[ApiController]
[Route("api/lists")]
public class TodoListsController : Controller
{
	private readonly ILogger<TodoListsController> _logger;
	private readonly IDbContextFactory<TodoDbContext> _dbContextFactory;
	private readonly IMapper _mapper;

	public TodoListsController(ILogger<TodoListsController> logger, IDbContextFactory<TodoDbContext> dbContextFactory, IMapper mapper)
	{
		_logger = logger;
		_dbContextFactory = dbContextFactory;
		_mapper = mapper;
	}

	[HttpGet(Name = nameof(GetLists))]
	[ProducesResponseType(typeof(IEnumerable<TodoTaskListGet>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetLists()
	{
		using var context = await _dbContextFactory.CreateDbContextAsync();
		var lists = await context
			.TodoTaskLists
			.AsNoTracking()
			.ProjectTo<TodoTaskListGet>(_mapper.ConfigurationProvider)
			.ToArrayAsync();

		return Ok(lists);
	}

	[HttpPost(Name = nameof(PostList))]
	[ProducesResponseType(typeof(TodoTaskListGet), StatusCodes.Status201Created)]
	public async Task<IActionResult> PostList([FromBody] TodoTaskListCreate create)
	{
		using var context = await _dbContextFactory.CreateDbContextAsync();
		var newTodoTaskList = _mapper.Map<TodoTaskList>(create);

		context.TodoTaskLists.Add(newTodoTaskList);
		try
		{
			await context.SaveChangesAsync();
		}
		catch (Exception ex) when (ex is DbUpdateException or DbUpdateConcurrencyException)
		{
			_logger.LogError(exception: ex, message: "Failed to add to the Database.");
			throw;
		}
		return Created($"/api/lists/{newTodoTaskList.PublicId}", _mapper.Map<TodoTaskListGet>(newTodoTaskList));
	}

	[HttpGet("{listPubId:guid}", Name = nameof(GetList))]
	[ProducesResponseType(typeof(TodoTaskListGet), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(Guid), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetList([FromRoute] Guid listPubId)
	{
		using var context = await _dbContextFactory.CreateDbContextAsync();
		var list = await context
			.TodoTaskLists
			.AsNoTracking()
			.SingleOrDefaultAsync(ls => ls.PublicId == listPubId);

		if (list is null)
		{
			return NotFound(listPubId);
		}
		return Ok(_mapper.Map<TodoTaskListGet>(list));
	}

	[HttpPut("{listPubId:guid}", Name = nameof(PutList))]
	[ProducesResponseType(typeof(TodoTaskListGet), StatusCodes.Status200OK)]
	public async Task<IActionResult> PutList([FromRoute] Guid listPubId, [FromBody] TodoTaskListPut put)
	{
		using var context = await _dbContextFactory.CreateDbContextAsync();
		var list = await context
			.TodoTaskLists
			.SingleOrDefaultAsync(ls => ls.PublicId == listPubId);

		if (list is null)
		{
			return NotFound();
		}
				
		_mapper.Map(put, list);
		try
		{
			await context.SaveChangesAsync();
		}
		catch (Exception ex) when (ex is DbUpdateException or DbUpdateConcurrencyException)
		{
			_logger.LogError(exception: ex, message: "Failed to update the TodoTaskList Uid `{Uid}`", list.TodoTaskListId);
			throw;
		}
		return Ok(_mapper.Map<TodoTaskListGet>(list));
	}

	[HttpGet("{listPubId:guid}/tasks", Name = nameof(GetTasksFromList))]
	[ProducesResponseType(typeof(IEnumerable<TodoTaskGet>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetTasksFromList([FromRoute] Guid listPubId)
	{
		using var context = await _dbContextFactory.CreateDbContextAsync();
		var list = await context
			.TodoTaskLists
			.AsNoTracking()
			.Include(ls => ls.TodoTasks)
			.SingleOrDefaultAsync(ls => ls.PublicId == listPubId);
		
		if (list is null)
		{
			return NotFound();
		}
		return Ok(_mapper.Map<IEnumerable<TodoTaskGet>>(list.TodoTasks));
	}

	[HttpPost("{listPubId:guid}/tasks", Name = nameof(PostTaskToList))]
	[ProducesResponseType(typeof(TodoTaskGet), StatusCodes.Status201Created)]
	public async Task<IActionResult> PostTaskToList([FromRoute] Guid listPubId, [FromBody] TodoTaskCreate create)
	{
		using var context = await _dbContextFactory.CreateDbContextAsync();
		var list = await context
			.TodoTaskLists
			.SingleOrDefaultAsync(ls => ls.PublicId == listPubId);

		if (list is null)
		{
			return NotFound();
		}
		
		var newTodoTask = _mapper.Map<TodoTask>(create);
		list.TodoTasks.Add(newTodoTask);
		try
		{
			await context.SaveChangesAsync();
		}
		catch (Exception ex) when (ex is DbUpdateException or DbUpdateConcurrencyException)
		{
			_logger.LogError(exception: ex,
				message: "Failed to add to List `{PublicId}`", listPubId);
			throw;
		}
		return Created($"/api/tasks/{newTodoTask.PublicId}", _mapper.Map<TodoTaskGet>(newTodoTask));
	}
}
