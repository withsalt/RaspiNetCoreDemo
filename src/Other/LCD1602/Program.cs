using Iot.Device.CharacterLcd;
using System.Device.I2c;
using SixLabors.ImageSharp;

namespace LCD1602
{
    class Program
    {
        /// <summary>
        /// LCD1602
        /// 未测试
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var i2cLcdDevice = I2cDevice.Create(new I2cConnectionSettings(busId: 1, deviceAddress: 0x3E));
            var i2cRgbDevice = I2cDevice.Create(new I2cConnectionSettings(busId: 1, deviceAddress: 0x62));

            using (LcdRgb display = new LcdRgb(new Size(16, 2), i2cLcdDevice, i2cRgbDevice))
            {
                display.Clear();
                display.Write("Hello World!");
            }
        }
    }
}
