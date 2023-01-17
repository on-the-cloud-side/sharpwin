using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace sharpwin
{
    internal class DBJ
    {
        static public void ShowMessage(string message,
        [CallerFilePath] string file = "",
        [CallerLineNumber] int lineNumber = 0,
        [CallerMemberName] string caller = null)
        {
            //Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine("\n" + message);
            //Console.ResetColor();
            //Console.WriteLine("\nfile: " + file + "\nline " + lineNumber + "\nfunction:" + caller + "()");
#if DEBUG
            System.Diagnostics.Debug.WriteLine(message);
            System.Diagnostics.Debug.WriteLine("\nfile: " + file + "\nline " + lineNumber + "\nfunction:" + caller + "()");
#endif
        }

        static public void Win32ErrMessage(string message,
        [CallerFilePath] string file = "",
        [CallerLineNumber] int lineNumber = 0,
        [CallerMemberName] string caller = null)
        {
          
            ShowMessage(
            string.Format(message + ", error = {0}", new Win32Exception(Marshal.GetLastWin32Error()).Message),
            file,
            lineNumber,
            caller);
       
        }

    }
}
