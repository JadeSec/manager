using System;

namespace App.Domain.Exceptions
{
    public class ControllerException : Exception
    {
        public ControllerException(string message) : base(message)
        {
        }
    }
}
