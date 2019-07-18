using System;

namespace RentCar.Services.Exceptions
{
    public class IntegrityException : ApplicationException
    {
        public IntegrityException(string msg) : base(msg)
        {
        }
    }
}
