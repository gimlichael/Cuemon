using System;
using System.IO;
using System.Reflection;
using Cuemon.Reflection;

namespace Cuemon.IO
{
    internal sealed class InternalStreamWriter : StreamWriter
    {
        private readonly IFormatProvider _provider;

        internal InternalStreamWriter(Stream output, StreamWriterSettings settings) : base(output, settings.Encoding, settings.BufferSize)
        {
            _provider = settings.FormatProvider;
            this.AutoFlush = settings.AutoFlush;
            this.NewLine = settings.NewLine;
            this.TryLeaveStreamOpen();
        }

        public override IFormatProvider FormatProvider
        {
            get { return _provider; }
        }

        public override bool AutoFlush { get; set; }

        public override string NewLine { get; set; }

        private void TryLeaveStreamOpen()
        {
            FieldInfo closable = ReflectionUtility.GetField(this.GetType(), "closable", ReflectionUtility.BindingInstancePublicAndPrivate);
            if (closable != null)
            {
                closable.SetValue(this, false);
            }
        }
    }
}