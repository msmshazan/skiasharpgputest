using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using PInvoke;
using SkiaSharp;
using EGLDisplay = System.IntPtr;
using EGLContext = System.IntPtr;
using EGLConfig = System.IntPtr;
using EGLSurface = System.IntPtr;
using EGLNativeDisplayType = System.IntPtr;
using EGLNativeWindowType = System.Object;
using EGLBoolean = System.UInt32;

namespace skiagputest
{
    public class AngleBackend
    {
        private const int EGL_FALSE = 0;
        private const int EGL_RENDER_BUFFER = 0x3086;
        private const int EGL_BACK_BUFFER = 0x3084;
        private const int EGL_HEIGHT = 0x3056;
        private const int EGL_WIDTH = 0x3057;
        private const int EGL_NONE = 0x3038;
        private const int EGL_TRUE = 1;
        private const int EGL_PLATFORM_ANGLE_ANGLE = 0x3202;
        private const int EGL_PLATFORM_ANGLE_TYPE_ANGLE = 0x3203;
        private const int EGL_PLATFORM_ANGLE_TYPE_D3D11_ANGLE = 0x3208;
        private const int EGL_PLATFORM_ANGLE_TYPE_D3D9_ANGLE = 0x3207;
        private const int EGL_ALPHA_SIZE = 0x3021;
        private const int EGL_BLUE_SIZE = 0x3022;
        private const int EGL_GREEN_SIZE = 0x3023;
        private const int EGL_RED_SIZE = 0x3024;
        private const int EGL_DEPTH_SIZE = 0x3025;
        private const int EGL_STENCIL_SIZE = 0x3026;
        private const int EGL_DIRECT_COMPOSITION_ANGLE = 0x33A5;
        private const int EGL_FIXED_SIZE_ANGLE = 0x3201;
        private const int EGL_CONTEXT_CLIENT_VERSION = 0x3098;
        private const int GL_FRAMEBUFFER_BINDING = 0x8CA6;
        private const int EGL_SAMPLES = 0x3031;
        private const int EGL_SAMPLE_BUFFERS = 0x3032;
        private const int EGL_PLATFORM_ANGLE_TYPE_VULKAN_ANGLE = 0x3450;
        private const int GL_BGRA8_EXT = 0x93A1;
        private const int EGL_PLATFORM_ANGLE_DEVICE_TYPE_ANGLE = 0x3209;
        private const int EGL_PLATFORM_ANGLE_DEVICE_TYPE_SWIFTSHADER_ANGLE = 0x3487;
        private const int EGL_WINDOW_BIT = 0x0004;
        private const int GL_READ_FRAMEBUFFER_ANGLE = 0x8CA8;
        private const int GL_DRAW_FRAMEBUFFER_ANGLE = 0x8CA9;
        private const int EGL_SURFACE_TYPE = 0x3033;
        
        private Delegate EGL_GetProcAddress(string name, Type type)
        {
            IntPtr addr = Win32GetProcAddress(EGL, name);
            if (addr == IntPtr.Zero)
            {
                throw new Exception(name);
            }
            return Marshal.GetDelegateForFunctionPointer(addr, type);
        }

        private Delegate GL_GetProcAddress(string name, Type type)
        {
            IntPtr addr = Win32GetProcAddress(GL, name);
            if (addr == IntPtr.Zero)
            {
                throw new Exception(name);
            }
            return Marshal.GetDelegateForFunctionPointer(addr, type);
        }

