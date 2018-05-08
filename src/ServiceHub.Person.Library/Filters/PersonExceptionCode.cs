using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceHub.Person.Library.Filters
{
    public enum PersonExceptionCode
    {
        OK,
        NotFound,
        Invalid,
        Created,
        AlreadyExists
    }
}
