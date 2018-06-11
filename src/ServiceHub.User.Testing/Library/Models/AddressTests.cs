﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using ServiceHub.User.Library.Models;

namespace ServiceHub.User.Testing.Library.Models
{
    public class AddressTests
    {
        /// <value> Field for an Address to be tested.</value>
        Address _address;

        #region Test Data
        /// <value> A string over the max limit size for Name. </value>
        readonly string OversizedString = new string('A', 256);
        /// <value> A string of max size for Name. </value>
        readonly string MaxLengthString = new string('A', 255);
        /// <value> An empty string. </value>
        readonly string UndersizedString = new string('A', 0);
        /// <value> A string of minimum size for Name. </value>
        readonly string SingleCharacterString = new string('A', 1);

        /// <value> A generic name that will serve as the baseline for the tests. </value>
        readonly Address ControlAddress = new Address() {
            AddressId = new Guid("11111111-1111-1111-1111-111111111111"),
            Address1 = "100 Long Street",
            Address2 = "Apt 500",
            City = "Tampa",
            PostalCode = "12345",
            State = "FL",
            Country = "us" };
        
        /// <summary>
        /// MemberData set consisting of upper case state codes for all US states
        /// and territories.
        /// </summary>
        /// <returns>Returns each upper case state code </returns>
        public static IEnumerable<object[]> AmericanStateCodesUpperCase()
        {
            // American States
            yield return new object[] { "AL" };
            yield return new object[] { "AK" };
            yield return new object[] { "AR" };
            yield return new object[] { "AZ" };
            yield return new object[] { "CA" };
            yield return new object[] { "CO" };
            yield return new object[] { "CT" };
            yield return new object[] { "DE" };
            yield return new object[] { "FL" };
            yield return new object[] { "GA" };
            yield return new object[] { "HI" };
            yield return new object[] { "ID" };
            yield return new object[] { "IL" };
            yield return new object[] { "IN" };
            yield return new object[] { "IA" };
            yield return new object[] { "KS" };
            yield return new object[] { "KY" };
            yield return new object[] { "LA" };
            yield return new object[] { "ME" };
            yield return new object[] { "MD" };
            yield return new object[] { "MA" };
            yield return new object[] { "MI" };
            yield return new object[] { "MN" };
            yield return new object[] { "MS" };
            yield return new object[] { "MO" };
            yield return new object[] { "MT" };
            yield return new object[] { "NE" };
            yield return new object[] { "NV" };
            yield return new object[] { "NH" };
            yield return new object[] { "NJ" };
            yield return new object[] { "NM" };
            yield return new object[] { "NY" };
            yield return new object[] { "NC" };
            yield return new object[] { "ND" };
            yield return new object[] { "OH" };
            yield return new object[] { "OK" };
            yield return new object[] { "OR" };
            yield return new object[] { "PA" };
            yield return new object[] { "RI" };
            yield return new object[] { "SC" };
            yield return new object[] { "SD" };
            yield return new object[] { "TN" };
            yield return new object[] { "TX" };
            yield return new object[] { "UT" };
            yield return new object[] { "VT" };
            yield return new object[] { "VA" };
            yield return new object[] { "WA" };
            yield return new object[] { "WV" };
            yield return new object[] { "WI" };
            yield return new object[] { "WY" };

            // American territories
            yield return new object[] { "AS" }; 
            yield return new object[] { "DC" };
            yield return new object[] { "GU" };
            yield return new object[] { "MH" };
            yield return new object[] { "FM" };
            yield return new object[] { "MP" };
            yield return new object[] { "PW" };
            yield return new object[] { "PR" };
            yield return new object[] { "VI" };

        }
        
