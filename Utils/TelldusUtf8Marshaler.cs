using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TellCore.Utils
{
    internal enum MarshalDirection
    {
        In,
        Out
    }

    internal class TelldusUtf8Marshaler : ICustomMarshaler
    {
        static readonly TelldusUtf8Marshaler inputInstance = new TelldusUtf8Marshaler(MarshalDirection.In);
        static readonly TelldusUtf8Marshaler outputInstance = new TelldusUtf8Marshaler(MarshalDirection.Out);

        readonly MarshalDirection direction;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelldusUtf8Marshaler"/> class.
        /// </summary>
        /// <param name="direction">
        /// if set to <c>true</c> the marshaller will free strings using telldus API. 
        /// Use this for marshallers that receive strings from the API. If false the
        /// strings will</param>
        public TelldusUtf8Marshaler(MarshalDirection direction)
        {
            this.direction = direction;
        }

        public static ICustomMarshaler GetInstance(string cookie)
        {
            if (String.Equals(cookie, "in", StringComparison.OrdinalIgnoreCase))
                return inputInstance;

            if (String.Equals(cookie, "out", StringComparison.OrdinalIgnoreCase))
                return outputInstance;

            throw new ArgumentException("Support cookie values are 'in' or 'out'. See MarshalDirection enum");
        }

        public void CleanUpManagedData(object ManagedObj)
        {
        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
            if(direction == MarshalDirection.In)
                Marshal.FreeHGlobal(pNativeData);
            else
                NativeMethods.tdReleaseString(pNativeData);
        }

        public int GetNativeDataSize()
        {
            return -1;
        }

        public IntPtr MarshalManagedToNative(object ManagedObj)
        {
            if (direction == MarshalDirection.Out)
                throw new InvalidOperationException("Marshaller is used in output mode, can't marshal managed to native");

            if (ManagedObj == null) return IntPtr.Zero;

            var str = ManagedObj as string;

            if (str == null)
                throw new InvalidOperationException("TelldusUtf8Marshaler only supports marshalling strings");

            return FromManaged(str);
        }

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            if (direction == MarshalDirection.In)
                throw new InvalidOperationException("Marshaller is used in input mode, can't marshal managed to native");

            return FromNative(pNativeData);
        }

        // Borrowed from libgit2 https://github.com/libgit2/libgit2sharp/blob/cfae3571996d3d5cfb8fb37b1777fcc59d83b68a/LibGit2Sharp/Core/EncodingMarshaler.cs#L93
        internal static unsafe IntPtr FromManaged(string value)
        {
            if (value == null)
            {
                return IntPtr.Zero;
            }

            int length = Encoding.UTF8.GetByteCount(value);
            var buffer = (byte*)Marshal.AllocHGlobal(length + 1).ToPointer();

            if (length > 0)
            {
                fixed (char* pValue = value)
                {
                    Encoding.UTF8.GetBytes(pValue, value.Length, buffer, length);
                }
            }

            buffer[length] = 0;

            return new IntPtr(buffer);
        }

        // Borrowed from libgit2 https://github.com/libgit2/libgit2sharp/blob/cfae3571996d3d5cfb8fb37b1777fcc59d83b68a/LibGit2Sharp/Core/EncodingMarshaler.cs#L93
        internal static unsafe string FromNative(IntPtr pNativeData)
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
