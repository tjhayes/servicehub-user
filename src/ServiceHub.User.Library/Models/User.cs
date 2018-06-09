using System;

namespace ServiceHub.User.Library.Models
{
    /// <summary>
    /// Model for a user's personal information. 
    /// </summary>
    public class User
    {
        public Guid UserId { get; set; }
        /// <value>The name of the training location</value>
        public string Location { get; set; }
        /// <value>The user's residential address</value>
        public Address Address { get; set; }
        public string Email { get; set; }
        public Name Name { get; set; }
        public string Gender { get; set; }
        /// <value>The user's job title</value>
        public string Type { get; set; }
    }
}