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


    }
}
