using System;
using System.Collections.Generic;
using System.Text;

namespace Person.Library.Filters
{
    public class FoundationException : Exception
    {
        public FoundationExceptionType Type { get; set; }
        public FoundationExceptionCode Code { get; set; }

        public FoundationException(FoundationExceptionType type, FoundationExceptionCode code)
        {
            Type = type;
            Code = code;
        }
    }
}
