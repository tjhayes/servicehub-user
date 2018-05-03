using ServiceHub.Person.Library.Abstracts;

namespace ServiceHub.Person.Context.Models
{
    public class Person : AModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string BatchName { get; set; }
        public string Phone { get; set; }
        public bool HasCar { get; set; }
        public bool IsMale { get; set; }
        public string Address { get; set; }
    }
}
