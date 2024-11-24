using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.WebApi.Controllers;
using NSubstitute;
using Microsoft.AspNetCore.Http;
using ToDoList.Persistence.Repositories;
using FluentAssertions;
using Xunit;

namespace ToDoList.Test.UnitTests
{
    public class DeleteUnitTests
    {
        [Fact]
        public async Task Delete_DeleteByIdValidItemId_ReturnsNoContent()
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
                Category = "test"
            };
            repositoryMock.ReadById(toDoItem.ToDoItemId).Returns(Task.FromResult<ToDoItem?>(toDoItem));

            // Act
            var result = await controller.DeleteByIdAsync(toDoItem.ToDoItemId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            await repositoryMock.Received(1).DeleteById(toDoItem);
        }

        [Fact]
        public async Task Delete_DeleteByIdInvalidItemId_ReturnsNotFound()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
            var controller = new ToDoItemsController(repositoryMock);
            var invalidId = -1;

            repositoryMock.ReadById(invalidId).Returns((ToDoItem)null);

            // Act
            var result = await controller.DeleteByIdAsync(invalidId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            await repositoryMock.DidNotReceive().DeleteById(Arg.Any<ToDoItem>());
        }

        [Fact]
        public async Task Delete_DeleteByIdUnhandledExceptionInDeleteById_ReturnsInternalServerError()
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
                Category = "test"
            };

            repositoryMock.ReadById(toDoItem.ToDoItemId).Returns(Task.FromResult<ToDoItem?>(toDoItem));
            repositoryMock.When(r => r.DeleteById(toDoItem)).Do(x => throw new InvalidOperationException());

            // Act
            var result = await controller.DeleteByIdAsync(toDoItem.ToDoItemId);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task Delete_DeleteByIdUnhandledExceptionInReadById_ReturnsInternalServerError()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
            var controller = new ToDoItemsController(repositoryMock);
            var toDoItemId = 1;

            repositoryMock.When(r => r.ReadById(toDoItemId)).Do(x => throw new InvalidOperationException());

            // Act
            var result = await controller.DeleteByIdAsync(toDoItemId);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}







