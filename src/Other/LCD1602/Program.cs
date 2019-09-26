using Iot.Device.CharacterLcd;
using System;
using System.Device.I2c;

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

            using var lcd = new LcdRgb1602(i2cLcdDevice, i2cRgbDevice);
            lcd.Clear();
            lcd.Write("Hello World!");
        }
    }
}
