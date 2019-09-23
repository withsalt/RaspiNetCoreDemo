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
            Console.WriteLine("Hello Serial port!");
            //注册退出事件
            Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs eventArgs) => { };

            var ports = SerialPort.GetPortNames();
            bool isTTY = false;
            foreach (var prt in ports)
            {
                Console.WriteLine($"Serial name: {prt}");
                if (prt.Contains("ttyS0"))
                {
                    isTTY = true;
                }
            }
            if (!isTTY)
            {
                Console.WriteLine("No ttyS0 serial port!");
                return;
            }
            Console.WriteLine("Yes, we have the embedded serial port available, opening it");
            SerialPort mySer = new SerialPort("/dev/ttyS0", 115200);
            mySer.DataReceived += DataReceived;
            mySer.Open();
            while (true)
            {
                byte[] send = Encoding.UTF8.GetBytes("NOW:" + DateTime.Now.ToString());
                mySer.Write(send, 0, send.Length);
                Thread.Sleep(1000);
            }
        }

        private static void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Console.WriteLine($"Received: {e.EventType.ToString()}");
        }
    }
}
