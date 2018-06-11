using System;

namespace ServiceHub.User.Library.Models
{
    /// <summary>
    /// Model for a user's personal information. 
    /// </summary>
    public class User
    {
        /// <value>The user's unique Id</value>
        public Guid UserId { get; set; }
        /// <value>The name of the training location</value>
        public string Location { get; set; }
        /// <value>The user's residential address</value>
        public Address Address { get; set; }
        /// <value>The user's email address</value>
        public string Email { get; set; }
        /// <value>Object storing the user's names (first, middle, last)</value>
        public Name Name { get; set; }
        /// <value>The user's gender (Male/Female)</value>
        public string Gender { get; set; }
        /// <value>The user's job title</value>
        public string Type { get; set; }

        /// <summary>
        /// Returns true if the User model is valid, and false otherwise.
        /// </summary>
        /// <remarks>
        /// All fields are required except Address, thus if any field besides
        /// Address is null or the Guid is the default value, the model is invalid.
        /// If Address is not null, the Address must be valid for the User to 
        /// be valid (via Address's validate method)
        /// Name must be valid for the User to be valid also (via Name's validate method).
        /// </remarks>
        /// <returns>True if user model is valid and false if invalid.</returns>
        public Boolean Validate()
        {
            return false;
        }
    }
}