        public IntPtr AngleGetProcAddress(string name)
        {
            var ptr = IntPtr.Zero;
            ptr = EglGetProcAddress(name);
            if (ptr == IntPtr.Zero)
            {
                throw new Exception(name);

            }

            return ptr;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate EGLDisplay GetPlatformDisplayExt(uint platform, IntPtr display, int[] attribList);

        public GetPlatformDisplayExt EglGetPlatformDisplayExt;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate EGLBoolean Initialize(EGLDisplay dpy, ref int major, ref int minor);

        public Initialize EglInitialize;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void GetConfigs(EGLDisplay dpy, EGLConfig[] configs, int configSize, IntPtr numConfig);

        public GetConfigs EglGetConfigs;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate uint ChooseConfig(EGLDisplay dpy, int[] attribList, ref EGLConfig configs, int configSize, ref int numConfig);

        public ChooseConfig EglChooseConfig;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr CreateContext(EGLDisplay dpy, EGLConfig config, IntPtr shareContext, int[] attribList);

        public CreateContext EglCreateContext;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CreatePbufferSurface(EGLDisplay dpy, EGLConfig config, IntPtr attribList);

        public CreatePbufferSurface EglCreatePbufferSurface;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr CreateWindowSurface(EGLDisplay dpy, EGLConfig config, IntPtr win, int[] attribList);

        public CreateWindowSurface EglCreateWindowSurface;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate uint MakeCurrent(EGLDisplay dpy, EGLSurface draw, EGLSurface read, EGLContext ctx);

        public MakeCurrent EglMakeCurrent;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SwapBuffers(EGLDisplay dpy, EGLSurface surface);

        public SwapBuffers EglSwapBuffers;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void DestroySurface(EGLDisplay dpy, EGLSurface surface);

        public DestroySurface EglDestroySurface;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void DestroyContext(EGLDisplay dpy, EGLContext ctx);

        public DestroyContext EglDestroyContext;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void Terminate(EGLDisplay dpy);

        public Terminate EglTerminate;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SwapInterval(EGLDisplay dpy, int interval);

        public SwapInterval EglSwapInterval;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void WaitGL();

        public WaitGL EglWaitGL;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void WaitNative(int engine);

        public WaitNative EglWaitNative;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void WaitClient();

        public WaitClient EglWaitClient;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SurfaceAttrib(EGLDisplay dpy, EGLSurface surface, int attrib, int value);

        public SurfaceAttrib EglSurfaceAttrib;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr EGLGetProcAddress(string value);

        public EGLGetProcAddress EglGetProcAddress;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void GetIntegerv(uint pname, ref int data);

        public GetIntegerv GlGetIntegerv;
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void RenderbufferStorageMultisampleANGLE(uint target, int samples, uint internalformat, int width, int height);

        public RenderbufferStorageMultisampleANGLE GlRenderbufferStorageMultisampleANGLE;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void BlitFramebufferANGLE(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, uint filter);

        public BlitFramebufferANGLE GlBlitFramebufferANGLE;
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

        [DllImport("kernel32", CharSet = CharSet.Ansi,EntryPoint = "GetProcAddress", SetLastError = true)]
        static extern IntPtr Win32GetProcAddress(IntPtr hModule, string procName);

        public IntPtr EGL;
        public IntPtr GL;
        public IntPtr VK;
        public bool IsCpuBackend;

        public AngleBackend(bool isCpuBackend = false)
        {
            IsCpuBackend = isCpuBackend;
            var ptrsize = Marshal.SizeOf(typeof(IntPtr));
            if (ptrsize == 8)
            {
                EGL = LoadLibrary("x64/libEGL.dll");
                GL = LoadLibrary("x64/libGLESv2.dll");
            }
            else
            {
                EGL = LoadLibrary("x86/libEGL.dll");
                GL = LoadLibrary("x86/libGLESv2.dll");
            }
            EglChooseConfig = (ChooseConfig)EGL_GetProcAddress("eglChooseConfig", typeof(ChooseConfig));
            EglGetPlatformDisplayExt = (GetPlatformDisplayExt)EGL_GetProcAddress("eglGetPlatformDisplayEXT", typeof(GetPlatformDisplayExt));
            EglInitialize = (Initialize)EGL_GetProcAddress("eglInitialize", typeof(Initialize));
            EglGetConfigs = (GetConfigs)EGL_GetProcAddress("eglGetConfigs", typeof(GetConfigs));
            EglCreateContext = (CreateContext)EGL_GetProcAddress("eglCreateContext", typeof(CreateContext));
            EglCreatePbufferSurface = (CreatePbufferSurface)EGL_GetProcAddress("eglCreatePbufferSurface", typeof(CreatePbufferSurface));
            EglCreateWindowSurface = (CreateWindowSurface)EGL_GetProcAddress("eglCreateWindowSurface", typeof(CreateWindowSurface));
            EglDestroyContext = (DestroyContext)EGL_GetProcAddress("eglDestroyContext", typeof(DestroyContext));
            EglMakeCurrent = (MakeCurrent)EGL_GetProcAddress("eglMakeCurrent", typeof(MakeCurrent));
            EglSwapBuffers = (SwapBuffers)EGL_GetProcAddress("eglSwapBuffers", typeof(SwapBuffers));
            EglSurfaceAttrib = (SurfaceAttrib)EGL_GetProcAddress("eglSurfaceAttrib", typeof(SurfaceAttrib));
            EglWaitClient = (WaitClient)EGL_GetProcAddress("eglWaitClient", typeof(WaitClient));
            EglWaitGL = (WaitGL)EGL_GetProcAddress("eglWaitGL", typeof(WaitGL));
            EglDestroySurface = (DestroySurface)EGL_GetProcAddress("eglDestroySurface", typeof(DestroySurface));
            EglSwapInterval = (SwapInterval)EGL_GetProcAddress("eglSwapInterval", typeof(SwapInterval));
            EglTerminate = (Terminate)EGL_GetProcAddress("eglTerminate", typeof(Terminate));
            EglSurfaceAttrib = (SurfaceAttrib)EGL_GetProcAddress("eglSurfaceAttrib", typeof(SurfaceAttrib));
            EglWaitNative = (WaitNative)EGL_GetProcAddress("eglWaitNative", typeof(WaitNative));
            EglGetProcAddress = (EGLGetProcAddress)EGL_GetProcAddress("eglGetProcAddress", typeof(EGLGetProcAddress));
            GlGetIntegerv = (GetIntegerv) GL_GetProcAddress("glGetIntegerv", typeof(GetIntegerv));
            GlRenderbufferStorageMultisampleANGLE = 
                (RenderbufferStorageMultisampleANGLE) GL_GetProcAddress("glRenderbufferStorageMultisampleANGLE", typeof(RenderbufferStorageMultisampleANGLE));
            GlBlitFramebufferANGLE =
                (BlitFramebufferANGLE) GL_GetProcAddress("glBlitFramebufferANGLE", typeof(BlitFramebufferANGLE));
        }

        public uint GetFrameBuffer()
        {
            int buffer = -1;
            GlGetIntegerv(GL_FRAMEBUFFER_BINDING, ref buffer);
            return (uint) buffer;
        }

        public uint GetFramebufferFormat()
        {
            return (uint)GL_BGRA8_EXT;
    }
        private EGLDisplay display;
        private EGLSurface eglsurface;
        public void InitializeContext(int width,int height,IntPtr hwnd,IntPtr hdc)
        {
            int[] attributes = { EGL_PLATFORM_ANGLE_TYPE_ANGLE, EGL_PLATFORM_ANGLE_TYPE_D3D11_ANGLE, EGL_NONE };
            if (IsCpuBackend)
            {
                attributes = new [] {EGL_PLATFORM_ANGLE_DEVICE_TYPE_ANGLE,EGL_PLATFORM_ANGLE_DEVICE_TYPE_SWIFTSHADER_ANGLE , EGL_PLATFORM_ANGLE_TYPE_ANGLE, EGL_PLATFORM_ANGLE_TYPE_VULKAN_ANGLE, EGL_NONE };
            }
            display = EglGetPlatformDisplayExt(EGL_PLATFORM_ANGLE_ANGLE,hdc, attributes);
            int major = 0;
            int minor = 0;
            EglInitialize(display, ref major, ref minor);
            int[] configattribs =
            {
                //EGL_SURFACE_TYPE,EGL_WINDOW_BIT,
                EGL_BLUE_SIZE, 8,
                EGL_GREEN_SIZE, 8,
                EGL_RED_SIZE, 8,
                EGL_ALPHA_SIZE, 8,
                EGL_DEPTH_SIZE, 24,
                EGL_STENCIL_SIZE, 8,
                EGL_NONE
            };
            IntPtr surfaceConfig = IntPtr.Zero;
            int confignum = 0;
            EglChooseConfig(display, configattribs, ref surfaceConfig, 1, ref confignum);
            int[] contextattribs =
            {
                EGL_CONTEXT_CLIENT_VERSION, 3,
                EGL_NONE
            };
            int[] surfaceattribs =
            {
                EGL_RENDER_BUFFER, EGL_BACK_BUFFER,
                EGL_DIRECT_COMPOSITION_ANGLE , EGL_TRUE,
                EGL_FIXED_SIZE_ANGLE, EGL_TRUE,
                EGL_WIDTH,  width,
                EGL_HEIGHT, height,
                EGL_NONE
            };
            var context = EglCreateContext(display, surfaceConfig, IntPtr.Zero, contextattribs);
            eglsurface = EglCreateWindowSurface(display, surfaceConfig, hwnd, surfaceattribs);
            EglMakeCurrent(display, eglsurface, eglsurface, context);
        }

        public void Swap()
        {
            EglSwapBuffers(display, eglsurface);
        }
    }
}
