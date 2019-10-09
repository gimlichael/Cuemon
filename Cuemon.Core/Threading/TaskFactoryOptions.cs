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
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="AsyncOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="PartitionSize"/></term>
        ///         <description>2 x <see cref="Environment.ProcessorCount"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Scheduler"/></term>
        ///         <description><see cref="TaskScheduler.Current"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="CreationOptions"/></term>
        ///         <description><see cref="TaskCreationOptions.LongRunning"/></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public TaskFactoryOptions()
        {
            CreationOptions = TaskCreationOptions.LongRunning;
            Scheduler = TaskScheduler.Current;
            PartitionSize = 2 * Environment.ProcessorCount;
        }

        /// <summary>
        /// Gets or sets the size of the partition to allocate work to a set of tasks.
        /// </summary>
        /// <value>The size of the partition to allocate work to a set of tasks.</value>
        public int PartitionSize { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="TaskCreationOptions"/> used to create the task.
        /// </summary>
        /// <value>The <see cref="TaskCreationOptions"/> used to create the task.</value>
        public TaskCreationOptions CreationOptions { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="TaskScheduler"/> that is used to schedule the task.
        /// </summary>
        /// <value>The <see cref="TaskScheduler"/> that is used to schedule the task.</value>
        public TaskScheduler Scheduler { get; set; }
    }
}