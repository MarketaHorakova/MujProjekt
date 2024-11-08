using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi.Controllers;

namespace ToDoList.Test.UnitTests
{
    // READ
    public class GetUnitTests
    {
        [Fact]
        public void Get_ReadWhenSomeItemAvailable_ReturnsOk()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
            var items = new List<ToDoItem>
            {
               new ToDoItem { ToDoItemId = 1, Name = "Test Item 1" },
               new ToDoItem { ToDoItemId = 2, Name = "Test Item 2" }
            };
            repositoryMock.GetAll().Returns(items);

            var controller = new ToDoItemsController(repositoryMock);

            // Act
            var result = controller.Read();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<ToDoItemGetResponseDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
            Assert.Contains(returnValue, item => item.Id == 1 && item.Name == "Test Item 1");
            Assert.Contains(returnValue, item => item.Id == 2 && item.Name == "Test Item 2");
        }

        [Fact]
        public void Get_ReadWhenNoItemAvailable_ReturnsNotFound()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
            var controller = new ToDoItemsController(repositoryMock);

            // Act
            var result = controller.Read();
            var resultResult = result.Result;

            // Assert
            Assert.IsType<NotFoundResult>(resultResult);
        }

        [Fact]
        public void Get_ReadUnhandledException_ReturnsInternalServerError()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
            repositoryMock.GetAll().Returns(x => { throw new Exception("Unhandled exception"); });

            var controller = new ToDoItemsController(repositoryMock);

            // Act
            var result = controller.Read();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

            // var problemDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
            // Assert.Equal("Unhandled exception", problemDetails.Detail);
            // Assert.Equal(StatusCodes.Status500InternalServerError, problemDetails.Status);
        }
    }
}


