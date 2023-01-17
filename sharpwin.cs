
//To use the functions and the message structures from win32api.cs, we can use the following code. 
using sharpwin;
using System;
using System.Runtime.InteropServices;

class Program
{
    private static IntPtr hinst;
    private static UInt16 atom;
    static void Main(string[] args)
    {
        Main2(System.Diagnostics.Process.GetCurrentProcess().Handle, IntPtr.Zero, string.Empty, (int)ShowWindowCommands.Normal);

        //MessagePump messagePump = new MessagePump();  
        //messagePump.CreateMessagePump(IntPtr.Zero, IntPtr.Zero, "some string", 0);  

        DBJ.ShowMessage("\n\nDone");
    }

    // Reference this page : Using Window Class -http://msdn.microsoft.com/en-us/library/ms633575%28v=VS.85%29.aspx  

    static bool Main2(IntPtr hinstance, IntPtr hPrevInstance, string lpCmdLine, int nCmdShow)
    {
        MSG msg;

        if (!InitApplication(hinstance))
            return false;

        if (!InitInstance(hinstance, nCmdShow))
            return false;

        sbyte hasMessage;

        while ((hasMessage = WinAPI.GetMessage(out msg, IntPtr.Zero, 0, 0)) != 0 && hasMessage != -1)
        {
            WinAPI.TranslateMessage(ref msg);
            WinAPI.DispatchMessage(ref msg);
        }
        return msg.wParam == UIntPtr.Zero;
        //UNREFERENCED_PARAMETER(lpCmdLine);   
    }


    private static bool InitApplication(IntPtr hinstance)
    {

        WNDCLASSEX wcx = new WNDCLASSEX();

        wcx.cbSize = Marshal.SizeOf(wcx);
        wcx.style = (int)(ClassStyles.VerticalRedraw | ClassStyles.HorizontalRedraw);

        unsafe 
        {
            //IntPtr address = MainWndProc; // --this is not necessary to put inside a Unsafe context
            IntPtr address2 = Marshal.GetFunctionPointerForDelegate((Delegate)(WndProc)MainWndProc);
            wcx.lpfnWndProc = address2;
        }
        const int IDI_APPLICATION = 32512; // DBJ was here
        wcx.cbClsExtra = 0;
        wcx.cbWndExtra = 0;
        wcx.hInstance = hinstance;
        wcx.hIcon =     WinAPI.LoadIcon( IntPtr.Zero, new IntPtr((int)IDI_APPLICATION));
        wcx.hCursor = WinAPI.LoadCursor(IntPtr.Zero, (int)Win32_IDC_Constants.IDC_ARROW);
        wcx.hbrBackground = WinAPI.GetStockObject(StockObjects.WHITE_BRUSH);
        wcx.lpszMenuName = "MainMenu";
        wcx.lpszClassName = "MainWClass";

        UInt16 ret = WinAPI.RegisterClassEx2(ref wcx);
        if (ret == 0)
        {
            DBJ.Win32ErrMessage("Failed to call RegisterClasEx");
            return false;
        }
        //return WinAPI.RegisterClassEx(ref wcx) != 0;  
        atom = ret;
        return ret != 0;
    }



    private static bool InitInstance(IntPtr hInstance, int nCmdShow)
    {
        IntPtr hwnd;

        hinst = hInstance;
        //short a = 0;

        hwnd = WinAPI.CreateWindowEx2(
            0,
            //"MainWClass",  <-- buggy?
            atom,
            "Sample",
            WindowStyles.WS_OVERLAPPEDWINDOW,
            //Win32_CW_Constant.CW_USEDEFAULT,
            //Win32_CW_Constant.CW_USEDEFAULT,
            //Win32_CW_Constant.CW_USEDEFAULT,
            //Win32_CW_Constant.CW_USEDEFAULT,
            10,10, 600, 300,
            IntPtr.Zero,
            IntPtr.Zero,
            hInstance,
            IntPtr.Zero);
        if (hwnd == IntPtr.Zero)
        {
            DBJ.Win32ErrMessage("Failed to InitInstance" );
            return false;
        }
        WinAPI.ShowWindow(hwnd, (ShowWindowCommands)nCmdShow);
        WinAPI.UpdateWindow(hwnd);
        return true;
    }

    // check this post - http://stackoverflow.com/questions/1969049/c-sharp-p-invoke-marshalling-structures-containing-function-pointers  
    //   
    static IntPtr MainWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
        IntPtr hdc;
        PAINTSTRUCT ps;
        RECT rect;

        switch ((WM)msg)
        {
            case WM.PAINT:
                hdc = WinAPI.BeginPaint(hWnd, out ps);
                WinAPI.GetClientRect(hWnd, out rect);
                WinAPI.DrawText(hdc, "Hello from 2023 to Windows 98!", -1, ref rect, Win32_DT_Constant.DT_SINGLELINE | Win32_DT_Constant.DT_CENTER | Win32_DT_Constant.DT_VCENTER);
                WinAPI.EndPaint(hWnd, ref ps);
                return IntPtr.Zero;
                //break;
            case WM.DESTROY:
                WinAPI.PostQuitMessage(0);
                return IntPtr.Zero;
                //break;
        }

        return WinAPI.DefWindowProc(hWnd, (WM)msg, wParam, lParam);
    }
}
