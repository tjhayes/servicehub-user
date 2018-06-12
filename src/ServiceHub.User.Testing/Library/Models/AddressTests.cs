using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using ServiceHub.User.Library.Models;

namespace ServiceHub.User.Testing.Library.Models
{
    public class AddressTests
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
        /// Generates a sample valid non-American user
        /// </summary>
        /// <returns>A valid user object with a non-American address</returns>
        public User.Library.Models.User Non_US_User()
        {
            User.Library.Models.User validNonAmericanUser = new User.Library.Models.User();
            validNonAmericanUser.Address = new Address();
            validNonAmericanUser.Address.AddressId =
                new Guid("44444444-4444-4444-4444-444444444444");
            validNonAmericanUser.Address.Address1 = "132 Old Road";
            validNonAmericanUser.Address.Address2 = "Apt 100";
            validNonAmericanUser.Address.City = "Maastricht";
            validNonAmericanUser.Address.PostalCode = "3581 CD";
            validNonAmericanUser.Address.State = "Limburg";
            validNonAmericanUser.Address.Country = "NL";
            validNonAmericanUser.Email = "sophie@jansen.com";
            validNonAmericanUser.Gender = "Female";
            validNonAmericanUser.Location = "Tampa";
            validNonAmericanUser.Name = new Name();
            validNonAmericanUser.Name.First = "Sophie";
            validNonAmericanUser.Name.Middle = "Emma";
            validNonAmericanUser.Name.Last = "Jansen";
            validNonAmericanUser.Name.NameId = new Guid("55555555-5555-5555-5555-555555555555");
            validNonAmericanUser.Type = "Associate";
            validNonAmericanUser.UserId = new Guid("66666666-6666-6666-6666-666666666666");
            return validNonAmericanUser;
        }

        /// <summary>
        /// Test that user Postal Code is required
        /// </summary>
        [Fact]
        [Trait("Type", "RequiredField")]
        public void PostalCodeRequired()
        {
            // Arrange
            User.Library.Models.User us = US_User();

            // Act
            us.Address.PostalCode = null;

            // Assert that null Postal Code fails validation
            Assert.False(us.Validate());
        }

        /// <summary>
        /// Test that user Postal Code isn't an empty string
        /// </summary>
        [Fact]
        [Trait("Type", "NotEmptyString")]
        public void PostalCodeNotEmptyString()
        {
            // Arrange
            User.Library.Models.User us = US_User();

            // Act
            us.Address.PostalCode = "";

            // Assert that empty string Postal Code fails validation
            Assert.False(us.Validate());
        }

        /// <summary>
        /// Test that invalid US country codes fail
        /// </summary>
        /// <param name="countryCode">The country code to test</param>
        [Theory]
        [Trait("Type", "TrueNegative")]
        [InlineData("")]
        [InlineData("United States")]
        [InlineData("USA")]
        public void InvalidUSCountryCodeFails(string countryCode)
        {
            // Arrange
            User.Library.Models.User us = US_User();

            // Act
            us.Address.Country = countryCode;

            // Assert that invalid US Country code fails
            Assert.False(us.Validate());
        }

        /// <summary>
        /// Test that Address must have an Id.
        /// </summary>
        [Fact]
        [Trait("Type", "RequiredField")]
        public void AddressIDRequired()
        {
            //Arrange
            User.Library.Models.User us = US_User();

            //Act
            us.Address.AddressId = Guid.Empty;

            //Assert that fail validation
            Assert.False(us.Address.Validate());
        }

        /// <summary>
        /// Test to ensure that at least one address field is filled in.
        /// </summary>
        [Fact]
        [Trait("Type", "RequiredField")]
        public void Address1Required()
        {
            //Arrange
            User.Library.Models.User us = US_User();

            //Act
            us.Address.Address1 = null;

            //Assert fail validation
            Assert.False(us.Address.Validate());
        }

        /// <summary>
        /// Test that the required address is not an empty string.
        /// </summary>
        [Fact]
        [Trait("Type", "NotEmptyString")]
        public void Address1IsNotEmptyString()
        {
            //Arrange
            User.Library.Models.User us = US_User();

            //Act
            us.Address.Address1 = "";

            //Assert fail validation
            Assert.False(us.Address.Validate());
        }

