using System;
using Xunit;

namespace ServiceHub.User.Testing.Library.Models
{
    class NameTests
    {
        private static string OversizedName = new string('A', 256);
        private static string MaxLengthName = new string('A', 255);
        private static string MinLengthName = new string('A', 1);

    }
}
