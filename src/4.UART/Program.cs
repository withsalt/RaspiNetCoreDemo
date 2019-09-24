using System;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace _4.UART
{
    class Program
    {
        static void Main(string[] args)
        {
            //注册退出事件
            Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs eventArgs) => { };

            string portName = "ttyAMA0";
            try
            {
                var ports = SerialPort.GetPortNames();
                bool isTTY = false;
                if(ports.Length == 0)
                {
                    Console.WriteLine($"No serial port allow to use!");
                    return;
                }
                Console.WriteLine("Serial List:");
                foreach (var prt in ports)
                {
                    Console.WriteLine($"  {prt}");
                    if (prt.Contains(portName))
                    {
                        isTTY = true;
                    }
                }
                if (!isTTY)
                {
                    Console.WriteLine($"No {portName} serial port!");
                    return;
                }
                using SerialPort serial = new SerialPort($"/dev/{portName}", 115200);
                serial.DataReceived += DataReceived;
                serial.Encoding = Encoding.UTF8;
                serial.Open();
                if (!serial.IsOpen)
                {
                    Console.WriteLine($"Port {portName} cannot open!");
                    return;
                }
                while (true)
                {
                    string input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        continue;
                    }
                    serial.Write(input);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (sender is SerialPort)
                {
                    SerialPort serial = sender as SerialPort;
                    string read = serial.ReadLine();
                    Console.WriteLine(read);
                }
                Console.WriteLine($"Received: {e.EventType.ToString()}");
            }
            catch
            {

            }
        }
    }
}
