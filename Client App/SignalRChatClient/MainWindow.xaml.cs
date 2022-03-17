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

        //Allows us to call our serialport with _serialPort
        static SerialPort _serialPort;

        public MainWindow()
        {
            InitializeComponent();

            //Calls our connection to the hub, with the url pointing towards our other program, with a parameter of the hub.
            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:59557/PlanetSelecterHub")
                .Build();

            //If the connection fails, and there is an error, wait and try again.
            #region snippet_ClosedRestart
            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };
            #endregion

            //Calls our connection method to open the connection to the other program, the moment we start the WPF.
            ConnectionOpen();
        }

        private async void ConnectionOpen()
        {
            await ConnectMethod();
        }

        private async Task ConnectMethod()
        {
            //We need to target the "PlanetSelecter" method, we have defined in the hub of our website
            connection.On<string>("PlanetSelecter", (planetId) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var newPlanetId = $"{planetId}";
                });
            });

            await connection.StartAsync();
        }

        private async void SendPlanetId(string planetId)
        {
            //sends our planetId over to the other program, so we can send the planet id
            await connection.InvokeAsync("SendMessage", planetId);
        }

        //Needed to make a button click for the loop, since we wouldn't get the WPF to get created as it would be stuck in the loop.
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Setup the serialport
            _serialPort = new SerialPort();
            //Define which com-port we are listening on
            _serialPort.PortName = "COM4";//Set your board COM
            //Defines the rate
            _serialPort.BaudRate = 9600;
            //Open the port so we can listen.
            _serialPort.Open();
            //While loop so we will keep listening for a value
            while (true)
            {
                //Set the string planetId to be what get's send from the arduino serial.
                string planetId = _serialPort.ReadExisting();

                //If the planetId isn't empty
                if (planetId != "")
                {
                    Console.WriteLine(planetId);
                    //Run the sendPlanetId method.
                    SendPlanetId(planetId);
                }
            }
        }
    }
}
#endregion
