using System;
using Xunit;
using ServiceHub.User.Library.Models;

namespace ServiceHub.User.Testing.Library.Models
{
    public class NameTests
    {
        /// <value> A string over the max limit size for Name. </value>
        readonly string OversizedName = new string('A', 256);
        /// <value> A string of max size for Name. </value>
        readonly string MaxLengthName = new string('A', 255);
        /// <value> An empty string. </value>
        readonly string UndersizedName = new string('A', 0);
        /// <value> A string of minimum size for Name. </value>
        readonly string MinLengthName = new string('A', 1);

        readonly Name ControlName = new Name() { NameId = Guid.NewGuid(), First = "John", Middle = "Jacob", Last = "Schmidt" };

        #region Validate
        #endregion


        #region ValidateFirst
        /// <summary>
        /// Tests to make sure a null first name would be invalid.
        /// </summary>
        [Fact]
        [Trait("Type", "RequiredField")]
        public void ValidateFirst_NullArgument_ReturnsFalse()
        {
            // Arrange
            // Act
            var result = Name.ValidateFirst(null);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests to make sure an empty string would not be an acceptable first name.
        /// </summary>
        [Fact]
        [Trait("Type", "NotEmptyString")]
        public void ValidateFirst_EmptyName_ReturnsFalse()
        {
            // Arrange
            // Act
            var result = Name.ValidateFirst(UndersizedName);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests to make sure an oversized string would not be an acceptable first name.
        /// </summary>
        [Fact]
        [Trait("Type", "OversizedString")]
        public void ValidateFirst_OversizedName_ReturnsFalse()
        {
            // Arrange
            // Act
            var result = Name.ValidateFirst(OversizedName);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests to make sure a 255 letter string would be an acceptable first name.
        /// </summary>
        [Fact]
        [Trait("Type", "Control")]
        public void ValidateFirst_MaxLengthName_ReturnsTrue()
        {
            // Arrange
            // Act
            var result = Name.ValidateFirst(MaxLengthName);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Tests to make sure a one letter string would be an acceptable first name.
        /// </summary>
        [Fact]
        [Trait("Type", "Control")]
        public void ValidateFirst_MinLengthName_ReturnsTrue()
        {
            // Arrange
            // Act
            var result = Name.ValidateFirst(MinLengthName);

            // Assert
            Assert.True(result);
        }
        #endregion

        #region ValidateMiddle
        /// <summary>
        /// Tests to make sure a null middle name would be valid.
        /// </summary>
        [Fact]
        [Trait("Type", "NotRequiredField")]
        public void ValidateMiddle_NullArgument_ReturnsTrue()
        {
            // Arrange
            // Act
            var result = Name.ValidateMiddle(null);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Tests to make sure an empty string would not be an acceptable middle name.
        /// </summary>
        [Fact]
        [Trait("Type", "NotEmptyString")]
        public void ValidateMiddle_EmptyName_ReturnsFalse()
        {
            // Arrange
            // Act
            var result = Name.ValidateMiddle(UndersizedName);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests to make sure an oversized string would not be an acceptable middle name.
        /// </summary>
        [Fact]
        [Trait("Type", "OversizedString")]
        public void ValidateMiddle_OversizedName_ReturnsFalse()
        {
            // Arrange
            // Act
            var result = Name.ValidateMiddle(OversizedName);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests to make sure a 255 letter string would be an acceptable middle name.
        /// </summary>
        [Fact]
        [Trait("Type", "Control")]
        public void ValidateMiddle_MaxLengthName_ReturnsTrue()
        {
            // Arrange
            // Act
            var result = Name.ValidateMiddle(MaxLengthName);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Tests to make sure a one letter string would be an acceptable middle name.
        /// </summary>
        [Fact]
        [Trait("Type", "Control")]
        public void ValidateMiddle_MinLengthName_ReturnsTrue()
        {
            // Arrange
            // Act
            var result = Name.ValidateMiddle(MinLengthName);

            // Assert
            Assert.True(result);
        }
        #endregion

        #region ValidateLast
        /// <summary>
        /// Tests to make sure a null last name would be invalid.
        /// </summary>
        [Fact]
        [Trait("Type", "RequiredField")]
        public void ValidateLast_NullArgument_ReturnsFalse()
        {
            // Arrange
            // Act
            var result = Name.ValidateLast(null);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests to make sure an empty string would not be an acceptable last name.
        /// </summary>
        [Fact]
        [Trait("Type", "NotEmptyString")]
        public void ValidateLast_EmptyName_ReturnsFalse()
        {
            // Arrange
            // Act
            var result = Name.ValidateLast(UndersizedName);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests to make sure an oversized string would not be an acceptable last name.
        /// </summary>
        [Fact]
        [Trait("Type", "OversizedString")]
        public void ValidateLast_OversizedName_ReturnsFalse()
        {
            // Arrange
            // Act
            var result = Name.ValidateLast(OversizedName);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests to make sure a 255 letter string would be an acceptable last name.
        /// </summary>
        [Fact]
        [Trait("Type", "Control")]
        public void ValidateLast_MaxLengthName_ReturnsTrue()
        {
            // Arrange
            // Act
            var result = Name.ValidateLast(MaxLengthName);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Tests to make sure a one letter string would be an acceptable last name.
        /// </summary>
        [Fact]
        [Trait("Type", "Control")]
        public void ValidateLast_MinLengthName_ReturnsTrue()
        {
            // Arrange
            // Act
            var result = Name.ValidateLast(MinLengthName);

            // Assert
            Assert.True(result);
        }
        #endregion
    }
}
