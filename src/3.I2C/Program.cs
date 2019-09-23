using System;
using System.Device.I2c;
using System.Threading;

namespace _3.I2C
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                I2cConnectionSettings settings = new I2cConnectionSettings(1, (byte)I2cAddress.AddrLow);
                I2cDevice device = I2cDevice.Create(settings);

                using (Sht30 sensor = new Sht30(device))
                {
                    // read temperature (℃)
                    double temperature = sensor.Temperature;
                    // read humidity (%)
                    double humidity = sensor.Humidity;
                    // open heater
                    sensor.Heater = true;

                    Console.WriteLine($"Temperature: {temperature.ToString("0.0")} ℃, Humidity: { humidity.ToString("0.0")} %");
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
