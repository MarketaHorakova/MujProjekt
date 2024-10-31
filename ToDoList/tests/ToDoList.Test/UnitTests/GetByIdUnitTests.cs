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
        [Fact]
        public void GetById_ValidId_ReturnsItem()
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
            var result = controller.ReadById(toDoItem.ToDoItemId);
            var okResult = result.Result as OkObjectResult;
            var value = okResult?.Value as ToDoItemGetResponseDto;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(value);

            Assert.Equal(toDoItem.ToDoItemId, value.Id);
            Assert.Equal(toDoItem.Description, value.Description);
            Assert.Equal(toDoItem.IsCompleted, value.IsCompleted);
            Assert.Equal(toDoItem.Name, value.Name);
        }

        [Fact]
        public void GetById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
            var controller = new ToDoItemsController(repositoryMock);
            var invalidId = -1;

            repositoryMock.GetById(invalidId).Returns((ToDoItem)null);

            // Act

            var result = controller.ReadById(invalidId);
            var resultResult = result.Result;

            // Assert
            Assert.IsType<NotFoundResult>(resultResult);
        }

    }
}





