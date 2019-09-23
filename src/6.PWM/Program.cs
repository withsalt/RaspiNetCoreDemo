using System;
using System.Device.Pwm.Drivers;
using System.Threading;

namespace _6.PWM
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello PWM!");
            try
            {
                using (var pwmChannel = new SoftwarePwmChannel(17))
                {
                    pwmChannel.Start();
                    for (double fill = 0.0; fill <= 1.0; fill += 0.01)
                    {
                        pwmChannel.DutyCyclePercentage = fill;
                        Thread.Sleep(300);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
