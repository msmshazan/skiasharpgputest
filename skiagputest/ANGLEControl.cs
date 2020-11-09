using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Media;
using PInvoke;
using SkiaSharp;

namespace skiagputest
{
    public class AngleControl : HwndHost
    {
      
        private AngleBackend ANGLE;
        private bool handled = false;
        

        public AngleControl(double Width, double Height)
        {
            ANGLE = new AngleBackend();
            this.Width = Width;
            this.Height = Height;
        }

        const int WS_EX_NOREDIRECTIONBITMAP = 0x00200000;
        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            var wndclass = new User32.WNDCLASSEX();
            wndclass.cbSize = Marshal.SizeOf<User32.WNDCLASSEX>();
            wndclass.style = 0;
            wndclass.hInstance = Kernel32.GetModuleHandle(null).DangerousGetHandle();
            User32.RegisterClassEx(ref wndclass);
            var windowHandle = User32.CreateWindowEx((User32.WindowStylesEx)WS_EX_NOREDIRECTIONBITMAP, "static", "", User32.WindowStyles.WS_CHILD | User32.WindowStyles.WS_CLIPCHILDREN | User32.WindowStyles.WS_VISIBLE, 0, 0, (int)Width, (int)Height, hwndParent.Handle, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            User32.SetWindowPos(windowHandle,hwndParent.Handle,0,0, (int)Width,(int)Height,User32.SetWindowPosFlags.SWP_SHOWWINDOW);
            ANGLE.InitializeContext((int)Width,(int)Height,true,windowHandle,User32.GetDC(windowHandle).DangerousGetHandle());
            var angleInterface = GRGlInterface.CreateAngle(ANGLE.AngleGetProcAddress);
            var grContext = GRContext.CreateGl(angleInterface);
            var buffer = ANGLE.GetFrameBuffer();
            var fbinfo = new GRGlFramebufferInfo(buffer, ANGLE.GetFramebufferFormat());
            var backendRT = new GRBackendRenderTarget((int)Width, (int)Height, 4, 8, fbinfo);
            var surfaceprops = new SKSurfaceProperties(SKSurfacePropsFlags.UseDeviceIndependentFonts, SKPixelGeometry.Unknown);
            var surface = SKSurface.Create(grContext, backendRT, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888, null, surfaceprops);
            var canvas = surface.Canvas;
            canvas.Clear(SKColor.Empty);
            SKPaint paint = new SKPaint();
            paint.Style = SKPaintStyle.Fill;
            paint.IsAntialias = true;
            paint.StrokeWidth = 4;
            paint.Color = new SKColor(0xff4285f4);
            SKRect rect = SKRect.Create(10,10,100,160);
            canvas.DrawRect(rect,paint);
            SKRoundRect oval = new SKRoundRect();
            oval.SetOval(rect);
            oval.Offset(40, 80);
            paint.Color = new SKColor(0xffdb4437);
            canvas.DrawRoundRect(oval, paint);
            paint.Color = new SKColor(0xff0f9d58);
            canvas.DrawCircle(180, 50, 25, paint);
            rect.Offset(80, 50);
            paint.Color = new SKColor(0xfff4b400);
            paint.Style = SKPaintStyle.Stroke;
            canvas.DrawRoundRect(rect, 10, 10, paint);
            canvas.Flush();
            ANGLE.Swap();
            return new HandleRef(this, windowHandle);
        }

        protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            handled = false;
            return IntPtr.Zero;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            ANGLE.Swap();
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            DestroyWindow(hwnd.Handle);
        }

        [DllImport("user32.dll", EntryPoint = "DestroyWindow", CharSet = CharSet.Unicode)]
        internal static extern bool DestroyWindow(IntPtr hwnd);
    }



}
