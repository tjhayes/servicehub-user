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

            var actual = new CM.PersonRepository();

            Assert.True(expected == actual.GetType());
    }
  }
}
