﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Gaming.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

// Every protocol typically has a standard port number. For example, HTTP is typically 80, FTP is 20 and 21, etc.
// For this example, we'll choose an arbitrary port number.

namespace ECE_306
{
    /// <summary>
    /// This application is used for the remote control of the ECE 306 RC Cars.
    /// 
    /// Network Protocol: Through the use of StreamSocket, we set up a connection on
    ///     first connect, and here we get to the first assumption: consistent connection
    ///     to your vehicle. There is currently no error handling for a connection loss.
    /// Message Style: I use a non-standard protocol in this for transmission, but it can
    ///     be replaced with whatever is needed for you. The current protocol goes like:
    ///         " P=<pin> <command>=<time>"
    ///     When using the xbox controller, the formatting of the message is:
    ///         " P=<pin> l=<PWM_value>  P=<pin> r=<PWM_value>"
    ///     I will try to comment at all location this style is used to simplify the
    ///     adjustment of the code.
    /// 
    /// Controller Usage: Once connected, clicking the "XBOX" button will connect
    ///     any Xbox One Controller currently plugged in/connected to the PC.
    /// 
    /// UI Usage: Start by connecting (IP and port), then enter your PIN into the box
    ///     before attempting to send commands. Once connected, the other buttons will
    ///     make themselves available. The forward/left/right/back commands require the
    ///     time text box to contain the time length you want. The xbox controller system
    ///     doesn't use the time variable, instead creating the PWM values.
    ///     
    /// A couple notes for those who want to use this:
    ///     1. There is no guarantee this works with your car. I made it to work for
    ///         mine, and I don't have all the error codes needed to prepare this
    ///         for other situations
    ///     2. I put this together using multiple sets of command system located online,
    ///         and adjusted them to fit the usage needed. I am, by no means, an expert
    ///         in how each line of code takes action.
    ///     3. Feel free to make changes, and if they add functionality/error handling,
    ///         please go ahead and upload the changes to the github linked below.
    /// 
    /// 
    /// 
    /// Thank you,
    ///     Darian Deanhardt
    ///     dmdinnc@gmail.com
    ///     
    ///     
    /// </summary>
    /// 



    public sealed partial class MainPage : Page
    {
        private Gamepad _Gamepad = null;

