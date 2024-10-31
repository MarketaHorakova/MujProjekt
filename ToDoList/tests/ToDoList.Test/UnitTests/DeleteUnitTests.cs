using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.WebApi.Controllers;
using NSubstitute;
using Microsoft.AspNetCore.Http;
using ToDoList.Persistence.Repositories;

namespace ToDoList.Test.UnitTests
{
    public class DeleteUnitTests
    {
        [Fact]
        public void Delete_ValidId_ReturnsNoContent()
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
            repositoryMock.GetById(toDoItem.ToDoItemId).Returns(toDoItem);

            // Act
            var result = controller.DeleteById(toDoItem.ToDoItemId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            repositoryMock.Received(1).Delete(toDoItem);
        }

        [Fact]
        public void Delete_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
            var controller = new ToDoItemsController(repositoryMock);
            var invalidId = -1;

            repositoryMock.GetById(invalidId).Returns((ToDoItem)null);

            // Act
            var result = controller.DeleteById(invalidId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            repositoryMock.DidNotReceive().Delete(Arg.Any<ToDoItem>());
        }

        [Fact]
        public void Delete_UnhandledException_Returns500()
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

            repositoryMock.GetById(toDoItem.ToDoItemId).Returns(toDoItem);
            repositoryMock.When(r => r.Delete(toDoItem)).Do(x => throw new InvalidOperationException());

            // Act
            var result = controller.DeleteById(toDoItem.ToDoItemId);

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }
    }
}







