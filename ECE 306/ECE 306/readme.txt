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