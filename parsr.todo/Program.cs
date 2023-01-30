using System.Reflection;
using Microsoft.EntityFrameworkCore;
using parsr.todo.db;

/* Big thank you to Alex Potter (@ajp_dev) for this finding and making this application much easier to scaffold out by providing
 *   instructions on how to use dev-server proxying for SPAs.
 * Read more: https://alexpotter.dev/net-6-with-vue-3/ */

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContextFactory<TodoDbContext>(options =>
{
	options.UseSqlServer(
		builder.Configuration.GetConnectionString("TodoDb.dev"),
		opts => opts.MigrationsAssembly("parsr.todo.migrations"));
});

/* Provide some JSON Serialization options for data interchange.
 *	Reference handler will prevent infinite reference loops
 *	Naming policy will ensure we use camelCase which is standard JSON convention. */

builder.Services.AddControllersWithViews()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
		options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
	});

builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(Assembly.GetCallingAssembly());

var app = builder.Build();

app.UseRouting();

/* We need to initialize swagger in-development prior to using the UseEndpoints method.
 *	Additionally, this block is used to apply migrations if that argument is passed in the development environment. */
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();

	if (args.Contains("-enable-runtime-migrations"))
	{
		using var scope = app.Services.CreateScope();
		var dbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<TodoDbContext>>();
		using var context = dbFactory.CreateDbContext();
		context.Database.Migrate();
	}
}

/* We have to define the endpoints using UseEndpoints prior to configuring the SpaDev server proxy
 *	to prevent redirecting api requests to the web application frontend for development
 *	(Credit to @ajp_dev) */
#pragma warning disable ASP0014 // Suggest using top level route registrations
app.UseEndpoints(endpoints =>
{
	endpoints.MapControllerRoute(
		name: "default",
		pattern: "{controller}/{action=Index}/{id?}");
	endpoints.MapSwagger();
});
#pragma warning restore ASP0014 // Suggest using top level route registrations

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	string viteHost = app.Configuration.GetValue<string?>("ViteConfig:server.host") ?? "http://localhost";
	int vitePort = app.Configuration.GetValue("ViteConfig:server.port", 0);
	
	app.UseSpa(spa =>
	{
		spa.UseProxyToSpaDevelopmentServer($"{viteHost}:{vitePort}");
	});
}
else
{
	app.MapFallbackToFile("index.html");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
	app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller}/{action=Index}/{id?}");

app.Run();
