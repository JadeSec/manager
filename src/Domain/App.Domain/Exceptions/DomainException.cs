using System;

namespace App.Domain.Exceptions
{
    public class DomainException: Exception
    {
        /// <summary>
        /// Use in this scenario ex:
        /// try
        /// {
        ///  ...
        /// }
        /// catch(DomainException) when (e.When.Equal("expire"))
        /// {
        ///  ...
        /// }
        /// catch(DomainException) when (e.WhenEqual(DomainException.WhenTypes.NOT_FOUND))
        /// {
        ///  ...
        /// }
        /// 
        /// </summary>
        public string When { get; set; }

        public object[] Objects { get; set; }

        public DomainException(string message) : base(message)
        {
            When = string.Empty;
        }

        public DomainException(string message, params object[] objects) : base(message)
        {
            When = string.Empty;
            Objects = objects;
        }

        public DomainException(string message, string when, params object[] objects) : base(message)
        {
            When = when;
            Objects = objects;
        }

        public DomainException(string message, WhenTypes when, params object[] objects) : base(message)
        {
            When = _getWhenName(when);
            Objects = objects;
        }

        public DomainException(string message, string when) : base(message)
        {
            When = when;
        }

        public DomainException(string message, WhenTypes when) : base(message)
        {
            When = _getWhenName(when);
        }

        public DomainException(WhenTypes when) : base(string.Empty)
        {
            When = _getWhenName(when);
        }

        public bool WhenEqual(WhenTypes when)
            => When.Equals(_getWhenName(when));

        public bool WhenEqual(string when)
            => When.Equals(when);

        private string _getWhenName(WhenTypes when)
            => Enum.GetName(typeof(WhenTypes), when);

        public enum WhenTypes
        {
            NOT_FOUND,
            EXPIRED,
            INVALID,
            DENIED,
            FORBIDDEN,
            UNAUTHORIZED
        }
    }
}