        /// <summary>
        /// MemberData set consisting of lower case state codes for all US states
        /// and territories.
        /// </summary>
        /// <returns>Returns each lower case state code. </returns>
        public static IEnumerable<object[]> AmericanStateCodesLowerCase()
        {
            // American States
            yield return new object[] { "al" };
            yield return new object[] { "ak" };
            yield return new object[] { "ar" };
            yield return new object[] { "az" };
            yield return new object[] { "ca" };
            yield return new object[] { "co" };
            yield return new object[] { "ct" };
            yield return new object[] { "de" };
            yield return new object[] { "fl" };
            yield return new object[] { "ga" };
            yield return new object[] { "hi" };
            yield return new object[] { "id" };
            yield return new object[] { "il" };
            yield return new object[] { "in" };
            yield return new object[] { "ia" };
            yield return new object[] { "ks" };
            yield return new object[] { "ky" };
            yield return new object[] { "la" };
            yield return new object[] { "me" };
            yield return new object[] { "me" };
            yield return new object[] { "ma" };
            yield return new object[] { "mi" };
            yield return new object[] { "mn" };
            yield return new object[] { "ms" };
            yield return new object[] { "mo" };
            yield return new object[] { "mt" };
            yield return new object[] { "ne" };
            yield return new object[] { "nv" };
            yield return new object[] { "nh" };
            yield return new object[] { "nj" };
            yield return new object[] { "nm" };
            yield return new object[] { "ny" };
            yield return new object[] { "nc" };
            yield return new object[] { "nd" };
            yield return new object[] { "oh" };
            yield return new object[] { "ok" };
            yield return new object[] { "or" };
            yield return new object[] { "pa" };
            yield return new object[] { "ri" };
            yield return new object[] { "sc" };
            yield return new object[] { "sd" };
            yield return new object[] { "tn" };
            yield return new object[] { "tx" };
            yield return new object[] { "ut" };
            yield return new object[] { "vt" };
            yield return new object[] { "va" };
            yield return new object[] { "wa" };
            yield return new object[] { "wv" };
            yield return new object[] { "wi" };
            yield return new object[] { "wy" };

            // American territories
            yield return new object[] { "as" };
            yield return new object[] { "dc" };
            yield return new object[] { "gu" };
            yield return new object[] { "mh" };
            yield return new object[] { "fm" };
            yield return new object[] { "mp" };
            yield return new object[] { "pw" };
            yield return new object[] { "pr" };
            yield return new object[] { "vi" };

        }


        #endregion

        /// <summary>
        /// Setup constructor.  Initializes _address to a passing Address value.
        /// </summary>
        public AddressTests()
        {
            _address = ControlAddress;
        }

        #region Validate
        
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
            _address.PostalCode = null;

            // Act
            var result = _address.Validate();

            // Assert that null Postal Code fails validation
            Assert.False(result);
        }

        /// <summary>
        /// Test that user Postal Code isn't an empty string
        /// </summary>
        [Fact]
        [Trait("Type", "NotEmptyString")]
        public void PostalCodeNotEmptyString()
        {
            // Arrange
            _address.PostalCode = "";

            // Act
            var result = _address.Validate();

            // Assert that empty string Postal Code fails validation
            Assert.False(result);
        }

        /// <summary>
        /// Test that user Country is required
        /// </summary>
        [Fact]
        [Trait("Type", "RequiredField")]
        public void CountryRequired()
        {
            // Arrange
            _address.Country = null;

            // Act
            var result = _address.Validate();

            // Assert that null Country fails validation
            Assert.False(result);
        }

        /// <summary>
        /// Test that valid non-US country codes pass
        /// </summary>
        /// <param name="countryCode">The country code to test</param>
        [Theory]
        [Trait("Type", "TruePositive")]
        [InlineData("GB")]
        [InlineData("ZA")]
        [InlineData("NL")]
        [InlineData("BR")]
        [InlineData("CO")]
        [InlineData("FR")]
        public void ValidNonUSCountryCodePasses(string countryCode)
        {
            // Arrange
            _address.Country = countryCode;

            // Act
            var result = _address.Validate();

            // Assert that valid Country passes
            Assert.True(result);
        }

        /// <summary>
        /// Test that valid US country codes pass
        /// </summary>
        /// <param name="countryCode">The country code to test</param>
        [Theory]
        [Trait("Type", "TruePositive")]
        [InlineData("US")]
        [InlineData("us")]
        public void ValidUSCountryCodePasses(string countryCode)
        {
            // Arrange
            _address.Country = countryCode;

            // Act
            var result = _address.Validate();

            // Assert that valid US Country code passes
            Assert.True(result);
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
            _address.Country = countryCode;

            // Act
            var result = _address.Validate();

            // Assert that invalid US Country code fails
            Assert.False(result);
        }

