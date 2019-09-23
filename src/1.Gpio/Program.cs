using System;
using System.Device.Gpio;
using System.Threading;

namespace _1.Gpio
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start!");
            try
            {
                int pin = 7;
                using (GpioController controller = new GpioController(PinNumberingScheme.Board))
                {
                    //注册退出事件
                    Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs eventArgs) => 
                    {
                        if (controller.IsPinOpen(pin))
                        {
                            controller.ClosePin(pin);
                        }
                        controller.Dispose();
                        Console.WriteLine("Close pin and exit successful!");
                    };
                    // 设置引脚
                    controller.OpenPin(pin, PinMode.Output);
                    if (!controller.IsPinOpen(pin))
                    {
                        Console.WriteLine($"Set pin {pin} output mode failed.");
                    }
                    while (true)
                    {
                        // 打开
                        controller.Write(pin, PinValue.High);
                        Thread.Sleep(500);
                        // 关闭 
                        controller.Write(pin, PinValue.Low);
                        Thread.Sleep(500);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
