using System.IO;

namespace Cuemon.IO
{
    internal static class StreamDecoratorExtensions
    {
        internal static void CopyStreamCore(this IDecorator<Stream> decorator, Stream destination, int bufferSize = 81920, bool changePosition = true)
        {
            var source = decorator.Inner;
            long lastPosition = 0;
            if (changePosition && source.CanSeek)
            {
                lastPosition = source.Position;
                if (source.CanSeek) { source.Position = 0; }
            }

            source.CopyTo(destination, bufferSize);
            destination.Flush();

            if (changePosition && source.CanSeek) { source.Position = lastPosition; }
            if (changePosition && destination.CanSeek) { destination.Position = 0; }
        }

        internal static byte[] ToByteArrayCore(this IDecorator<Stream> decorator, int bufferSize = 81920, bool leaveOpen = false)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            Validator.ThrowIfFalse(decorator.Inner.CanRead, nameof(decorator.Inner), "Stream cannot be read from.");
            try
            {
                if (decorator.Inner is MemoryStream s)
                {
                    return s.ToArray();
                }

                using (var memoryStream = new MemoryStream(new byte[decorator.Inner.Length]))
                {
                    var oldPosition = decorator.Inner.Position;
                    if (decorator.Inner.CanSeek)
                    {
                        decorator.Inner.Position = 0;
                    }

                    decorator.Inner.CopyTo(memoryStream, bufferSize);
                    if (decorator.Inner.CanSeek)
                    {
                        decorator.Inner.Position = oldPosition;
                    }

                    return memoryStream.ToArray();
                }
            }
            finally
            {
                if (!leaveOpen)
                {
                    decorator.Inner.Dispose();
                }
            }
        }
    }
}