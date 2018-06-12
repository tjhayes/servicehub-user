using System;
using Xunit;
using ServiceHub.User.Library.Models;

namespace ServiceHub.User.Testing.Library.Models
{
    public class UserTests
    {
        /// <summary>
        /// Generates a sample valid American user
        /// </summary>
        /// <returns>A valid user object with an American address</returns>
        public User.Library.Models.User US_User()
        {
            User.Library.Models.User validAmericanUser = new User.Library.Models.User();
            validAmericanUser.Address = new Address();
            validAmericanUser.Address.AddressId = new Guid("11111111-1111-1111-1111-111111111111");
            validAmericanUser.Address.Address1 = "Apt 500, 100 Long Street";
            validAmericanUser.Address.Address2 = null;
            validAmericanUser.Address.City = "Tampa";
            validAmericanUser.Address.PostalCode = "12345";
            validAmericanUser.Address.State = "FL";
            validAmericanUser.Address.Country = "us";
            validAmericanUser.Email = "john@smith.com";
            validAmericanUser.Gender = "M";
            validAmericanUser.Location = "Tampa";
            validAmericanUser.Name = new Name();
            validAmericanUser.Name.First = "John";
            validAmericanUser.Name.Middle = null;
            validAmericanUser.Name.Last = "Smith";
            validAmericanUser.Name.NameId = new Guid("22222222-2222-2222-2222-222222222222");
            validAmericanUser.Type = "Associate";
            validAmericanUser.UserId = new Guid("33333333-3333-3333-3333-333333333333");
            return validAmericanUser;
        }

        /// <summary>
        /// Test a default user
        /// </summary>
        [Fact]
        [Trait("Type", "ControlGroup")]
        public void DefaultUserShouldBeInvalid()
        {
            User.Library.Models.User defaultUser = new User.Library.Models.User();
            Assert.False(defaultUser.Validate());
        }

        /// <summary>
        /// Test the sample American user
        /// </summary>
        [Fact]
        [Trait("Type", "ControlGroup")]
        public void SampleAmericanUserShouldBeValid()
        {
            Assert.True(US_User().Validate());
        }

        /// <summary>
        /// Test that UserId is required
        /// </summary>
        [Fact]
        [Trait("Type", "RequiredField")]
        public void UserIdRequired()
        {
            // Arrange
            User.Library.Models.User us = US_User();

            // Act
            us.UserId = Guid.Empty;

            // Assert that empty Guids fail validation
            Assert.False(us.Validate());
        }

        /// <summary>
        /// Test that user Location is required
        /// </summary>
        [Fact]
        [Trait("Type", "RequiredField")]
        public void UserLocationRequired()
        {
            // Arrange
            User.Library.Models.User us = US_User();

            // Act
            us.Location = null;

            // Assert that null Location fails validation
            Assert.False(us.Validate());
        }

        /// <summary>
        /// Test that user Location isn't an empty string
        /// </summary>
        [Fact]
        [Trait("Type", "NotEmptyString")]
        public void UserLocationNotEmptyString()
        {
            // Arrange
            User.Library.Models.User us = US_User();

            // Act
            us.Location = "";

            // Assert that empty string Location fails validation
            Assert.False(us.Validate());
        }

        /// <summary>
        /// Test that user Address is not required
        /// </summary>
        [Fact]
        [Trait("Type", "NotRequiredField")]
        public void UserAddressNotRequired()
        {
            // Arrange
            User.Library.Models.User us = US_User();

            // Act
            us.Address = null;

            // Assert that null Address passes
            Assert.True(us.Validate());
        }

        /// <summary>
        /// Test that user Email is required
        /// </summary>
        [Fact]
        [Trait("Type", "RequiredField")]
        public void UserEmailRequired()
        {
            // Arrange
            User.Library.Models.User us = US_User();

            // Act
            us.Email = null;

            // Assert that null Email fails validation
            Assert.False(us.Validate());
        }

        /// <summary>
        /// Test that user Email can't be an empty string
        /// </summary>
        [Fact]
        [Trait("Type", "NotEmptyString")]
        public void UserEmailNotEmptyString()
        {
            // Arrange
            User.Library.Models.User us = US_User();

            // Act
            us.Email = "";

            // Assert that empty string Email fails validation
            Assert.False(us.Validate());
        }

