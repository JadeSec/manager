using System;

namespace App.Infra.Integration.Aws.Exceptions
{
    public class BucketException: Exception
    {
        public BucketException(string message) : base(message)
        {

        }
    }
}