        /// <summary>
        /// Test that invalid non-US country codes fail
        /// </summary>
        /// <param name="countryCode">The country code to test</param>
        [Theory]
        [Trait("Type", "TrueNegative")]
        [InlineData("")]
        [InlineData("China")]
        [InlineData("ZZ")]
        [InlineData("YY")]
        [InlineData("XX")]
        [InlineData("A")]
        [InlineData("ABC")]
        public void InvalidNonUSCountryCodeFails(string countryCode)
        {
            // Arrange
            _address.Country = countryCode;

            // Act
            var result = _address.Validate();

            // Assert that invalid Non-US Country code fails
            Assert.False(result);
        }

        /// <summary>
        /// Test that an empty Guid is invalid.
        /// </summary>
        [Fact]
        [Trait("Type", "RequiredField")]
        public void AddressIDRequired()
        {
            //Arrange
            _address.AddressId = Guid.Empty;

            //Act
            var result = _address.Validate();

            //Assert that fail validation
            Assert.False(result);
        }

        /// <summary>
        /// Test that missing Address1 is invalid.
        /// </summary>
        [Fact]
        [Trait("Type", "RequiredField")]
        public void Address1Required()
        {
            //Arrange
            _address.Address1 = null;

            //Act
            var result = _address.Validate();

            //Assert fail validation
            Assert.False(result);
        }

        /// <summary>
        /// Test that the required address is not an empty string.
        /// </summary>
        [Fact]
        [Trait("Type", "NotEmptyString")]
        public void Address1IsNotEmptyString()
        {
            //Arrange
            _address.Address1 = "";

            //Act
            var result = _address.Validate();

            //Assert fail validation
            Assert.False(result);
        }

        /// <summary>
        /// Test to ensure that a secondary address is not required.
        /// </summary>
        [Fact]
        [Trait("Type", "NotRequired")]
        public void Address2NotRequired()
        {
            //Arrange
            _address.Address2 = null;

            //Act
            var result = _address.Validate();

            //Assert pass validation
            Assert.True(result);
        }

        /// <summary>
        /// Ensure that the secondary address is not an empty string
        /// </summary>
        [Fact]
        [Trait("Type", "NotEmptyString")]
        public void Address2NotEmptyString()
        {
            //Arrange
            _address.Address2 = "";

            //Act
            var result = _address.Validate();

            //Assert fail validation
            Assert.False(result);
        }

        /// <summary>
        /// Ensure that the city property is required.
        /// </summary>
        [Fact]
        [Trait("Type", "RequiredField")]
        public void CityRequired()
        {
            //Arrange
            _address.City = null;

            //Act
            var result = _address.Validate();

            //Assert fail validation
            Assert.False(result);
        }

        /// <summary>
        /// Ensure that the city is not an empty string.
        /// </summary>
        [Fact]
        [Trait("Type", "NotEmptyString")]
        public void CityNotEmptyString()
        {
            //Arrange
            _address.City = "";

            //Act
            var result = _address.Validate();

            //Assert fail validation
            Assert.False(result);
        }

        /// <summary>
        /// Test that state is required.
        /// </summary>
        [Fact]
        [Trait("Type", "RequiredField")]
        public void StateRequired()
        {
            //Arrange
            _address.State = null;

            //Act
            var result = _address.Validate();

            //Assert fail validation
            Assert.False(result);
        }

        /// <summary>
        /// Test that state is not an empty string.
        /// </summary>
        [Fact]
        [Trait("Type", "NotEmptyString")]
        public void StateNotEmptyString()
        {
            //Arrange
            _address.State = "";

            //Act
            var result = _address.Validate();

            //Assert fail validation
            Assert.False(result);
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
            _address.State = state;

            //Act
            var result = _address.Validate();

            //Assert pass validation
            Assert.True(result);
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
            _address.State = state;

            //Act
            var result = _address.Validate();

            //Assert pass validation
            Assert.False(result);
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
            _address.PostalCode = zip;
            _address.Country = "US";

            //Act
            var result = _address.Validate();

            //Assert pass validation
            Assert.True(result);
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
            _address.PostalCode = zip;
            _address.Country = "US";

            //Act
            var result = _address.Validate();

            //Assert pass validation
            Assert.False(result);
        }

