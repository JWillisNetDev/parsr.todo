using Microsoft.EntityFrameworkCore;
using parsr.todo.db.Models;
using System.Reflection;

namespace parsr.todo.db;

public class TodoDbContext : DbContext
{
	public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
	{
	}

	public DbSet<TodoTask> TodoTasks => Set<TodoTask>();

	public DbSet<TodoTaskList> TodoTaskLists => Set<TodoTaskList>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}

	public override int SaveChanges(bool acceptAllChangesOnSuccess)
	{
		var auditableEntries = ChangeTracker
			.Entries()
			.Where(e => e.Entity is IAuditModified && e.State is EntityState.Added or EntityState.Modified)
			.ToList();

		foreach (var entry in auditableEntries)
		{
			var auditable = (IAuditModified)entry.Entity;
			var utcNow = DateTime.UtcNow;
			auditable.DateTimeUpdatedUtc = utcNow;
			if (entry.State == EntityState.Added)
				auditable.DateTimeCreatedUtc = utcNow;
		}

		return base.SaveChanges(acceptAllChangesOnSuccess);
	}

	public override int SaveChanges()
		=> SaveChanges(acceptAllChangesOnSuccess: true);

	public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
	{
		var auditableEntries = ChangeTracker
			.Entries()
			.Where(e => e.Entity is IAuditModified && e.State is EntityState.Added or EntityState.Modified)
			.ToList();

		foreach (var entry in auditableEntries)
		{
			var auditable = (IAuditModified)entry.Entity;
			var utcNow = DateTime.UtcNow;
			auditable.DateTimeUpdatedUtc = utcNow;
			if (entry.State == EntityState.Added)
				auditable.DateTimeCreatedUtc = utcNow;
		}

		return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		=> SaveChangesAsync(acceptAllChangesOnSuccess: true, cancellationToken);
}
