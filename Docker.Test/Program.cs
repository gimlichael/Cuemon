using System;
using System.Runtime.InteropServices;
using Cuemon;
using Cuemon.Extensions.Diagnostics;
using Cuemon.Reflection;

namespace Docker.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                throw new ApplicationException("Test");
            }
            catch (Exception e)
            {
                ExceptionInsights.Embed(e, snapshot: SystemSnapshot.CaptureAll);
                Console.WriteLine(e);
                Console.WriteLine("***");
                var ed = ExceptionInsights.ToExceptionDescriptor(e);
                Console.WriteLine(ed.ToInsightsString());
            }
        }
    }
}
