using System;
using System.Threading.Tasks;

namespace Cuemon.Threading
{
    /// <summary>
    /// Configuration options for <see cref="TaskFactory"/>.
    /// </summary>
    public class TaskFactoryOptions : AsyncOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskFactoryOptions"/> class.
        /// </summary>
        public TaskFactoryOptions()
        {
            CreationOptions = TaskCreationOptions.LongRunning;
            Scheduler = TaskScheduler.Current;
            ChunkSize = 2 * Environment.ProcessorCount;
        }

        public int ChunkSize { get; set; }

        public TaskCreationOptions CreationOptions { get; set; }

        public TaskScheduler Scheduler { get; set; }
    }
}