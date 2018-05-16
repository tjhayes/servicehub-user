using Xunit;
using CM = ServiceHub.Person.Context.Models;

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
    }
}
