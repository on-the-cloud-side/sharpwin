
Or,  you can use the following code. - it is just the same code but with different code structure. 
```c#
public class MessagePump
{
    // please refer the following code to   
    //  http://www.pinvoke.net/default.aspx/user32/CreateWindowEx.html  
    // and check on this for the marshal  
    // http://www.pinvoke.net/default.aspx/Structures.WNDCLASS  
    public void CreateMessagePump(IntPtr hInstance, IntPtr hPrevInstance, string lpszCmdLine, int nCmdShow)
    {
        //MSG msg;  
        bool bRet;
        WNDCLASS wc;

        lpszCmdLine = null;

        hInstance = System.Diagnostics.Process.GetCurrentProcess().Handle;
        //WNDCLASS wndClass;  
        //WNDCLASS wndClass = new WNDCLASS();  
        WNDCLASSEX wndClass = new WNDCLASSEX();
        //wndClass.cbSize = sizeof(WNDCLASSEX); // you cannot use the sizeof operator, instead use the Marshal.Sizeof ()   
        wndClass.cbSize = Marshal.SizeOf(typeof(WNDCLASSEX));

        string szName = "HelloWin";
        //wndClass.style = (uint) (ClassStyles.HorizontalRedraw | ClassStyles.VerticalRedraw);  
        //wndClass.style = ClassStyles.HorizontalRedraw | ClassStyles.VerticalRedraw;  
        wndClass.style = (int)(ClassStyles.HorizontalRedraw | ClassStyles.VerticalRedraw);


        // all those Method.MethodHandle.GetFunctionPointer() call is so STUPID  
        //wndClass.lpfnWndProc = ((WndProc)((hWnd, message, wParam, lParam) =>  
        //{  
        //    IntPtr hdc;  
        //    PAINTSTRUCT ps;  
        //    RECT rect;  
        //    //switch ((WM) message)  
        //    //{  
        //    //    WinAPI.BeginPaint(hWnd, out ps);  
        //    //    break;  
        //    //}  
        //    switch ((WM)message)  
        //    {  
        //        case WM.PAINT:  
        //            hdc = WinAPI.BeginPaint(hWnd, out ps);  
        //            WinAPI.GetClientRect(hWnd, out rect);  
        //            WinAPI.DrawText(hdc, "Hello, Windows 98!", -1, ref rect, Win32_DT_Constant.DT_SINGLELINE | Win32_DT_Constant.DT_CENTER | Win32_DT_Constant.DT_VCENTER);  
        //            WinAPI.EndPaint(hWnd, ref ps);  
        //            return IntPtr.Zero;  
        //            break;  
        //        case WM.DESTROY:  
        //            WinAPI.PostQuitMessage(0);  
        //            return IntPtr.Zero;  
        //            break;  
        //    }  

        //    return WinAPI.DefWindowProc(hWnd, (WM)message, wParam, lParam);  
        //}  
        //)).Method.MethodHandle.GetFunctionPointer();  

        wndClass.lpfnWndProc = Marshal.GetFunctionPointerForDelegate((WndProc)((hWnd, message, wParam, lParam) =>
        {
            IntPtr hdc;
            PAINTSTRUCT ps;
            RECT rect;
            //switch ((WM) message)  
            //{  
            //    WinAPI.BeginPaint(hWnd, out ps);  
            //    break;  
            //}  
            switch ((WM)message)
            {
                case WM.PAINT:
                    hdc = WinAPI.BeginPaint(hWnd, out ps);
                    WinAPI.GetClientRect(hWnd, out rect);
                    WinAPI.DrawText(hdc, "Hello, Windows 98!", -1, ref rect, Win32_DT_Constant.DT_SINGLELINE | Win32_DT_Constant.DT_CENTER | Win32_DT_Constant.DT_VCENTER);
                    WinAPI.EndPaint(hWnd, ref ps);
                    return IntPtr.Zero;
                    break;
                case WM.DESTROY:
                    WinAPI.PostQuitMessage(0);
                    return IntPtr.Zero;
                    break;
            }

            return WinAPI.DefWindowProc(hWnd, (WM)message, wParam, lParam);
        }
        ));


        //wndClass.lpfnWndProc = (WndProc) ((hWnd, message, wParam, lParam) => {   
        //    IntPtr hdc;  
        //    PAINTSTRUCT ps;  
        //    RECT rect;  
        //    //switch ((WM) message)  
        //    //{  
        //    //    WinAPI.BeginPaint(hWnd, out ps);  
        //    //    break;  
        //    //}  
        //    switch ((WM)message)  
        //    {  
        //        case WM.PAINT:  
        //            hdc = WinAPI.BeginPaint(hWnd, out ps);  
        //            WinAPI.GetClientRect(hWnd, out rect);  
        //            WinAPI.DrawText(hdc, "Hello, Windows 98!", -1, ref rect, Win32_DT_Constant.DT_SINGLELINE | Win32_DT_Constant.DT_CENTER | Win32_DT_Constant.DT_VCENTER);  
        //            WinAPI.EndPaint(hWnd, ref ps);  
        //            return IntPtr.Zero;  
        //            break;  
        //        case WM.DESTROY:  
        //            WinAPI.PostQuitMessage(0);  
        //            return IntPtr.Zero;  
        //            break;  
        //    }  

        //    return WinAPI.DefWindowProc(hWnd, (WM)message, wParam, lParam);  
        //}  
        //);  


        wndClass.cbClsExtra = 0;
        wndClass.cbWndExtra = 0;
        wndClass.hInstance = hInstance;
        wndClass.hIcon = WinAPI.LoadIcon(
            IntPtr.Zero, new IntPtr((int)SystemIcons.IDI_APPLICATION));
        //wndClass.hCursor = WinAPI.LoadCursor(IntPtr.Zero, (int)IdcStandardCursor.IDC_ARROW);  
        wndClass.hCursor = WinAPI.LoadCursor(IntPtr.Zero, (int)Win32_IDC_Constants.IDC_ARROW);
        wndClass.hbrBackground = WinAPI.GetStockObject(StockObjects.WHITE_BRUSH);
        wndClass.lpszMenuName = null;
        wndClass.lpszClassName = szName;


        //WindowStyleEx.WS_EX_OVERLAPPEDWINDOW  
        //ushort regResult = WinAPI.RegisterClass(ref wndClass); // change to RegisterClassEx  
        //ushort regResult = (ushort)WinAPI.RegisterClassEx(ref wndClass); // change the varie RegisterClassEx2  
        UInt16 regRest = WinAPI.RegisterClassEx2(ref wndClass);

        //if (regResult == 0)  
        //{  
        //    int lastError = Marshal.GetLastWin32Error();  
        //    string errorMessage = new Win32Exception(lastError).Message;  

        //    WinAPI.MessageBox(IntPtr.Zero, "This program requires windows NT!",  
        //        szName, (int) MessageBoxOptions.IconError);  
        //}  

        // this varie of CreateWindowEx do no work   
        //IntPtr hwnd = WinAPI.CreateWindowEx(  
        //    0,  
        //    szName,  
        //    "The Hello Program",  
        //    WindowStyles.WS_OVERLAPPEDWINDOW,  
        //    Win32_CW_Constant.CW_USEDEFAULT,  
        //    Win32_CW_Constant.CW_USEDEFAULT,  
        //    Win32_CW_Constant.CW_USEDEFAULT,  
        //    Win32_CW_Constant.CW_USEDEFAULT,  
        //    IntPtr.Zero,  
        //    IntPtr.Zero,  
        //    hInstance,  
        //    IntPtr.Zero);  

        IntPtr hwnd = WinAPI.CreateWindowEx2(
            0,
            regRest,
            "The hello proram",
            WindowStyles.WS_OVERLAPPEDWINDOW,
            Win32_CW_Constant.CW_USEDEFAULT,
            Win32_CW_Constant.CW_USEDEFAULT,
            Win32_CW_Constant.CW_USEDEFAULT,
            Win32_CW_Constant.CW_USEDEFAULT,
            IntPtr.Zero,
            IntPtr.Zero,
            hInstance,
            IntPtr.Zero);

        //IntPtr hwnd = WinAPI.CreateWindowEx(  
        //    WindowStylesEx.WS_EX_OVERLAPPEDWINDOW,  
        //    //new IntPtr((int)(uint)regResult),  
        //    //Marshal.PtrToStringAnsi(new IntPtr((int) (uint) regResult)),  
        //    szName,  
        //    //szName, //Window Class name  
        //    "The Hello Program", // window caption  
        //    WindowStyles.WS_OVERLAPPEDWINDOW, // Window style  
        //    Win32_CW_Constant.CW_USEDEFAULT,  // initial x position  
        //    Win32_CW_Constant.CW_USEDEFAULT, // initial y position  
        //    Win32_CW_Constant.CW_USEDEFAULT, // initial x size  
        //    Win32_CW_Constant.CW_USEDEFAULT, // initial y size  
        //    IntPtr.Zero,                     // parent window handle  
        //    IntPtr.Zero,                    // program menu handle  
        //    hInstance,                      // program instance handle  
        //    IntPtr.Zero);                   // Creation Parameter  

        if (hwnd == IntPtr.Zero)
        {
            int lastError = Marshal.GetLastWin32Error();
            string errorMessage = new Win32Exception(lastError).Message;
        }
        WinAPI.ShowWindow(hwnd, ShowWindowCommands.Normal);
        WinAPI.UpdateWindow(hwnd);
        WinAPI.UpdateWindow(hwnd);
        MSG msg;
        while (WinAPI.GetMessage(out msg, IntPtr.Zero, 0, 0) != 0)
        {
            WinAPI.TranslateMessage(ref msg);
            WinAPI.DispatchMessage(ref msg);
        }

        return;
    }
}
//Together with its new drive code.
static void Main(string[] args)
{
    //Main2(System.Diagnostics.Process.GetCurrentProcess().Handle, IntPtr.Zero, string.Empty, (int)ShowWindowCommands.Normal);  
    MessagePump messagePump = new MessagePump();
    messagePump.CreateMessagePump(IntPtr.Zero, IntPtr.Zero, "some string", 0);
}
```