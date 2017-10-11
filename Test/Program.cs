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
                var receptors = new List<string> { "09356099419" };

                var api = new KavenegarApi("4E496A78385546715A62627834437A723941426263513D3D");

                var result = await api.Send("0013658000175", receptors, "خدمات پیام کوتاه کاوه نگار");

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