        /// <summary>
        /// Tests to ensure that an oversized Address1 string is not valid.
        /// </summary>
        [Fact]
        public void Validate_OversizedAddress1_ReturnsFalse()
        {
            // Arrange
            _address.Address1 = OversizedString;

            // Act
            var result = _address.Validate();

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests to ensure that an oversized Address2 string is not valid.
        /// </summary>
        [Fact]
        public void Validate_OversizedAddress2_ReturnsFalse()
        {
            // Arrange
            _address.Address2 = OversizedString;

            // Act
            var result = _address.Validate();

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests to ensure that an oversized City string is not valid.
        /// </summary>
        [Fact]
        public void Validate_OversizedCity_ReturnsFalse()
        {
            // Arrange
            _address.City = OversizedString;

            // Act
            var result = _address.Validate();

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests to ensure that an oversized country string is not valid.
        /// </summary>
        [Fact]
        public void Validate_OversizedCountry_ReturnsFalse()
        {
            // Arrange
            _address.Country = OversizedString;

            // Act
            var result = _address.Validate();

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests to ensure that an oversized Address1 string is not valid.
        /// </summary>
        [Fact]
        public void Validate_OversizedPostalCode_ReturnsFalse()
        {
            // Arrange
            _address.PostalCode = OversizedString;

            // Act
            var result = _address.Validate();

            // Assert
            Assert.False(result);
        }
        #endregion

        #region ValidateAmericanState
        /// <summary>
        /// Tests to ensure that uppercase state codes are ruled as valid.
        /// </summary>
        [Theory, MemberData(nameof(AmericanStateCodesUpperCase))]
        public void ValidateAmericanState_ValidStateCodesUpperCase_ReturnsTrue(string state)
        {
            // Arrange
            _address.State = state;

            // Act
            var result = _address.ValidateAmericanState();

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Tests to ensure that lowercase state codes are ruled as invalid.
        /// </summary>
        [Theory, MemberData(nameof(AmericanStateCodesLowerCase))]
        public void ValidateAmericanState_ValidStateCodesLowerCase_ReturnsFalse(string state)
        {
            // Arrange
            _address.State = state;

            // Act
            var result = _address.ValidateAmericanState();

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests to ensure that state codes are ruled as invalid.
        /// </summary>
        [Fact]
        public void ValidateAmericanState_NullState_ReturnsFalse()
        {
            // Arrange
            _address.State = null;

            // Act
            var result = _address.ValidateAmericanState();

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests to ensure that empty state code is ruled as invalid.
        /// </summary>
        [Fact]
        public void ValidateAmericanState_EmptyState_ReturnsFalse()
        {
            // Arrange
            _address.State = "";

            // Act
            var result = _address.ValidateAmericanState();

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests to ensure that state codes with leading whitespace are ruled as invalid.
        /// </summary>
        [Fact]
        public void ValidateAmericanState_LeadingSpacesValidState_ReturnsFalse()
        {
            // Arrange
            _address.State = " AZ";

            // Act
            var result = _address.ValidateAmericanState();

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests to ensure that state codes with trailing whitespace are ruled as invalid.
        /// </summary>
        [Fact]
        public void ValidateAmericanState_TrailingSpacesValidState_ReturnsFalse()
        {
            // Arrange
            _address.State = "AZ ";

            // Act
            var result = _address.ValidateAmericanState();

            // Assert
            Assert.False(result);
        }
        #endregion

        #region ValidateAmericanPostalCode
        /// <summary>
        /// Tests to ensure that null zip codes are ruled as invalid.
        /// </summary>
        [Fact]
        public void ValidateAmericanPostalCode_NullZipCode_ReturnsFalse()
        {
            // Arrange
            _address.PostalCode = null;
            
            // Act
            var result = _address.ValidateAmericanPostalCode();

            // Assert
            Assert.False(result);
        }
        #endregion

    }
}
