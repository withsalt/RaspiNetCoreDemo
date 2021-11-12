# RaspiNetCoreDemo
树莓派.Net Core Iot Demo。在树莓派上面，使用C#操作GPIO，包含基本的GPIO操作示例和常见通讯协议Demo  

系列教程：[传送门](https://www.quarkbook.com/?p=686 "传送门")

### 前提条件
1、树莓派上面安装了.NET 6.0（框架依赖部署条件下）  
2、树莓派可以联网  
3、有一台安装了VS2019的计算机，并可以连接到树莓派（SSH）  

### 参考文献
- [张高兴的 .NET Core IoT 入门指南](https://zhangyue.xin/Article/Content/61 "张高兴的 .NET Core IoT 入门指南")
- [官方Demo](https://github.com/dotnet/iot/tree/master/src/devices "官方Demo")

### Demo列表
#### 1、Gpio
基本的GPIO操作，点亮一个LED灯泡。[传送门](https://www.quarkbook.com/?p=686 "传送门")

#### 2、DHT22
DHT22温湿度传感器的使用，DHT22是单总线的温湿度传感器。通过此案例可以深入学习GPIO的使用。[传送门](https://www.quarkbook.com/?p=699 "传送门")

#### 3、I2C
I2C通讯协议的学习，以SHT3x温湿度传感器为例。主要是SHT3x使用简单方便，没有繁杂的配置流程。[传送门](https://www.quarkbook.com/?p=709 "传送门")

#### 4、UART
树莓派GPIO硬件串口通讯。串口也是很常见的通讯协议，通过串口和电脑，Arduino之类的设备通讯。[传送门](https://www.quarkbook.com/?p=712 "传送门")

#### 5、SPI
SPI通讯协议的使用和学习。

#### 6、PWM
通过C#实现软件PWM，编写一个呼吸灯。
