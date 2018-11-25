# ECE_306_Controller_App
Application for the network control of a vehicle we create in ECE 306 at NC State University

This application is used for the remote control of the ECE 306 RC Cars.

For edits, the 2 files you have to change are MainPage.xaml for design changes, or MainPage.xaml.cs for edits to how commands work/message formatting.

This application was made using Microsoft Visual Studio 2017, using the blank template for a Visual C# UWP Application. Because of this, this program will only work on Windows 10, theoretically any version of it, although I have only tested with the most recent update.

Network Protocol: Through the use of StreamSocket, we set up a connection on first connect, and here we get to the first assumption: consistent connection to your vehicle. There is currently no error handling for a connection loss.
Message Style: I use a non-standard protocol in this for transmission, but it can be replaced with whatever is needed for you. The current protocol goes like:
* " P=(Pin) (Command)=(Time)"

When using the xbox controller, the formatting of the message is:
* " P=(Pin) l=(PWM_value)  P=(Pin) r=(PWM_value)", with PWM values between -10,000 and 10,000

I will try to comment at all location this style is used to simplify the adjustment of the code.

Controller Usage: Once connected, clicking the "XBOX" button will connect any Xbox One Controller currently plugged in/connected to the PC.

UI Usage: Start by connecting (IP and port), then enter your PIN into the box before attempting to send commands. Once connected, the other buttons will make themselves available. The forward/left/right/back commands require the time text box to contain the time length you want. The xbox controller system doesn't use the time variable, instead creating the PWM values.
    
A couple notes for those who want to use this:
1. There is no guarantee this works with your car. I made it to work for mine, and I don't have all the error codes needed to prepare this for other situations.
2. I put this together using multiple sets of command system located online, and adjusted them to fit the usage needed. I am, by no means, an expert in how each line of code takes action.
3. Feel free to make changes, and if they add functionality/error handling, please go ahead and upload the changes to the github linked below.

https://github.com/dmdeanha/ECE_306_Controller_App

Thank you,

Darian Deanhardt, dmdinnc@gmail.com
