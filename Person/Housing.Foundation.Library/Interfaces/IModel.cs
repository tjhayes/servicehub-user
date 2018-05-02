using System;

namespace Housing.Foundation.Library.Interfaces
{
    /// <summary>
    /// This is an interface for AModel where Apartment, Person, and Batch classes implements from.
    /// </summary>
    public interface IModel
    {
        /// <value>Gets the ObjectId from MongoDB and stores as string for APIs</value>
        string ModelId { get; set; }

        ///<value>This gets the last modified date.</value>
        DateTime LastModified { get; set; }
    }
}