        private async void btnConnect_Click(object sender,
                                               RoutedEventArgs e)
        {
            Gamepad.GamepadAdded += Gamepad_GamepadAdded;
            Gamepad.GamepadRemoved += Gamepad_GamepadRemoved;

            while (true)
            {
                //sets up an asynchronous task that will run in parallel with the rest of the program.
                
                await Dispatcher.RunAsync(
                           CoreDispatcherPriority.Normal, () =>
                           {
                               if (_Gamepad == null)
                               {
                                   return;
                               }

                               // Get the current state
                               var reading = _Gamepad.GetCurrentReading();

                               double leftStickX = reading.LeftThumbstickX;   // returns a value between -1.0 and +1.0
                               double leftStickY = reading.LeftThumbstickY;   // returns a value between -1.0 and +1.0
                               double rightStickX = reading.RightThumbstickX; //returns value between -1 and 1
                               double rightStickY = reading.RightThumbstickY; //returns value between -1 and 1

                               // choose a deadzone -- readings inside this radius are ignored.
                               const double deadzoneRadius = 0.09;
                               const double deadzoneSquared = deadzoneRadius * deadzoneRadius;

                               // Pythagorean theorem -- for a right triangle, hypotenuse^2 = (opposite side)^2 + (adjacent side)^2
                               double oppositeSquaredLeft = leftStickY * leftStickY;
                               double adjacentSquaredLeft = leftStickX * leftStickX;
                               double oppositeSquaredRight = rightStickX * rightStickX;
                               double adjacentSquaredRight = rightStickY * rightStickY;

                               // accept and process input if true; otherwise, reject and ignore it.
                               if ((oppositeSquaredLeft + adjacentSquaredLeft) > deadzoneSquared)
                               {
                                   tbLeftThumbstickX.Text =
                                         reading.LeftThumbstickX.ToString();
                                   tbLeftThumbstickY.Text =
                                         reading.LeftThumbstickY.ToString();
                               } else
                               {
                                   leftStickX = 0.0;
                                   leftStickY = 0.0;
                                   tbLeftThumbstickX.Text = "0";
                                   tbLeftThumbstickY.Text = "0";
                               }
                               if((oppositeSquaredRight + adjacentSquaredRight) > deadzoneSquared)
                               {
                                   tbRightThumbstickX.Text =
                                        reading.RightThumbstickX.ToString();
                                   tbRightThumbstickY.Text =
                                        reading.RightThumbstickY.ToString();
                               } else
                               {
                                   rightStickX = 0.0;
                                   rightStickY = 0.0;
                                   tbRightThumbstickX.Text = "0";
                                   tbRightThumbstickY.Text = "0";
                               }


                               tbLeftTrigger.Text =
                       reading.LeftTrigger.ToString();
                               tbRightTrigger.Text =
                       reading.RightTrigger.ToString();
                               tbButtons.Text = string.Empty;
                               tbButtons.Text +=
                      (reading.Buttons & GamepadButtons.A)
                         == GamepadButtons.A ? "A " : "";
                               tbButtons.Text +=
                       (reading.Buttons & GamepadButtons.B)
                         == GamepadButtons.B ? "B " : "";
                               tbButtons.Text +=
                       (reading.Buttons & GamepadButtons.X)
                         == GamepadButtons.X ? "X " : "";
                               tbButtons.Text +=
                       (reading.Buttons & GamepadButtons.Y)
                         == GamepadButtons.Y ? "Y " : "";
                               tbButtons.Text +=
                       (reading.Buttons & GamepadButtons.LeftShoulder) ==
                         GamepadButtons.LeftShoulder
                           ? "LeftShoulder " : "";
                               tbButtons.Text +=
                       (reading.Buttons & GamepadButtons.RightShoulder) ==
                         GamepadButtons.RightShoulder
                           ? "RightShoulder " : "";
                               tbButtons.Text +=
                       (reading.Buttons & GamepadButtons.LeftThumbstick) ==
                         GamepadButtons.LeftThumbstick
                           ? "LeftThumbstick " : "";
                               tbButtons.Text +=
                       (reading.Buttons & GamepadButtons.RightThumbstick) == GamepadButtons.RightThumbstick
                           ? "RightThumbstick " : "";
                               tbButtons.Text +=
                       (reading.Buttons & GamepadButtons.DPadLeft) ==
                         GamepadButtons.DPadLeft ? "DPadLeft " : "";
                               tbButtons.Text +=
                       (reading.Buttons & GamepadButtons.DPadRight) ==
                         GamepadButtons.DPadRight ? "DPadRight " : "";
                               tbButtons.Text +=
                       (reading.Buttons & GamepadButtons.DPadUp) ==
                         GamepadButtons.DPadUp ? "DPadUp " : "";
                               tbButtons.Text +=
                       (reading.Buttons & GamepadButtons.DPadDown) ==
                         GamepadButtons.DPadDown ? "DPadDown " : "";

                               int PWM_Left = Convert.ToInt16(leftStickY * 10000); //the 10,000 values are my wheel period.
                               int PWM_Right = Convert.ToInt16(rightStickY * 10000);
                               string myMessage = "";
                               string lastMessage = "";

                               //The next two lines are related to the message formatting, if you follow a different protocol.
                               myMessage += " P=" + pin_input.Password + " l=" + Convert.ToString(PWM_Left);
                               myMessage += " P=" + pin_input.Password + " r=" + Convert.ToString(PWM_Right);

                               if (lastMessage != myMessage)
                               {
                                   lastMessage = myMessage;
                                   my_message_test.Text = myMessage;
                                   send_message(myMessage);
                               }

                           });

                await Task.Delay(TimeSpan.FromMilliseconds(200));
            }
        }

