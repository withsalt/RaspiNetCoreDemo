using Iot.Device.CpuTemperature;
using System;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                CpuTemperature cpuTemperature = new CpuTemperature();

                while (true)
                {
                    if (cpuTemperature.IsAvailable)
                    {
                        double temperature = cpuTemperature.Temperature.Celsius;
                        if (!double.IsNaN(temperature))
                        {
                            Console.WriteLine($"CPU Temperature: {temperature} C");
                        }
                    }

                    Thread.Sleep(1000);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
