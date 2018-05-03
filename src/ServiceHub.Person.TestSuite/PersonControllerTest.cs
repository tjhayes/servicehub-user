using ServiceHub.Person.Service.Controllers;
using CM = ServiceHub.Person.Context.Models;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using ServiceHub.Person.Library.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ServiceHub.Person.TestSuite
{
    public class PersonControllerTest
    {
        [Fact]
        public void PersonGetByEmail()
        {
            var mockRepo = new Mock<IRepository<CM.Person>>();
            var email = "person@gmail.com";
            var newPerson = new CM.Person
            {
                ModelId = "5a74ca174bbfc73ea03e8bb2",
                LastModified = DateTime.Parse("2018-02-07 09:58:47.389"),
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
        public void PersonGetByEmailShouldRespondNotFoundIfMissing()
        {
            var mockRepo = new Mock<IRepository<CM.Person>>();
            var email = "person@gmail.com";
            var newPerson = new CM.Person
            {
                ModelId = "5a74ca174bbfc73ea03e8bb2",
                LastModified = DateTime.Parse("2018-02-07 09:58:47.389"),
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
