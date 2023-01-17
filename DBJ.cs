using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace sharpwin
{
    internal class DBJ
    {
        static public void ShowMessage(string message,
        [CallerFilePath] string file = "",
        [CallerLineNumber] int lineNumber = 0,
        [CallerMemberName] string caller = null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n" + message);
            Console.ResetColor();
            Console.WriteLine("\nfile: " + file + "\nline " + lineNumber + "\nfunction:" + caller + "()");
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
