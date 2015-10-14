using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TellCore.Utils
{
    public class DisposableStringPointer : IDisposable
    {
        public string Value
        {
            get { return (string)OutMarshaler.MarshalNativeToManaged(Pointer); }
        }

        public IntPtr Pointer { get; set; }

        static TelldusUtf8Marshaler InMarshaler { get; set; }
        static TelldusUtf8Marshaler OutMarshaler { get; set; }

        static DisposableStringPointer()
        {
            InMarshaler = new TelldusUtf8Marshaler(MarshalDirection.In);
            OutMarshaler = new TelldusUtf8Marshaler(MarshalDirection.Out);
        }

        public DisposableStringPointer()
            : this(string.Empty)
        {
        }

        public DisposableStringPointer(string value)
        {
            Pointer = InMarshaler.MarshalManagedToNative(value);
        }

        public void Dispose()
        {
            InMarshaler.CleanUpNativeData(Pointer);
        }
    }
}