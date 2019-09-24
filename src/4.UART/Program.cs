﻿using System;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace _4.UART
{
class Program
{
    /// <summary>
    /// 树莓派串口操作，也可以用于Windows系统（COMx）
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
        {
            //注册退出事件
            Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs eventArgs) => { };

            string portName = "ttyAMA0";
            try
            {
                //查询系统中支持的串口列表
                var ports = SerialPort.GetPortNames();
                bool isTTY = false;
                if (ports.Length == 0)
                {
                    Console.WriteLine($"No serial port allow to use!");
                    return;
                }
                Console.WriteLine("Serial List:");
                foreach (var prt in ports)
                {
                    Console.WriteLine($"  {prt}");
                    if (prt.Contains(portName))
                        isTTY = true;
                }
                if (!isTTY)
                {
                    Console.WriteLine($"No {portName} serial port!");
                    return;
                }
                //Linux加上/dev
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    portName = $"/dev/{portName}";
                }
                //开始使用串口
                using SerialPort serial = new SerialPort(portName, 115200);
                serial.DataReceived += DataReceived;
                serial.Encoding = Encoding.UTF8;
                serial.Open();
                if (serial.IsOpen)
                {
                    Console.WriteLine($"Port {portName} has opened!");
                }
                else
                {
                    Console.WriteLine($"Port {portName} cannot open!");
                    return;
                }
                while (true)
                {
                    //读取输入的数据，然后发送
                    string input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        continue;
                    }
                    serial.Write(input);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    private static void DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        try
        {
            if (!(sender is SerialPort))
            {
                return;
            }
            SerialPort serial = sender as SerialPort;
            string read = serial.ReadExisting();
            Console.WriteLine("-->" + read);
        }
        catch { }
    }
}
}
