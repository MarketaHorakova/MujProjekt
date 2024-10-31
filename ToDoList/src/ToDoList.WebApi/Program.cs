using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using ToDoList.Domain.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
{
    //Configure DI
    builder.Services.AddControllers();
    builder.Services.AddDbContext<ToDoItemsContext>(); //options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    builder.Services.AddScoped<IRepository<ToDoItem>, ToDoItemsRepository>();
}

var app = builder.Build();
{
    //Configure Middleware (HTTP request pipeline)
    app.MapControllers();
}

app.Run();
