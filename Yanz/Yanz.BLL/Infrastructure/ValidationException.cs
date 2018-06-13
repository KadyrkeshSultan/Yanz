using System;

namespace Yanz.BLL.Infrastructure
{
    public class ValidationException : Exception
    {
        public string Property { get; protected set; }

        public ValidationException(string message, string prop)
            :base(message)
        {
            Property = prop;
        }
    }
}
