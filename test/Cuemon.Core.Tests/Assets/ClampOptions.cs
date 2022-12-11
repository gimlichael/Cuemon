using System;

namespace Cuemon.Assets
{
    public class ClampOptions
    {
        private int _maxConcurrentJobs;

        public ClampOptions()
        {
            MaxConcurrentJobs = 10;
        }

        public int MaxConcurrentJobs
        {
            get => _maxConcurrentJobs;
            set => _maxConcurrentJobs = Math.Clamp(value, 1, byte.MaxValue);
        }
    }
}
