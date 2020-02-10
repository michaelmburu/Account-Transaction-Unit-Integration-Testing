using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessTests
{
    public class UnitTestException : Exception
    {
        public UnitTestException(string message):base(message)
        {

        }
    }
}
