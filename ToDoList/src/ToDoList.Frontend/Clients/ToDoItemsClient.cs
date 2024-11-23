using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Domain.DTOs;
using ToDoList.Frontend.Views;

namespace ToDoList.Frontend.Clients;
public class ToDoItemsClient(HttpClient httpClient) : IToDoItemsClient
{
    public async Task<List<ToDoItemView>> ReadItemsAsync()
    {
        var toDoItemsView = new List<ToDoItemView>();
        var response = await httpClient.GetFromJsonAsync<List<ToDoItemGetResponseDto>>("api/ToDoItems");
        toDoItemsView = response.Select(dto => new ToDoItemView
        {
            ToDoItemId = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            IsCompleted = dto.IsCompleted
        }).ToList();
        return toDoItemsView;
    }
}
