using ServiceHub.Person.Library.Models;
using Xunit;
using CM = ServiceHub.Person.Context.Models;
using LM = ServiceHub.Person.Library.Models;

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



            var Options = new LM.Settings
            {
                ConnectionString = "",
                Database = "",
                CollectionName = "",
                CacheExpirationMinutes = 0
            };

            var actual = new CM.PersonRepository(new);
    }
  }
}
