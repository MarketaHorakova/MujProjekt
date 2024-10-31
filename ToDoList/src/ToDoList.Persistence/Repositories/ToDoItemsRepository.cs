using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;
using ToDoList.Persistence;

namespace ToDoList.Persistence.Repositories;

public class ToDoItemsRepository : IRepository<ToDoItem>
{
    private readonly ToDoItemsContext context;

    public ToDoItemsRepository(ToDoItemsContext context)
    {
        this.context = context;
    }

    public void Create(ToDoItem item)
    {
        context.ToDoItems.Add(item);
        context.SaveChanges();
    }

    public IEnumerable<ToDoItem> GetAll()
    {
        return context.ToDoItems.ToList();

    }

    public ToDoItem GetById(int toDoItemId)
    {
        return context.ToDoItems.Find(toDoItemId);
    }

    public void Update(ToDoItem item)
    {
        context.ToDoItems.Update(item);
        context.SaveChanges();
    }

    public void Delete(ToDoItem item)
    {
        context.ToDoItems.Remove(item);
        context.SaveChanges();
    }
}
