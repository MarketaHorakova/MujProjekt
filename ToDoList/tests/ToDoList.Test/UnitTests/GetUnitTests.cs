using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi.Controllers;

namespace ToDoList.Test.UnitTests
{
    public class GetUnitTests
    {
        [Fact]
        public void Get_AllItems_ReturnsAllItems()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
            var controller = new ToDoItemsController(repositoryMock);
            var toDoItems = new List<ToDoItem>
            {
                new ToDoItem
                {
                    ToDoItemId = 1,
                    Name = "Jmeno",
                    Description = "Popis",
                    IsCompleted = false
                }

            };
            repositoryMock.GetAll().Returns(toDoItems);

            // Act
            var result = controller.Read();
            var okResult = result.Result as OkObjectResult;
            var value = okResult?.Value as IEnumerable<ToDoItemGetResponseDto>;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(value);

            var firstItem = value.First();
            Assert.Equal(toDoItems[0].ToDoItemId, firstItem.Id);
            Assert.Equal(toDoItems[0].Description, firstItem.Description);
            Assert.Equal(toDoItems[0].IsCompleted, firstItem.IsCompleted);
            Assert.Equal(toDoItems[0].Name, firstItem.Name);
        }

        [Fact]
        public void Get_NoItems_ReturnsNotFound()
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
    }
}