        //deals with controller disconnects, unchanged from where I found it.
        private async void Gamepad_GamepadRemoved(object sender,
                                                  Gamepad e)
        {
            _Gamepad = null;

            await Dispatcher.RunAsync(
                               CoreDispatcherPriority.Normal, () =>
                               {
                                   tbConnected.Text = "Controller removed";
                               });
        }
        //deals with controller connects, unchanged from where I found it.
        private async void Gamepad_GamepadAdded(object sender,
                                                Gamepad e)
        {
            _Gamepad = e;

            await Dispatcher.RunAsync(
                         CoreDispatcherPriority.Normal, () =>
                         {
                             tbConnected.Text = "Controller added";
                         });
        }





    //Sockets setup
    public Windows.Networking.Sockets.StreamSocket streamSocket = new Windows.Networking.Sockets.StreamSocket();
        public Stream outputStream;
        public StreamWriter streamWriter;
        public String Socket_Status = "";
        public async void StartClient(string IP, string port)
        {
            try
            {
                // The server hostname that we will be establishing a connection to. In this example, the server and client are in the same process.
                var hostName = new Windows.Networking.HostName(IP);

                connection_status.Text = "Trying to connect...";

                await streamSocket.ConnectAsync(hostName, port);
                outputStream = streamSocket.OutputStream.AsStreamForWrite();
                streamWriter = new StreamWriter(outputStream);
                forward_send.IsEnabled = true;
                left_send.IsEnabled = true;
                right_send.IsEnabled = true;
                backwards_send.IsEnabled = true;
                btnConnect.IsEnabled = true;
                connection_status.Text = "Connected!";
            }
            catch (Exception ex)
             {
                Windows.Networking.Sockets.SocketErrorStatus webErrorStatus = Windows.Networking.Sockets.SocketError.GetStatus(ex.GetBaseException().HResult);
                this.log_list_box.Items.Add(webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : ex.Message);
                connection_status.Text = "Failed to Connect";
                forward_send.IsEnabled = false;
                left_send.IsEnabled = false;
                right_send.IsEnabled = false;
                backwards_send.IsEnabled = false;
                btnConnect.IsEnabled = false;
            }
        }

        // Send a request to the IOT device. Holds error handling, but only to tell you of a disconnect, not to fix it.
        public async void send_message(string message)
        {
            try
            {
                await streamWriter.WriteLineAsync(" " + message);
                await streamWriter.FlushAsync();
                this.log_list_box.Items.Add(string.Format("client sent the request: \"{0}\"", message));
            }
            catch (Exception ex)
            {
                Windows.Networking.Sockets.SocketErrorStatus webErrorStatus = Windows.Networking.Sockets.SocketError.GetStatus(ex.GetBaseException().HResult);
                this.log_list_box.Items.Add(webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : ex.Message);
                connection_status.Text = "Connection Lost";
            }
        }

        public MainPage()
        {
            this.InitializeComponent();
        }

        //Sets up the socket
        private void connect_button_Click(object sender, RoutedEventArgs e)
        {
            string ip_addr = ip_addr_input.Text;
            string port = port_input.Password;
            StartClient(ip_addr, port);

        }

        //Sends forward command 'F' with the time value specified
        private void forward_send_Click(object sender, RoutedEventArgs e)
        {
            send_message("P=" + pin_input.Password + " F=" + time_input.Text); //Edit message formatting here
        }

        private void backwards_send_Click(object sender, RoutedEventArgs e)
        {
            send_message("P=" + pin_input.Password + " B=" + time_input.Text); //Edit message formatting here
        }

        private void left_send_Click(object sender, RoutedEventArgs e)
        {
            send_message("P=" + pin_input.Password + " L=" + time_input.Text); //Edit message formatting here
        }

        private void right_send_Click(object sender, RoutedEventArgs e)
        {
            send_message("P=" + pin_input.Password + " R=" + time_input.Text); //Edit message formatting here
        }

        private void kill_motor_send_Click(object sender, RoutedEventArgs e)
        {
            send_message("P=" + pin_input.Password + " X=" + time_input.Text); //Edit message formatting here
        }
    }
}

