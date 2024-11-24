using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi.Controllers;
using Xunit;

namespace ToDoList.Test.UnitTests
{
    public class PutUnitTests
    {
        [Fact]
        public async Task Put_UpdateByIdWhenItemUpdated_ReturnsNoContent()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
            var controller = new ToDoItemsController(repositoryMock);
            var toDoItem = new ToDoItem
            {
                ToDoItemId = 1,
                Name = "Jmeno",
                Description = "Popis",
                IsCompleted = false,
                Category = "old category"
            };

            var request = new ToDoItemUpdateRequestDto(
                Name: "Jine jmeno",
                Description: "Jiny popis",
                IsCompleted: true,
                Category: "new category"
            );

            repositoryMock.ReadById(toDoItem.ToDoItemId).Returns(Task.FromResult<ToDoItem?>(toDoItem));

            // Act
            var result = await controller.UpdateByIdAsync(toDoItem.ToDoItemId, request);

            // Assert
            Assert.IsType<NoContentResult>(result);
            await repositoryMock.Received(1).Update(Arg.Is<ToDoItem>(item =>
                item.ToDoItemId == toDoItem.ToDoItemId &&
                item.Name == request.Name &&
                item.Description == request.Description &&
                item.IsCompleted == request.IsCompleted &&
                item.Category == request.Category));
        }

        [Fact]
        public async Task Put_UpdateByIdWhenIdNotFound_ReturnsNotFound()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
            var controller = new ToDoItemsController(repositoryMock);
            var invalidId = -1;

            var request = new ToDoItemUpdateRequestDto(
                Name: "Jine jmeno",
                Description: "Jiny popis",
                IsCompleted: true,
                Category: "Work"
            );

            repositoryMock.ReadById(invalidId).Returns(Task.FromResult<ToDoItem?>(null));

            // Act
            var result = await controller.UpdateByIdAsync(invalidId, request);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            await repositoryMock.DidNotReceive().Update(Arg.Any<ToDoItem>());
        }

        [Fact]
        public async Task Put_UpdateByIdUnhandledException_ReturnsInternalServerError()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
            var controller = new ToDoItemsController(repositoryMock);
            var toDoItem = new ToDoItem
            {
                ToDoItemId = 1,
                Name = "Jmeno",
                Description = "Popis",
                IsCompleted = false
            };

            var request = new ToDoItemUpdateRequestDto(
                Name: "Jine jmeno",
                Description: "Jiny popis",
                IsCompleted: true,
                Category: "new category"
            );

            repositoryMock.ReadById(toDoItem.ToDoItemId).Returns(Task.FromResult<ToDoItem?>(toDoItem));
            repositoryMock.When(r => r.Update(Arg.Any<ToDoItem>())).Do(x => throw new InvalidOperationException());

            // Act
            var result = await controller.UpdateByIdAsync(toDoItem.ToDoItemId, request);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }
    }
}
