using ServiceHub.Person.Library.Models;
using Xunit;
using CM = ServiceHub.Person.Context.Models;
using LM = ServiceHub.Person.Library.Models;
using Moq;
using Microsoft.Extensions.Options;


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
        public void PersonRepositoryTest()
        {
            var expected = typeof(CM.PersonRepository);



            var Preferences = new Settings
            {
                ConnectionString = "mongodb://servicehub-dev:u2mZcmB3PS3x3hGStIbkQWJuty9fIlkQdNIIIwTbAtqvEofS6zanW7GGjLn6SZZdM8Hvi96SLGoaJXjhgCr71w==@servicehub-dev.documents.azure.com:10255/?ssl=true&replicaSet=globaldb",
                Database = "test",
                CollectionName = "test",
                CacheExpirationMinutes = 0
            };
            

            var actual = new CM.PersonRepository(Options.Create<LM.Settings>(Preferences));

            Assert.True(expected == actual.GetType());
    }
  }
}
