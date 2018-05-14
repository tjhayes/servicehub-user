using System.ComponentModel.DataAnnotations;

namespace ServiceHub.Person.Library.Models
{
    public class Person
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        public string EMail { get; set; }
        [Required]
        public string BatchName { get; set; }
        [Phone]
        public string Phone { get; set; }
        [Required]
        public bool HasCar { get; set; }
        [Required]
        public bool IsMale { get; set; }
        [Required]
        public Address Address { get; set; }
        [Required]
        public string Role {get; set;}
        [Required]
        public long PersonID {get; set;}
    }
}
