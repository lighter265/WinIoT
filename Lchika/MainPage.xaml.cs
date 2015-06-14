using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using myGpioExtentions;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

//デバッグメッセージは、Debug.WriteLine();で

//Stopwatch sw = new Stopwatch();
//sw.Start();
//sw.Stop();
//Debug.WriteLine(sw.ElapsedMilliseconds);


namespace Lchika
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private GpioController _gpio;
        private GpioPin _pin;
        private GpioPin _pin2;
        private const int LedPort1 = 5;
        private const int LedPort2 = 6;
        
        public MainPage()
        {
            this.InitializeComponent();
            ledBtn.Click += LedBtn_Click;
        }

        private void LedBtn_Click(object sender, RoutedEventArgs e)
        {
            _pin.RevPinValue();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            _gpio = GpioController.GetDefault();
            _pin = _gpio.OpenPin(LedPort1);
            _pin.SetDriveMode(GpioPinDriveMode.Output);
            _pin.Write(GpioPinValue.Low);

            _pin2 = _gpio.OpenPin(LedPort2);
            _pin2.SetDriveMode(GpioPinDriveMode.Output);
            _pin2.Write(GpioPinValue.Low);

            new Task(async () =>
            {
                while (true)
                {
                    _pin2.RevPinValue();
                    await Task.Delay(500);
                    _pin2.RevPinValue();
                    await Task.Delay(500);
                }
            }).Start();
        }
    }
}

namespace myGpioExtentions
{
    static class myGpioPin
    {
        public static void RevPinValue(this GpioPin _pin_)
        {
            _pin_.Write( (_pin_.Read() == GpioPinValue.High ? GpioPinValue.Low : GpioPinValue.High) );
        }
    }
}
