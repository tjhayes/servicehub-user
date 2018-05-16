using Xunit;
//using Moq;
using CON = ServiceHub.Person.Service.Controllers;
using CM = ServiceHub.Person.Context.Models;
using LM = ServiceHub.Person.Library.Models;
using System.Collections.Generic;

namespace ServiceHub.Person.Testing.Service
{
    public class TestSuite
    {
        [Fact]
        public void PersonControllerTest()
        {
            LM.Settings mockSettings = new LM.Settings(new List<string>() { "mongodb://admin123", "1", "2", "3", "4", "http://www.google.com", "6" });
            CM.PersonRepository mockPersonRepo = new CM.PersonRepository(mockSettings);
            var expected = typeof(CON.PersonController);

            var actual = new CON.PersonController(mockPersonRepo, mockSettings);

            Assert.True(expected == actual.GetType());
        }
    }
}
