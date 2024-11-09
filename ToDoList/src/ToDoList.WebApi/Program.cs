using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using ToDoList.Domain.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
{
    //Configure DI
    //WebApi services
    builder.Services.AddControllers(); // pridalo ToDoItemsController
    builder.Services.AddSwaggerGen();

    //Persistence services
    builder.Services.AddDbContext<ToDoItemsContext>(); // pridalo ToDoItemsContext
    builder.Services.AddScoped<IRepository<ToDoItem>, ToDoItemsRepository>(); // pridalo ToDoItemsRepository
}

var app = builder.Build();
{
    //Configure Middleware (HTTP request pipeline)
    app.MapControllers();
    app.UseSwagger();
    app.UseSwaggerUI(config => config.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDoList API V1"));
}
// http://localhost:5000/swagger/index.html
app.Run();
