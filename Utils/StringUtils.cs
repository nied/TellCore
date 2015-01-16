using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TellCore
{
    public static class StringUtils
    {
        // http://stackoverflow.com/questions/25127439/c-dll-sends-utf8-as-const-char-c-sharp-needs-to-do-what
        internal static unsafe string PointerToUtf8String(IntPtr pointer)
        {
            var data = new List<byte>();

            for (int i = 0; ; i++)
            {
                var currentByte = Marshal.ReadByte(pointer, i);

                if (currentByte == 0)
                    break;

                data.Add(currentByte);
            }

            return Encoding.UTF8.GetString(data.ToArray());
        }

        // http://stackoverflow.com/questions/10773440/conversion-in-net-native-utf-8-managed-string
        internal static unsafe char* StringToUtf8Pointer(string managedString)
        {
            int byteCount = Encoding.UTF8.GetByteCount(managedString);
            byte[] buffer = new byte[byteCount + 1];

            Encoding.UTF8.GetBytes(managedString, 0, managedString.Length, buffer, 0);
            IntPtr nativeUtf8 = Marshal.AllocHGlobal(buffer.Length);

            Marshal.Copy(buffer, 0, nativeUtf8, buffer.Length);

            return (char*)nativeUtf8;
        }
    }
}