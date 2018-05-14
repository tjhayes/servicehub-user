using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceHub.Person.Context.Models
{
    class Options : IOptions<Person>
    {
        Person IOptions<Person>.Value => throw new NotImplementedException();
    }
}
