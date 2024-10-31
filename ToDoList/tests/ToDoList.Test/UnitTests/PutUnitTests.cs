using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi.Controllers;

namespace ToDoList.Test.UnitTests
{
    public class PutUnitTests
    {
        [Fact]
        public void Put_ValidId_ReturnsNoContent()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
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
                IsCompleted: true
            );

            repositoryMock.GetById(toDoItem.ToDoItemId).Returns(toDoItem);

            // Act
            var result = controller.UpdateById(toDoItem.ToDoItemId, request);

            // Assert
            Assert.IsType<NoContentResult>(result);
            repositoryMock.Received(1).Update(Arg.Is<ToDoItem>(item =>
               item.Name == request.Name &&
               item.Description == request.Description &&
               item.IsCompleted == request.IsCompleted));
        }

        [Fact]
        public void Put_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
            var controller = new ToDoItemsController(repositoryMock);
            var invalidId = -1;

            var request = new ToDoItemUpdateRequestDto(
                Name: "Jine jmeno",
                Description: "Jiny popis",
                IsCompleted: true
            );

            repositoryMock.GetById(invalidId).Returns((ToDoItem)null);

            // Act
            var result = controller.UpdateById(invalidId, request);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            repositoryMock.DidNotReceive().Update(Arg.Any<ToDoItem>());
        }

        [Fact]
        public void Put_UnhandledException_Returns500()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
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
                IsCompleted: true
            );

            repositoryMock.GetById(toDoItem.ToDoItemId).Returns(toDoItem);
            repositoryMock.When(r => r.Update(Arg.Any<ToDoItem>())).Do(x => throw new InvalidOperationException());

            // Act
            var result = controller.UpdateById(toDoItem.ToDoItemId, request);

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }
    }
}


