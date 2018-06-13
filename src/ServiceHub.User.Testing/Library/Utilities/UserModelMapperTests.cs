using System;
using Xunit;
using ServiceHub.User.Context.Utilities;

namespace ServiceHub.User.Testing.Library.Utilities
{
    public class UserModelMapperTests
    {
        /// <summary>
        /// Generates a sample valid Library user
        /// </summary>
        /// <returns>A valid Library User</returns>
        public User.Library.Models.User LibraryUser()
        {
            User.Library.Models.User validLibraryUser = new User.Library.Models.User();
            validLibraryUser.Address = new User.Library.Models.Address();
            validLibraryUser.Address.AddressId = new Guid("11111111-1111-1111-1111-111111111111");
            validLibraryUser.Address.Address1 = "Apt 500, 100 Long Street";
            validLibraryUser.Address.Address2 = null;
            validLibraryUser.Address.City = "Tampa";
            validLibraryUser.Address.PostalCode = "12345";
            validLibraryUser.Address.State = "FL";
            validLibraryUser.Address.Country = "us";
            validLibraryUser.Email = "john@smith.com";
            validLibraryUser.Gender = "M";
            validLibraryUser.Location = "Tampa";
            validLibraryUser.Name = new User.Library.Models.Name();
            validLibraryUser.Name.First = "John";
            validLibraryUser.Name.Middle = null;
            validLibraryUser.Name.Last = "Smith";
            validLibraryUser.Name.NameId = new Guid("22222222-2222-2222-2222-222222222222");
            validLibraryUser.Type = "Associate";
            validLibraryUser.UserId = new Guid("33333333-3333-3333-3333-333333333333");
            return validLibraryUser;
        }

        /// <summary>
        /// Generates a sample valid Context User
        /// </summary>
        /// <returns>A valid Context User</returns>
        public User.Context.Models.User ContextUser()
        {
            User.Context.Models.User validContextUser = new User.Context.Models.User();
            validContextUser.Address = new User.Context.Models.Address();
            validContextUser.Address.AddressId = new Guid("11111111-1111-1111-1111-111111111111");
            validContextUser.Address.Address1 = "Apt 500, 100 Long Street";
            validContextUser.Address.Address2 = null;
            validContextUser.Address.City = "Tampa";
            validContextUser.Address.PostalCode = "12345";
            validContextUser.Address.State = "FL";
            validContextUser.Address.Country = "us";
            validContextUser.Email = "john@smith.com";
            validContextUser.Gender = "M";
            validContextUser.Location = "Tampa";
            validContextUser.Name = new User.Context.Models.Name();
            validContextUser.Name.First = "John";
            validContextUser.Name.Middle = null;
            validContextUser.Name.Last = "Smith";
            validContextUser.Name.NameId = new Guid("22222222-2222-2222-2222-222222222222");
            validContextUser.Type = "Associate";
            validContextUser.UserId = new Guid("33333333-3333-3333-3333-333333333333");
            return validContextUser;
        }


        [Fact]
        [Trait("Type", "UserModelMapper")]
        public void Valid_LibraryToContext_ShouldWork()
        {
            // Arrange
            var libraryUser = LibraryUser(); 

            // Act
            var contextUser = UserModelMapper.LibraryToContext(libraryUser);

            // Assert
            Assert.True(libraryUser.Validate());
            Assert.NotNull(contextUser);
            Assert.Equal(libraryUser.Name.First, contextUser.Name.First);
            Assert.Equal(libraryUser.Address.AddressId, contextUser.Address.AddressId);
        }

        [Fact]
        [Trait("Type", "UserModelMapper")]
        public void Invalid_LibraryToContext_ShouldReturnNull()
        {
            // Arrange
            var libraryUser = LibraryUser();
            libraryUser.Name = null;

            // Act
            var contextUser = UserModelMapper.LibraryToContext(libraryUser);

            // Assert that invalid library user maps to a null context user
            Assert.False(libraryUser.Validate());
            Assert.Null(contextUser);
        }

        [Fact]
        [Trait("Type", "UserModelMapper")]
        public void Null_LibraryToContext_ShouldReturnNull()
        {
            // Arrange
            User.Library.Models.User libraryUser = null;

            // Act
            var contextUser = UserModelMapper.LibraryToContext(libraryUser);

            // Assert that null library user maps to a null context user
            Assert.Null(contextUser);
        }

        [Fact]
        [Trait("Type", "UserModelMapper")]
        public void Valid_ContextToLibrary_ShouldWork()
        {
            // Arrange
            var contextUser = ContextUser();

            // Act
            var libraryUser = UserModelMapper.ContextToLibrary(contextUser);

            // Assert
            Assert.NotNull(libraryUser);
            Assert.Equal(libraryUser.Name.First, contextUser.Name.First);
            Assert.Equal(libraryUser.Address.AddressId, contextUser.Address.AddressId);
        }

        [Fact]
        [Trait("Type", "UserModelMapper")]
        public void Null_ContextToLibrary_ShouldReturnNull()
        {
            // Arrange
            User.Context.Models.User contextUser = null;

            // Act
            var libraryUser = UserModelMapper.ContextToLibrary(contextUser);

            // Assert
            Assert.Null(libraryUser);
        }
    }
}
