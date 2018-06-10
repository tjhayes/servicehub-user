using System;
using Xunit;
using ServiceHub.User.Library.Models;

namespace ServiceHub.User.Testing.Library.Models
{
    public class NameTests
    {
        readonly string OversizedName = new string('A', 256);
        readonly string MaxLengthName = new string('A', 255);
        readonly string UndersizedName = new string('A', 0);
        readonly string MinLengthName = new string('A', 1);

        readonly Name ControlName = new Name() { NameId = Guid.NewGuid(), First = "John", Middle = "Jacob", Last = "Schmidt" };

        #region ValidateFirst
        [Fact]
        public void ValidateFirst_NullArgument_ReturnsFalse()
        {
            // Arrange
            // Act
            var result = Name.ValidateFirst(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateFirst_EmptyName_ReturnsFalse()
        {
            // Arrange
            // Act
            var result = Name.ValidateFirst(UndersizedName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateFirst_OversizedName_ReturnsFalse()
        {
            // Arrange
            // Act
            var result = Name.ValidateFirst(OversizedName);

            // Assert
            Assert.False(result);
        }
        #endregion

        #region ValidateMiddle
        [Fact]
        public void ValidateMiddle_NullArgument_ReturnsTrue()
        {
            // Arrange
            // Act
            var result = Name.ValidateMiddle(null);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateMiddle_EmptyName_ReturnsFalse()
        {
            // Arrange
            // Act
            var result = Name.ValidateMiddle(UndersizedName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateMiddle_OversizedName_ReturnsFalse()
        {
            // Arrange
            // Act
            var result = Name.ValidateMiddle(OversizedName);

            // Assert
            Assert.False(result);
        }
        #endregion

        
        #region ValidateLast
        [Fact]
        public void ValidateLast_NullArgument_ReturnsFalse()
        {
            // Arrange
            // Act
            var result = Name.ValidateLast(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateLast_EmptyName_ReturnsFalse()
        {
            // Arrange
            // Act
            var result = Name.ValidateLast(UndersizedName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateLast_OversizedName_ReturnsFalse()
        {
            // Arrange
            // Act
            var result = Name.ValidateLast(OversizedName);

            // Assert
            Assert.False(result);
        }
        #endregion
    }
}
