using System;

namespace Kavenegar.Core.Exceptions;

public class KavenegarException : Exception
{
    public KavenegarException(
        string message) : base(message)
    {
    }
}