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
            if (direction == MarshalDirection.In)
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

        internal static IntPtr FromManaged(string value)
        {
            if (value == null) 
                return IntPtr.Zero;

            var buf = Encoding.UTF8.GetBytes(value + '\0');
            var ptr = Marshal.AllocHGlobal(buf.Length);

            Marshal.Copy(buf, 0, ptr, buf.Length);

            return ptr;
        }

        internal static string FromNative(IntPtr pNativeData)
        {
            if (pNativeData == IntPtr.Zero) 
                return null;

            int length = 0;

            while (Marshal.ReadByte(pNativeData, length) != 0) 
                length++;

            var buf = new byte[length];

            Marshal.Copy(pNativeData, buf, 0, length);

            return Encoding.UTF8.GetString(buf);
        }
    }
}
