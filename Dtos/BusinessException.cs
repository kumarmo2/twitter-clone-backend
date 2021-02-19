using System;

namespace Dtos
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message) { }

    }
}