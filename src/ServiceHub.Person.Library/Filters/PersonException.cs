using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceHub.Person.Library.Filters
{
    public class PersonException : Exception
    {
        public PersonExceptionType Type { get; set; }
        public PersonExceptionCode Code { get; set; }

        public PersonException(PersonExceptionType type, PersonExceptionCode code)
        {
            Type = type;
            Code = code;
        }
    }
}
