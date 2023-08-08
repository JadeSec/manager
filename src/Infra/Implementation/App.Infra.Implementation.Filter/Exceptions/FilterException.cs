using System;

namespace App.Infra.Implementation.Filter.Exceptions
{
    public class FilterException: Exception
    {
        public FilterException(string message): base(message)
        {

        }
    }
}
