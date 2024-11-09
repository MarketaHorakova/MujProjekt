using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi.Controllers;
using FluentAssertions;

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
            repositoryMock.ReadAll().Returns(items);

            var controller = new ToDoItemsController(repositoryMock);

            // Act
            var result = controller.Read();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            var returnValue = okResult.Value as IEnumerable<ToDoItemGetResponseDto>;
            returnValue.Should().NotBeNull();
            returnValue.Should().HaveCount(2);
            returnValue.Should().Contain(item => item.Id == 1 && item.Name == "Test Item 1");
            returnValue.Should().Contain(item => item.Id == 2 && item.Name == "Test Item 2");
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
            //Assert.IsType<NotFoundResult>(resultResult);
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Get_ReadUnhandledException_ReturnsInternalServerError()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
            repositoryMock.ReadAll().Returns(x => { throw new Exception("Unhandled exception"); });

            var controller = new ToDoItemsController(repositoryMock);

            // Act
            var result = controller.Read();

            // Assert
            var objectResult = result.Result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            // var problemDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
            // Assert.Equal("Unhandled exception", problemDetails.Detail);
            // Assert.Equal(StatusCodes.Status500InternalServerError, problemDetails.Status);
        }
    }
}


