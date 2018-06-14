using System;
using System.Runtime.Serialization;


namespace ServiceHub.User.Library.Models
{
    /// <summary>
    /// A model for storing people's names.
    /// </summary>
    /// <remarks>
    /// For people with multiple first/middle/last names
    /// you can separate the names with whitespace/commas/etc.
    /// </remarks>
    public class Name
    {
        /// <value>The unique Id for the user's Name Object</value>
        public Guid NameId { get; set; }
        /// <value>The user's first name(s)</value>
        public string First { get; set; }
        /// <value>The user's middle name(s)</value>
        public string Middle { get; set; }
        /// <value>The user's surname(s) or last name(s)</value>
        public string Last { get; set; }

        /// <value>The maximum length of the user's first name(s)</value>
        [IgnoreDataMember]
        private static readonly int FirstMaxLength = 255;
        /// <value>The maximum length of the user's middle name(s)</value>
        [IgnoreDataMember]
        private static readonly int MiddleMaxLength = 255;
        /// <value>The maximum length of the user's last name(s)</value>
        [IgnoreDataMember]
        private static readonly int LastMaxLength = 255;

        /// <summary>
        /// Check that the Name object represents a valid name.
        /// </summary>
        /// <remarks>
        /// First and Last names are required, as well as Name Id. 
        /// Middle name is optional.
        /// No name fields can be an empty string.
        /// </remarks>
        /// <returns>True if the name is valid and false otherwise.</returns>
        public Boolean Validate()
        {
            return (NameId != Guid.Empty && 
                ValidateFirst(First) && 
                ValidateMiddle(Middle) && 
                ValidateLast(Last));
        }

        /// <summary>
        /// Check that the string parameter would represent a valid first name.
        /// </summary>
        /// <remarks>
        /// First name is required, must not be empty, and must not exceed FirstMaxLength.
        /// </remarks>
        /// <returns>True if the first name is valid and false otherwise.</returns>
        public static Boolean ValidateFirst(string first)
        {
            if (first == null || 
                first.Length > FirstMaxLength || 
                first == "")
            { 
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check that the string parameter would represent a valid middle name.
        /// </summary>
        /// <remarks>
        /// Middle name is not required.
        /// Middle name must not be empty if it is not null.
        /// Middle name must not exceed MiddleMaxLength.
        /// </remarks>
        /// <returns>True if the middle name is valid and false otherwise.</returns>
        public static Boolean ValidateMiddle(string middleName)
        {
            if(middleName == null)
            {
                return true;
            }
            else if (middleName.Length > MiddleMaxLength ||
                middleName == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Check that the string parameter would represent a valid last name.
        /// </summary>
        /// <remarks>
        /// Last name is required, must not be empty, and must not exceed LastMaxLength.
        /// </remarks>
        /// <returns>True if the last name is valid and false otherwise.</returns>
        public static Boolean ValidateLast(string lastName)
        {
            if (lastName == null ||
                lastName.Length > LastMaxLength ||
                lastName == "")
            {
                return false;
            }

            return true;
        }
    }
}
