using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceHub.Person.Library.Filters
{
    public class PersonException : Exception
    {
        public ExceptionType Type { get; set; }
        public ExceptionCode Code { get; set; }

        public PersonException(ExceptionType type, ExceptionCode code)
        {
            Type = type;
            Code = code;
        }
    }
}
