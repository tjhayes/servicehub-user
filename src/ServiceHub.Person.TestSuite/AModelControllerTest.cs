using ServiceHub.Person.Library.Abstracts;
using ServiceHub.Person.Library.Interfaces;
using ServiceHub.Person.Service.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using PCM = ServiceHub.Person.Context.Models;

namespace ServiceHub.Person.TestSuite
{
    public class AModelControllerTest
    {
        [Fact]
        public void TestGetAll()
        {
            var mockRepo = new Mock<IRepository<IModel>>();
            var apartmentList = new List<IModel>();

            var mockModel = new Mock<IModel>();
            mockModel.Setup(m => m.ModelId).Returns("5a74ca174bbfc73ea03e8bb2");

            apartmentList.Add(mockModel.Object);

            mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(apartmentList);

            var controller = new AModelController<IModel>(mockRepo.Object);
            IEnumerable<IModel> result = controller.Get().Result;
            IEnumerator<IModel> enumerator = result.GetEnumerator();
            enumerator.MoveNext();

            Assert.Equal(mockModel.Object.ModelId, enumerator.Current.ModelId);
        }

        [Fact]
        public void TestGetById()
        {
            var mockRepo = new Mock<IRepository<IModel>>();
            var newModelId = "5a74ca174bbfc73ea03e8bb2";
            var mockModel = new Mock<IModel>();
            mockModel.Setup(m => m.ModelId).Returns("5a74ca174bbfc73ea03e8bb2");

            mockRepo.Setup(repo => repo.GetById(newModelId)).ReturnsAsync(mockModel.Object);

            var controller = new AModelController<IModel>(mockRepo.Object);
            IActionResult result = controller.Get(newModelId).Result;

            OkObjectResult objectResult = result as OkObjectResult;
            var val = objectResult.Value as IModel;
            Assert.Equal(mockModel.Object.ModelId, val.ModelId);
        }

        [Fact]
        public void Create_DoesNotThrowException()
        {
            var mockRepo = new Mock<IRepository<AModel>>();
            var testPerson = new PCM.Person();
            mockRepo.Setup(repo => repo.Create(testPerson));
            var controller = new AModelController<AModel>(mockRepo.Object);

            int expected = StatusCodes.Status201Created;
            int? actual = controller.CreatedAtAction("Post", testPerson).StatusCode;

            // todo: Verify that this actually happened.
            Assert.True(actual == expected);
        }

        [Fact]
        public void Controller_Update_Should_Work_With_Valid_Id()
        {
            // Arrange
            var mockRepo = new Mock<IRepository<IModel>>();
            var newModelId = "aaaaaaaaaaaaaaaaaaaaaaaa";
            var mockModel = new Mock<IModel>();
            mockModel.Setup(model => model.ModelId).Returns(newModelId);

            mockRepo.Setup(repo => repo.UpdateById(mockModel.Object.ModelId, mockModel.Object)).ReturnsAsync(true);
            var controller = new AModelController<IModel>(mockRepo.Object);

            // Act
            var expected = new NoContentResult();
            IActionResult result = controller.Put(newModelId, mockModel.Object).Result;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.ToString(), result.ToString());
        }

        [Fact]
        public void Controller_Update_Should_Not_Work_With_Invalid_Id()
        {
            // Arrange
            var mockRepo = new Mock<IRepository<IModel>>();
            var newModelId = "This is a bad ID!";
            var mockModel = new Mock<IModel>();
            mockModel.Setup(model => model.ModelId).Returns(newModelId);

            mockRepo.Setup(repo => repo.UpdateById(mockModel.Object.ModelId, mockModel.Object)).ReturnsAsync(false);
            var controller = new AModelController<IModel>(mockRepo.Object);

            // Act
            var expected = new NotFoundResult();
            IActionResult result = controller.Put(newModelId, mockModel.Object).Result;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.ToString(), result.ToString());
        }

        [Fact]
        public void DeleteByIdShouldReturn204()
        {
            var mockRepo = new Mock<IRepository<IModel>>();
            var newModelId = "5a74ca174bbfc73ea03e8bb2";
            var mockModel = new Mock<IModel>();
            //mock model
            mockModel.Setup(m => m.ModelId).Returns(newModelId);
            //give modelid from mockmodel to mockrepo
            mockRepo.Setup(repo => repo.DeleteById(mockModel.Object.ModelId)).ReturnsAsync(true);
            var controller = new AModelController<IModel>(mockRepo.Object);

            //should return 204, id to delete is in mock object
            IActionResult result = controller.Delete(newModelId).Result;
            var expected = new NoContentResult();//204
            var objectResult = result as NoContentResult;

            Assert.Equal(expected.ToString(), objectResult.ToString());
            //verify delete method was called
            mockRepo.Verify(r => r.DeleteById(newModelId));
        }

        [Fact]
        public void DeleteByIdShouldReturn404()
        {
            var mockRepo = new Mock<IRepository<IModel>>();
            var newModelId = "5a74ca174bbfc73ea03e8bb2";
            var mockModel = new Mock<IModel>();
            //mock model
            mockModel.Setup(m => m.ModelId).Returns(newModelId);
            //give modelid from mockmodel to mockrepo
            mockRepo.Setup(repo => repo.DeleteById(mockModel.Object.ModelId)).ReturnsAsync(true);
            var controller = new AModelController<IModel>(mockRepo.Object);

            //should return 404, id is different
            IActionResult result = controller.Delete("4a84ca174bbfc73ea03e8bb2").Result;
            var expected = new NotFoundResult();//404
            var objectResult = result as NotFoundResult;

            Assert.Equal(expected.ToString(), objectResult.ToString());
            //verify delete method was called
            mockRepo.Verify(r => r.DeleteById("4a84ca174bbfc73ea03e8bb2"));
        }
    }
}
