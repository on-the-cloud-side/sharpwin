using System;
using System.Runtime.InteropServices;

class HelloWorld
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [DllImport("user32.dll")]
    public static extern IntPtr CreateWindowEx(
      uint dwExStyle,
      string lpClassName,
      string lpWindowName,
      uint dwStyle,
      int x,
      int y,
      int nWidth,
      int nHeight,
      IntPtr hWndParent,
      IntPtr hMenu,
      IntPtr hInstance,
      IntPtr lpParam);

    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    public static extern bool UpdateWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

    static void Main()
    {
        var hWnd = CreateWindowEx(
          0,
          "static",
          "Hello, Windows!",
          0xCF0000,
          0,
          0,
          300,
          200,
          IntPtr.Zero,
          IntPtr.Zero,
          IntPtr.Zero,
          IntPtr.Zero);

        ShowWindow(hWnd, 1);
        UpdateWindow(hWnd);

        RECT rect;
        GetClientRect(hWnd, out rect);
        Console.WriteLine("Client Rectangle: {0}, {1}, {2}, {3}", rect.Left, rect.Top, rect.Right, rect.Bottom);
        Console.ReadKey();
    }
}
