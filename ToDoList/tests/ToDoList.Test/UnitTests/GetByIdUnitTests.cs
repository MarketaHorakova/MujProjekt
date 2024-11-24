using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi.Controllers;

namespace ToDoList.Test.UnitTests
{
    public class GetByIdUnitTests
    {
        // ### READBYID

        [Fact]
        public async Task Get_ReadByIdWhenSomeItemAvailable_ReturnsOk()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
            var item = new ToDoItem { ToDoItemId = 1, Name = "Test Item 1" };
            repositoryMock.ReadById(1).Returns(Task.FromResult<ToDoItem?>(item));

            var controller = new ToDoItemsController(repositoryMock);

            // Act
            var result = await controller.ReadByIdAsync(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<ToDoItemGetResponseDto>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("Test Item 1", returnValue.Name);
        }

        [Fact]
        public async Task Get_ReadByIdWhenItemIsNull_ReturnsNotFound()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
            repositoryMock.ReadById(1).Returns((ToDoItem)null);

            var controller = new ToDoItemsController(repositoryMock);

            // Act
            var result = await controller.ReadByIdAsync(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Get_ReadByIdUnhandledException_ReturnsInternalServerError()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
            repositoryMock.ReadById(1).Returns(Task.FromException<ToDoItem>(new Exception("Unhandled exception")));

            var controller = new ToDoItemsController(repositoryMock);

            // Act
            var result = await controller.ReadByIdAsync(1);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

        }

    }
}





