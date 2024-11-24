using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Persistence.Repositories;

// public interface IRepository<T> where T : class
// {
//     public void Create(T item); //T nahrazuje ToDoItem
//     public void Update(T item);
//     public void DeleteById(T item);

//     public T? ReadById(int toDoItemId);

//     public IEnumerable<T> ReadAll();

// }

public interface IRepositoryAsync<T> where T : class
{
    Task Create(T item); //T nahrazuje ToDoItem
    Task Update(T item);
    Task DeleteById(T item);

    Task<T?> ReadById(int toDoItemId);

    Task<IEnumerable<T>> ReadAll();

}
