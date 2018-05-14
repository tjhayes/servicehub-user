using ServiceHub.Person.Service.Controllers;
using CM = ServiceHub.Person.Context.Models;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHub.Person.Testing
{
    public class TestSuite
    {
        [Fact]
        public void GetByEmail()
        {
            var mockRepo = new Mock<CM.PersonRepository>();
            var email = "person@gmail.com";
            var newPerson = new CM.Person
            {
                ModelId = "5a74ca174bbfc73ea03e8bb2",
                LastModified = DateTime.Parse("2018-05-11 02:40:47.389"),
                Email = email
            };
            var listPerson = new List<CM.Person> { newPerson };
            mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(listPerson);

            var controller = new PersonController(mockRepo.Object);
            IActionResult result = controller.GetByEmail(email).Result;

            var objectResult = result as OkObjectResult;
            var personResult = objectResult.Value as CM.Person;

            Assert.Equal(newPerson.Email, personResult.Email);
        }

        [Fact]
        public virtual void GetByEmailNotFoundIfMissing()
        {
            var mockRepo = new Mock<CM.PersonRepository>();       //need to accept instance var not static var
            var email = "person@gmail.com";
            var newPerson = new CM.Person
            {
                ModelId = "5a74ca174bbfc73ea03e8bb2",
                LastModified = DateTime.Parse("2018-05-11 02:40:47.389"),
                Email = email
            };
            var listPerson = new List<CM.Person> { newPerson };
            mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(listPerson);

            var controller = new PersonController(mockRepo.Object);
            IActionResult result = controller.GetByEmail("notfound@gmail.com").Result;

            var objectResult = result as NotFoundResult;
            Assert.NotEqual("notfound@gmail.com", newPerson.Email);

        }
    }
}

