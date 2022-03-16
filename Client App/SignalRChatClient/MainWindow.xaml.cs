#region snippet_MainWindowClass
using System;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRChatClient
{
    public partial class MainWindow : Window
    {
        HubConnection connection;
        static SerialPort _serialPort;


        public MainWindow()
        {
            InitializeComponent();

            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:59557/PlanetSelecterHub")
                .Build();

            #region snippet_ClosedRestart
            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0,5) * 1000);
                await connection.StartAsync();
            };
            #endregion

            ConnectMethod();

            //_serialPort = new SerialPort();
            //_serialPort.PortName = "COM5";//Set your board COM
            //_serialPort.BaudRate = 9600;
            //_serialPort.Open();
            //while (true)
            //{
            //    string planetId = _serialPort.ReadExisting();
            //    if (planetId != "")
            //    {
            //      Console.WriteLine(planetId);
                    string planetId = "1";
                    SendPlanetId(planetId);
            //    }
            //}
        }


        private async Task ConnectMethod()
        {
            connection.On<string>("PlanetSelecter", (message) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var newMessage = $"{message}";
                });
            });
            
            await connection.StartAsync();
        }

        private async void SendPlanetId(string planetId)
        {
                await connection.InvokeAsync("SendMessage", planetId);
        }
    }
}
#endregion
