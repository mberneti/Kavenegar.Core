using Kavenegar.Core.Models.Enums;

namespace Kavenegar.Core.Exceptions
{
 public class ApiException : KavenegarException
 {
	readonly MetaCode _result;
	public ApiException(string message, int code)
	 : base(message)
	{
	 _result = (MetaCode)code;
	}

	public MetaCode Code
	{
	 get { return _result; }
	}

 }
}
