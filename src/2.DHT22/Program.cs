using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.Threading;

namespace _2.Dht22
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //稳定时间
                Console.WriteLine("Init...Please wait...");
                Thread.Sleep(2000);

                //使用GPIO 26
                int pin = 26;

                //第一种 调用Iot.Device.DHTxx中Dht22方法
                //using (Iot.Device.DHTxx.Dht22 dht = new Iot.Device.DHTxx.Dht22(pin, PinNumberingScheme.Logical))
                //{
                //    while (true)
                //    {
                //        Console.WriteLine($"Temperature: {dht.Temperature.Celsius.ToString("0.0")} ℃, Humidity: { dht.Humidity.ToString("0.0")} %");
                //        Thread.Sleep(1000);
                //    }
                //}

                //第二种 使用下方的程序，也是来源于Iot.Device.DHTxx
                while (true)
                {
                    byte[] readBuff = Read(pin);
                    if (readBuff == null)
                    {
                        //读取失败
                        Thread.Sleep(300);
                        continue;
                    }
                    double tem = GetTemperature(readBuff);
                    double hum = GetHumidity(readBuff);
                    Console.WriteLine($"Temperature: {tem.ToString("0.0")} ℃, Humidity: { hum.ToString("0.0")} %");
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 获取温度
        /// </summary>
        /// <param name="readBuff"></param>
        /// <returns></returns>
        static double GetTemperature(byte[] readBuff)
        {
            var temp = (readBuff[2] & 0x7F) + readBuff[3] * 0.1;
            // if MSB = 1 we have negative temperature
            temp = ((readBuff[2] & 0x80) == 0 ? temp : -temp);
            return temp;
        }

        /// <summary>
        /// 获取湿度
        /// </summary>
        /// <param name="readBuff"></param>
        /// <returns></returns>
        static double GetHumidity(byte[] readBuff)
        {
            return (readBuff[0] << 8 | readBuff[1]) * 0.1;
        }

        /// <summary>
        /// 根据Datasheet编写驱动
        /// </summary>
        static byte[] Read(int pin)
        {
            uint loopCount = 10000;
            Stopwatch stopwatch = new Stopwatch();
            byte[] readBuff = new byte[5];
            byte readVal = 0;
            uint count;

            try
            {
                using (GpioController controller = new GpioController())
                {
                    //第一步 打开GPIO，准备读取
                    controller.OpenPin(pin);
                    if (!controller.IsPinOpen(pin))
                    {
                        throw new Exception($"Open pin {pin} failed.");
                    }

                    //第二步 主机发起读取，并接收应答
                    controller.SetPinMode(pin, PinMode.Output);
                    controller.Write(pin, PinValue.Low);
                    // wait at least 18 milliseconds
                    // here wait for 18 milliseconds will cause sensor initialization to fail
                    DelayMicroseconds(20000, true);

                    //拉高，然后等待30us，准备接收信号
                    controller.Write(pin, PinValue.High);
                    DelayMicroseconds(30, true);
                    controller.SetPinMode(pin, PinMode.InputPullUp);

                    //传感器拉低，作为应答信号
                    count = loopCount;
                    while (controller.Read(pin) == PinValue.Low)
                    {
                        if (count-- == 0)
                        {
                            return null;
                        }
                    }

                    //拉高，作为开始信号
                    count = loopCount;
                    while (controller.Read(pin) == PinValue.High)
                    {
                        if (count-- == 0)
                        {
                            return null;
                        }
                    }

                    //第三步 读取40bit的数据
                    for (int i = 0; i < 40; i++)
                    {
                        //每位数据的起始信号，50us的低电平信号
                        count = loopCount;
                        while (controller.Read(pin) == PinValue.Low)
                        {
                            if (count-- == 0)
                            {
                                return null;
                            }
                        }

                        // 26 - 28 microseconds represent 0
                        // 70 microseconds represent 1
                        stopwatch.Restart();
                        count = loopCount;
                        while (controller.Read(pin) == PinValue.High)
                        {
                            if (count-- == 0)
                            {
                                return null;
                            }
                        }
                        stopwatch.Stop();

                        //位数据“0”的格式为： 50 微秒的低电平加 26-28 微秒的高电平；
                        //位数据“1”的格式为： 50 微秒的低电平加 70 微秒的高电平；
                        //这里取30us
                        readVal <<= 1;
                        if (!(stopwatch.ElapsedTicks * 1000000F / Stopwatch.Frequency <= 30))
                        {
                            readVal |= 1;
                        }

                        if (((i + 1) % 8) == 0)
                        {
                            readBuff[i / 8] = readVal;
                        }
                    }

                    //数据总线 SDA 输出 40 位数据后，继续输出低电平 50 微秒后转为输入状态
                    //一次读取结束，拉高总线，有上拉电阻的话，会自动拉高
                    DelayMicroseconds(50, true);
                    controller.SetPinMode(pin, PinMode.Output);
                    controller.Write(pin, PinValue.High);
                }
                //校验
                if ((readBuff[4] == ((readBuff[0] + readBuff[1] + readBuff[2] + readBuff[3]) & 0xFF)))
                {
                    return readBuff;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delay for at least the specified <paramref name="microseconds"/>.
        /// </summary>
        /// <param name="microseconds">The number of microseconds to delay.</param>
        /// <param name="allowThreadYield">
        /// True to allow yielding the thread. If this is set to false, on single-proc systems
        /// this will prevent all other code from running.
        /// </param>
        static void DelayMicroseconds(int microseconds, bool allowThreadYield)
        {
            long start = Stopwatch.GetTimestamp();
            ulong minimumTicks = (ulong)(microseconds * Stopwatch.Frequency / 1_000_000);

            if (!allowThreadYield)
            {
                do
                {
                    Thread.SpinWait(1);
                } while ((ulong)(Stopwatch.GetTimestamp() - start) < minimumTicks);
            }
            else
            {
                SpinWait spinWait = new SpinWait();
                do
                {
                    spinWait.SpinOnce();
                } while ((ulong)(Stopwatch.GetTimestamp() - start) < minimumTicks);
            }
        }
    }
}