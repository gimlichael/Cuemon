using System;
using System.Text;
using System.Threading;

namespace Cuemon.Threading
{
    internal class ThreadInfo
    {
        internal ThreadInfo(Thread thread = null)
        {
            Thread = thread ?? Thread.CurrentThread;
        }

        private Thread Thread { get; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            try
            {
                builder.Append(FormattableString.Invariant($"Culture: {Thread.CurrentCulture.ToString()}"));
                builder.Append("^");
                builder.Append(FormattableString.Invariant($"UICulture: {Thread.CurrentUICulture.ToString()}"));
                builder.Append("^");
                builder.Append(FormattableString.Invariant($"ManagedId: {Thread.ManagedThreadId.ToString()}"));
                builder.Append("^");
                builder.Append(FormattableString.Invariant($"Name: {Thread.Name ?? "null"}"));
                builder.Append("^");
                builder.Append(FormattableString.Invariant($"State: {Thread.ThreadState}"));
                builder.Append("^");
                builder.Append(FormattableString.Invariant($"Priority: {Thread.Priority}"));
                builder.Append("^");
                builder.Append(FormattableString.Invariant($"IsThreadPoolThread: {Thread.IsThreadPoolThread.ToString()}"));
                builder.Append("^");
                builder.Append(FormattableString.Invariant($"IsBackground: {Thread.IsBackground.ToString()}"));
            }
            catch (Exception)
            {
                // ignore platform exceptions and the likes hereof
            }
            return builder.ToString();
        }
    }
}