        /// <summary>
        /// Test to ensure that a secondary address is not required.
        /// </summary>
        [Fact]
        [Trait("Type", "NotRequired")]
        public void Address2NotRequired()
        {
            //Arrange
            User.Library.Models.User us = US_User();

            //Act
            us.Address.Address2 = null;

            //Assert pass validation
            Assert.True(us.Address.Validate());

        }

        /// <summary>
        /// Ensure that the secondary address is not an empty string
        /// </summary>
        [Fact]
        [Trait("Type", "AllowEmptyString")]
        public void Address2AllowEmptyString()
        {
            //Arrange
            User.Library.Models.User us = US_User();

            //Act
            us.Address.Address2 = "";

            //Assert fail validation
            Assert.True(us.Address.Validate());
        }

        /// <summary>
        /// Ensure that the city property is required.
        /// </summary>
        [Fact]
        [Trait("Type", "RequiredField")]
        public void CityRequired()
        {
            //Arrange
            User.Library.Models.User us = US_User();

            //Act
            us.Address.City = null;

            //Assert fail validation
            Assert.False(us.Address.Validate());
        }

        /// <summary>
        /// Ensure that the city is not an empty string.
        /// </summary>
        [Fact]
        [Trait("Type", "NotEmptyString")]
        public void CityNotEmptyString()
        {
            //Arrange
            User.Library.Models.User us = US_User();

            //Act
            us.Address.City = "";

            //Assert fail validation
            Assert.False(us.Address.Validate());
        }

        /// <summary>
        /// Test that state is required.
        /// </summary>
        [Fact]
        [Trait("Type", "RequiredField")]
        public void StateRequired()
        {
            //Arrange
            User.Library.Models.User us = US_User();

            //Act
            us.Address.State = null;

            //Assert fail validation
            Assert.False(us.Address.Validate());
        }

        /// <summary>
        /// Test that state is not an empty string.
        /// </summary>
        [Fact]
        [Trait("Type", "NotEmptyString")]
        public void StateNotEmptyString()
        {
            //Arrange
            User.Library.Models.User us = US_User();

            //Act
            us.Address.State = "";

            //Assert fail validation
            Assert.False(us.Address.Validate());
        }

        /// <summary>
        /// Test that valid state abbreviations pass.
        /// </summary>
        /// <param name="state"></param>
        [Theory]
        [Trait("Type", "TruePositive")]
        [InlineData("KS")]
        [InlineData("WV")]
        [InlineData("OK")]
        [InlineData("TX")]
        [InlineData("CA")]
        public void ValidAmericanStatePasses(string state)
        {
            //Arrange
            User.Library.Models.User us = US_User();

            //Act
            us.Address.State = state;

            //Assert pass validation
            Assert.True(us.Address.Validate());
        }

        /// <summary>
        /// Test that invalid state expressions fail.
        /// </summary>
        /// <param name="state"></param>
        [Theory]
        [Trait("Type", "TruePositive")]
        [InlineData("Kansas")]
        [InlineData("Wv")]
        [InlineData("Canada")]
        [InlineData("Europe")]
        [InlineData("I like pie")]
        public void InValidAmericanStateFails(string state)
        {
            //Arrange
            User.Library.Models.User us = US_User();

            //Act
            us.Address.State = state;

            //Assert pass validation
            Assert.False(us.Address.Validate());
        }

        /// <summary>
        /// Test that only proper postal code formats pass.
        /// </summary>
        /// <param name="zip"></param>
        [Theory]
        [Trait("Type", "TruePositive")]
        [InlineData("55555")]
        [InlineData("66043")]
        [InlineData("66048-1234")]
        [InlineData("66002")]
        [InlineData("12345-6789")]
        public void ValidAmericanPostalCodePasses(string zip)
        {
            //Arrange
            User.Library.Models.User us = US_User();

            //Act
            us.Address.PostalCode = zip;

            //Assert pass validation
            Assert.True(us.Address.Validate());
        }

        /// <summary>
        /// Test that invalid postal code formats fail.
        /// </summary>
        /// <param name="zip"></param>
        [Theory]
        [Trait("Type", "TruePositive")]
        [InlineData("5555")]
        [InlineData("66043-")]
        [InlineData("66048-12")]
        [InlineData("66002-12345")]
        [InlineData("123456")]
        public void InValidAmericanPostalCodeFails(string zip)
        {
            //Arrange
            User.Library.Models.User us = US_User();

            //Act
            us.Address.PostalCode = zip;

            //Assert pass validation
            Assert.False(us.Address.Validate());
        }
    }
}
