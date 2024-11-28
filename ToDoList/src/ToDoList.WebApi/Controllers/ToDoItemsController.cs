namespace ToDoList.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using System.Collections.Generic;
using System.Linq;
using System;
using ToDoList.Persistence.Repositories;

[ApiController]
[Route("api/[controller]")]
public class ToDoItemsController : ControllerBase
{
    private readonly IRepositoryAsync<ToDoItem> repository;

    public ToDoItemsController(IRepositoryAsync<ToDoItem> repository)
    {
        this.repository = repository;
    }

    [HttpPost]
    public async Task<ActionResult<ToDoItemGetResponseDto>> CreateAsync(ToDoItemCreateRequestDto request)
    {
        //map to Domain object as soon as possible
        var item = request.ToDomain();

        //try to create an item
        try
        {
            await repository.Create(item);

        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }

        //respond to client
        return CreatedAtAction(
            nameof(ReadByIdAsync),
            new { toDoItemId = item.ToDoItemId },
            ToDoItemGetResponseDto.FromDomain(item)); //201
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ToDoItemGetResponseDto>>> ReadAsync()
    {
        IEnumerable<ToDoItem> itemsToGet;
        try
        {
            itemsToGet = await repository.ReadAll();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }

        //respond to client
        return (itemsToGet is null || !itemsToGet.Any())
            ? NotFound() //404
            : Ok(itemsToGet.Select(ToDoItemGetResponseDto.FromDomain)); //200
    }

    [HttpGet("{toDoItemId:int}")]
    public async Task<ActionResult<ToDoItemGetResponseDto>> ReadByIdAsync(int toDoItemId)
    {
        //try to retrieve the item by id
        ToDoItem? itemToGet;
        try
        {
            itemToGet = await repository.ReadById(toDoItemId);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }

        //respond to client
        return (itemToGet is null)
            ? NotFound() //404
            : Ok(ToDoItemGetResponseDto.FromDomain(itemToGet)); //200
    }

    [HttpPut("{toDoItemId:int}")]
    public async Task<IActionResult> UpdateByIdAsync(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        //map to Domain object as soon as possible
        var updatedItem = request.ToDomain();
        updatedItem.ToDoItemId = toDoItemId;

        //try to update the item by retrieving it with given id
        try
        {
            //retrieve the item
            var itemToUpdate = await repository.ReadById(toDoItemId);
            if (itemToUpdate == null)
            {
                return NotFound(); //404
            }

            // Update the properties of the entity that is already being tracked
            itemToUpdate.Name = updatedItem.Name;
            itemToUpdate.Description = updatedItem.Description;
            itemToUpdate.IsCompleted = updatedItem.IsCompleted;
            itemToUpdate.Category = updatedItem.Category;

            await repository.Update(itemToUpdate);

        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }

        //respond to client
        return NoContent(); //204
    }

    [HttpDelete("{toDoItemId:int}")]
    public async Task<IActionResult> DeleteByIdAsync(int toDoItemId)
    {
        //try to delete the item
        try
        {
            var itemToDelete = await repository.ReadById(toDoItemId);
            if (itemToDelete is null)
            {
                return NotFound(); //404
            }
            await repository.DeleteById(itemToDelete);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

        //respond to client
        return NoContent(); //204
    }
}
