using System;

namespace Asasy.Domain.Common.Helpers
{
    public interface IMemoryMonitor
    {
        long GetCurrentMemoryUsage();
        void LogMemoryUsage(string operation);
        bool IsMemoryUsageHigh();
    }
}