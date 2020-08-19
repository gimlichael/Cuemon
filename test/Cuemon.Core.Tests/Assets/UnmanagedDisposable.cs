using System;
using System.Runtime.InteropServices;

namespace Cuemon.Assets
{
    public class UnmanagedDisposable : FinalizeDisposable
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall,
            SetLastError = true)]
        public static extern IntPtr CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr SecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile
        );
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        public IntPtr _handle = IntPtr.Zero;

        public UnmanagedDisposable()
        {
            _handle = CreateFile(@"C:\TestFile.txt",
                0x80000000, //access read-only
                1, //share-read
                IntPtr.Zero,
                3, //open existing
                0,
                IntPtr.Zero);
        }

        protected override void OnDisposeManagedResources()
        {

        }

        protected override void OnDisposeUnmanagedResources()
        {
            if (_handle != IntPtr.Zero)
            {
                CloseHandle(_handle);
            }
        }
    }
}