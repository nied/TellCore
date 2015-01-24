using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TellCore.Utils
{
    internal class TelldusUtf8Marshaler : ICustomMarshaler
    {
        static readonly TelldusUtf8Marshaler staticInstance = new TelldusUtf8Marshaler();

        public static ICustomMarshaler GetInstance(string cookie)
        {
            return staticInstance;
        }

        public void CleanUpManagedData(object ManagedObj)
        {
        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
            NativeMethods.tdReleaseString(pNativeData);
        }

        public int GetNativeDataSize()
        {
            return -1;
        }

        public IntPtr MarshalManagedToNative(object ManagedObj)
        {
            throw new NotImplementedException();
        }

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            return DecodeUtf8StringFromPointer(pNativeData);
        }

        // Borrowed from libgit2 https://github.com/libgit2/libgit2sharp/blob/cfae3571996d3d5cfb8fb37b1777fcc59d83b68a/LibGit2Sharp/Core/EncodingMarshaler.cs#L93
        internal static unsafe string DecodeUtf8StringFromPointer(IntPtr pNativeData)
        {
            if (pNativeData == IntPtr.Zero) return null;

            var start = (byte*)pNativeData;
            byte* walk = start;

            // Find the end of the string
            while (*walk != 0) walk++;

            if (walk == start) return string.Empty;

            return new string((sbyte*)pNativeData.ToPointer(), 0, (int)(walk - start), Encoding.UTF8);
        }
    }
}
