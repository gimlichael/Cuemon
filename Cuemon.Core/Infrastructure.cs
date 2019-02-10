﻿using System;
using System.IO;
using System.Reflection;

namespace Cuemon
{
    internal static class Infrastructure
    {
        public static int DefaultBufferSize = 2048;

        public static void WhileSourceReadDestinationWrite(Stream source, Stream destination, int bufferSize)
        {
            WhileSourceReadDestinationWrite(source, destination, bufferSize, false);
        }

        public static void WhileSourceReadDestinationWrite(Stream source, Stream destination, int bufferSize, bool changePosition)
        {
            long lastPosition = 0;
            if (changePosition && source.CanSeek)
            {
                lastPosition = source.Position;
                if (source.CanSeek) { source.Position = 0; }
            }

            byte[] buffer = new byte[bufferSize];
            int read;
            while ((read = source.Read(buffer, 0, buffer.Length)) != 0) { destination.Write(buffer, 0, read); }

            if (changePosition && source.CanSeek) { source.Position = lastPosition; }
            if (changePosition && destination.CanSeek) { destination.Position = 0; }
        }

        internal static MethodInfo ResolveDelegateInfo(Delegate wrapper, Delegate original)
        {
            if (original != null) { return original.GetMethodInfo(); }
            if (wrapper != null) { return wrapper.GetMethodInfo(); }
            return null;
        }
    }
}