        /// <summary>
        /// Test that valid email addresses pass the validator
        /// </summary>
        /// <param name="email">the email address to test</param>
        [Theory]
        [Trait("Type", "TruePositive")]
        [InlineData("1_2-a.b+c@a.b-c")]
        [InlineData("a@[123.123.123.123]")]
        [InlineData("a@123.123.123.123")]
        public void ValidUserEmailPasses(string email)
        {
            // Arrange
            User.Library.Models.User us = US_User();

            // Act
            us.Email = email;

            // Assert that valid emails pass
            Assert.True(us.Validate());
        }

        /// <summary>
        /// Test that invalid email addresses fail
        /// </summary>
        /// <param name="email">the email to test</param>
        [Theory]
        [Trait("Type", "TrueNegative")]
        [InlineData("abc")]
        [InlineData("@domain.com")]
        [InlineData("my<p>hello</p>my@mail.com")]
        [InlineData("a@b@c.com")]
        [InlineData("abc.com")]
        [InlineData("a@b.com word")]
        public void InvalidUserEmailFails(string email)
        {
            // Arrange
            User.Library.Models.User us = US_User();

            // Act
            us.Email = email;

            // Assert that invalid emails fails
            Assert.False(us.Validate());
        }

        /// <summary>
        /// Test that user's Name is required
        /// </summary>
        [Fact]
        [Trait("Type", "RequiredField")]
        public void UserNameRequired()
        {
            // Arrange
            User.Library.Models.User us = US_User();

            // Act
            us.Name = null;

            // Assert that null Name fails validation
            Assert.False(us.Validate());
        }

        /// <summary>
        /// Test that user's Gender is required
        /// </summary>
        [Fact]
        [Trait("Type", "RequiredField")]
        public void UserGenderRequired()
        {
            // Arrange
            User.Library.Models.User us = US_User();

            // Act
            us.Gender = null;

            // Assert that null Gender fails validation
            Assert.False(us.Validate());
        }

        /// <summary>
        /// Test that valid Genders pass
        /// </summary>
        /// <param name="gender">The Gender string to test</param>
        [Theory]
        [Trait("Type", "TruePositive")]
        [InlineData("M")]
        [InlineData("m")]
        [InlineData("male")]
        [InlineData("MALE")]
        [InlineData("Male")]
        [InlineData("F")]
        [InlineData("f")]
        [InlineData("female")]
        [InlineData("FEMALE")]
        [InlineData("Female")]
        public void ValidGenderPasses(string gender)
        {
            // Arrange
            User.Library.Models.User us = US_User();

            // Act
            us.Gender = gender;

            // Assert that valid Gender passes
            Assert.True(us.Validate());
        }

        /// <summary>
        /// Test that invalid Genders fail
        /// </summary>
        /// <param name="gender">The Gender string to test</param>
        [Theory]
        [Trait("Type", "TrueNegative")]
        [InlineData("X")]
        [InlineData("")]
        [InlineData("12345")]
        public void InvalidGenderFails(string gender)
        {
            // Arrange
            User.Library.Models.User us = US_User();
            // Act
            us.Gender = gender;

            // Assert that invalid Gender fails
            Assert.False(us.Validate());
        }

        /// <summary>
        /// Test that user Type is required
        /// </summary>
        [Fact]
        [Trait("Type", "RequiredField")]
        public void UserTypeRequired()
        {
            // Arrange
            User.Library.Models.User us = US_User();
            // Act
            us.Type = null;

            // Assert that empty Type fails validation
            Assert.False(us.Validate());
        }

        /// <summary>
        /// Test that user Type isn't an empty string
        /// </summary>
        [Fact]
        [Trait("Type", "NotEmptyString")]
        public void UserTypeNotEmptyString()
        {
            // Arrange
            User.Library.Models.User us = US_User();

            // Act
            us.Type = "";
            // Assert that empty string Type fails validation
            Assert.False(us.Validate());
        }







    }
}
