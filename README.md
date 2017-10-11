# Kavenegar Dot Net Standard Client
A cross-platform library for the Kavenegar sms web service; written in C#.
This package uses [kavenegar-csharp](https://github.com/KaveNegar/kavenegar-csharp) project under the hood.

## Installation
The package can be installed via [nuget](https://www.nuget.org/packages/Kavenegar.Core/1.0.1-alpha):

##### Package Manager
```
Install-Package Kavenegar.Core -Version 1.0.1-alpha
```
##### .NET CLI
```
dotnet add package Kavenegar.Core --version 1.0.1-alpha
```

## Usage
Send SMS Example:

```c#
Console.OutputEncoding = Encoding.UTF8;
try
{
	var receptors = new List<string> { "<ReceptorNumber>" };

	var api = new KavenegarApi("<ApiKey>");

	var result = await api.Send("<SenderNumber>", receptors, "<Message>");

	foreach (var r in result)
	{
		Console.Write($"{r.Messageid.ToString()}");
	}

}
catch (ApiException ex)
{
	// در صورتی که خروجی وب سرویس 200 نباشد این خطارخ می دهد.
	Console.Write("Message : " + ex.Message);
}
catch (HttpException ex)
{
	// در زمانی که مشکلی در برقرای ارتباط با وب سرویس وجود داشته باشد این خطا رخ می دهد
	Console.Write("Message : " + ex.Message);
}
```
