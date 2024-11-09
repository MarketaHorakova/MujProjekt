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
        public void Get_ReadByIdWhenSomeItemAvailable_ReturnsOk()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
            var item = new ToDoItem { ToDoItemId = 1, Name = "Test Item 1" };
            repositoryMock.GetById(1).Returns(item);

            var controller = new ToDoItemsController(repositoryMock);

            // Act
            var result = controller.ReadById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<ToDoItemGetResponseDto>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("Test Item 1", returnValue.Name);
        }

        [Fact]
        public void Get_ReadByIdWhenItemIsNull_ReturnsNotFound()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
            repositoryMock.GetById(1).Returns((ToDoItem)null);

            var controller = new ToDoItemsController(repositoryMock);

            // Act
            var result = controller.ReadById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Get_ReadByIdUnhandledException_ReturnsInternalServerError()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
            repositoryMock.GetById(1).Returns(x => { throw new Exception("Unhandled exception"); });

            var controller = new ToDoItemsController(repositoryMock);

            // Act
            var result = controller.ReadById(1);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

        }

    }
}





