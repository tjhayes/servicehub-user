using Xunit;
//using moq;
using CM = ServiceHub.Person.Context.Models;
using System.Collections.Generic;
using ServiceHub.Person.Library.Models;

namespace ServiceHub.Person.Testing.Context
{
    public class TestSuite
  {
    [Fact]
    public void PersonTest()
    {
            var expected = typeof(CM.Person);

            var actual = new CM.Person();

            Assert.True(expected == actual.GetType());

    }
        [Fact]
        public void DbTimeUpdaterTest()
        {
            var expected = typeof(CM.MetaData);

            var actual = new CM.MetaData();

            Assert.True(expected == actual.GetType());
        }

        [Fact]
        public void PersonRepositoryTest()
        {
            Settings mockSettings = new Settings(new List<string>() { "mongodb://admin123", "1", "2", "3", "4", "http://www.google.com", "6" });
            CM.PersonRepository mockPersonRepo = new CM.PersonRepository(mockSettings);

            var expected = typeof(CM.PersonRepository);

            var actual = new CM.PersonRepository(mockSettings);


        }
    }
}
