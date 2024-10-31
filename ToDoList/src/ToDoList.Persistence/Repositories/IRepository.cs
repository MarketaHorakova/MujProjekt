using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Persistence.Repositories;

public interface IRepository<T> where T : class
{
    public void Create(T item); //T nahrazuje ToDoItem
    public void Update(T item);
    public void Delete(T item);

    public T GetById(int toDoItemId);

    public IEnumerable<T> GetAll();

}
