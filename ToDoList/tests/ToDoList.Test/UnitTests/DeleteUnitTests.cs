using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.WebApi.Controllers;
using NSubstitute;
using Microsoft.AspNetCore.Http;
using ToDoList.Persistence.Repositories;
using FluentAssertions;

namespace ToDoList.Test.UnitTests
{
    public class DeleteUnitTests
    {
        //  DELETEBYID

        // Delete_DeleteByIdValidItemId_ReturnsNoContent()
        // Delete_DeleteByIdInvalidItemId_ReturnsNotFound()

        // Delete_DeleteByIdUnhandledException_ReturnsInternalServerError()
        // (pokud používáte repository.ReadById() následované repository.UpdateById(), je potřeba vytvořit dva testy, jeden za každou metodu, kterou namockujeme tak, aby vyhazovala výjimku)

        [Fact]
        public void Delete_DeleteByIdValidItemId_ReturnsNoContent()
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
            repositoryMock.ReadById(toDoItem.ToDoItemId).Returns(toDoItem);

            // Act
            var result = controller.DeleteById(toDoItem.ToDoItemId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            repositoryMock.Received(1).DeleteById(toDoItem);
        }

        [Fact]
        public void Delete_DeleteByIdInvalidItemId_ReturnsNotFound()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
            var controller = new ToDoItemsController(repositoryMock);
            var invalidId = -1;

            repositoryMock.ReadById(invalidId).Returns((ToDoItem)null);

            // Act
            var result = controller.DeleteById(invalidId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            repositoryMock.DidNotReceive().DeleteById(Arg.Any<ToDoItem>());
        }

        [Fact]
        public void Delete_DeleteByIdUnhandledExceptionInDeleteById_ReturnsInternalServerError()
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

            repositoryMock.ReadById(toDoItem.ToDoItemId).Returns(toDoItem);
            repositoryMock.When(r => r.DeleteById(toDoItem)).Do(x => throw new InvalidOperationException());

            // Act
            var result = controller.DeleteById(toDoItem.ToDoItemId);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public void Delete_DeleteByIdUnhandledExceptionInReadById_ReturnsInternalServerError()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
            var controller = new ToDoItemsController(repositoryMock);
            var toDoItemId = 1;

            repositoryMock.When(r => r.ReadById(toDoItemId)).Do(x => throw new InvalidOperationException());

            // Act
            var result = controller.DeleteById(toDoItemId);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}







