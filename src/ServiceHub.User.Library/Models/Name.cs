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
        public Guid NameId { get; set; }
        public string First { get; set; }
        public string Middle { get; set; }
        public string Last { get; set; }
    }
}