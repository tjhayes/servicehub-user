using System;

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
        private const int FIRST_MAX_LENGTH = 255;
        /// <value>The maximum length of the user's middle name(s)</value>
        private const int MIDDLE_MAX_LENGTH = 255;
        /// <value>The maximum length of the user's last name(s)</value>
        private const int LAST_MAX_LENGTH = 255;

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
            if (NameId == Guid.Empty) { return false; }
            if (First == null || First == "") { return false; }
            else if (First.Length > FIRST_MAX_LENGTH) { return false; }
            if (Middle != null &&
                (Middle == "" || Middle.Length > MIDDLE_MAX_LENGTH)) { return false; }
            if (Last == null || Last == "") { return false; }
            else if (Last.Length > LAST_MAX_LENGTH) { return false; }

            return true;
        }
    }
}