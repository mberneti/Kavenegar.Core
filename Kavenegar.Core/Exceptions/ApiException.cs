using Kavenegar.Core.Models.Enums;

namespace Kavenegar.Core.Exceptions;

public class ApiException : KavenegarException
{
    public ApiException(
        string message,
        int code) : base(message)
    {
        Code = (MetaCode)code;
    }

    public MetaCode Code { get; }
}