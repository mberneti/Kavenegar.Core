using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Kavenegar;
using Kavenegar.Core.Exceptions;

namespace test
{
    class Program
    {
        static async Task Main()
        {
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
        }
    }
}
