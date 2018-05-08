using ServiceHub.Person.Library.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace ServiceHub.Person.Library.Models
{
    public class Person : AModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string BatchName { get; set; }
        [Phone]
        public string Phone { get; set; }
        [Required]
        public bool HasCar { get; set; }
        [Required]
        public bool IsMale { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
