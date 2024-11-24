using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;
using ToDoList.Persistence;

namespace ToDoList.Persistence.Repositories;

public class ToDoItemsRepository : IRepositoryAsync<ToDoItem>
{
    private readonly ToDoItemsContext context;

    public ToDoItemsRepository(ToDoItemsContext context)
    {
        this.context = context;
    }

    public async Task Create(ToDoItem item)
    {
        await context.ToDoItems.AddAsync(item);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ToDoItem>> ReadAll()
    {
        return await context.ToDoItems.ToListAsync();

    }

    public async Task<ToDoItem> ReadById(int toDoItemId)
    {
        return await context.ToDoItems.FindAsync(toDoItemId);
    }

    public async Task Update(ToDoItem item)
    {
        context.ToDoItems.Update(item);
        await context.SaveChangesAsync();
    }

    public async Task DeleteById(ToDoItem item)
    {
        context.ToDoItems.Remove(item);
        await context.SaveChangesAsync();

    }
}
