# Kavenegar Dot Net Standard Client
A cross-platform library for the Kavenegar sms web service; written in C#.
This package uses [kavenegar-csharp](https://github.com/KaveNegar/kavenegar-csharp) project under the hood.

## Installation
The package can be installed via [nuget](https://www.nuget.org/packages/Kavenegar.Core/1.0.1-alpha2):

##### Package Manager
```
Install-Package Kavenegar.Core -Version 1.0.1-alpha2
```
##### .NET CLI
```
dotnet add package Kavenegar.Core --version 1.0.1-alpha2
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





<hr>

<div dir='rtl'>

	
#### راهنما

##### معرفی سرویس کاوه نگار

کاوه نگار یک وب سرویس ارسال و دریافت پیامک و تماس صوتی است که به راحتی میتوانید از آن استفاده نمایید.

##### ساخت حساب کاربری

اگر در وب سرویس کاوه نگار عضو نیستید میتوانید از [لینک عضویت](http://panel.kavenegar.com/client/membership/register) ثبت نام  و اکانت آزمایشی برای تست API دریافت نمایید.

##### مستندات

برای مشاهده اطلاعات کامل مستندات [وب سرویس پیامک](http://kavenegar.com/وب-سرویس-پیامک.html)  به صفحه [مستندات وب سرویس](http://kavenegar.com/rest.html) مراجعه نمایید.

##### راهنمای فارسی

در صورتی که مایل هستید راهنمای فارسی کیت توسعه کاوه نگار را مطالعه کنید به صفحه [کد ارسال پیامک](http://kavenegar.com/sdk.html) مراجعه نمایید.

##### اطالاعات بیشتر
برای مطالعه بیشتر به صفحه معرفی [وب سرویس کاوه نگار](http://kavenegar.com/%D9%88%D8%A8%D8%B3%D8%B1%D9%88%DB%8C%D8%B3-%D9%BE%DB%8C%D8%A7%D9%85%DA%A9.html) مراجعه نمایید .

 اگر در استفاده از کیت های سرویس کاوه نگار مشکلی یا پیشنهادی  داشتید ما را با یک Pull Request  یا  ارسال ایمیل به support@kavenegar.com  خوشحال کنید.
 

</div>
