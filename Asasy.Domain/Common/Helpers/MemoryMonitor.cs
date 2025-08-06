using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace Asasy.Domain.Common.Helpers
{
    public class MemoryMonitor : IMemoryMonitor
    {
        private readonly ILogger<MemoryMonitor> _logger;
        private const long HIGH_MEMORY_THRESHOLD = 1024 * 1024 * 1024; // 1GB

        public MemoryMonitor(ILogger<MemoryMonitor> logger)
        {
            _logger = logger;
        }

        public long GetCurrentMemoryUsage()
        {
            var process = Process.GetCurrentProcess();
            return process.WorkingSet64;
        }

        public void LogMemoryUsage(string operation)
        {
            var memoryUsage = GetCurrentMemoryUsage();
            var memoryUsageMB = memoryUsage / (1024 * 1024);
            
            _logger.LogInformation($"Memory usage after {operation}: {memoryUsageMB} MB");
            
            if (IsMemoryUsageHigh())
            {
                _logger.LogWarning($"High memory usage detected: {memoryUsageMB} MB");
            }
        }

        public bool IsMemoryUsageHigh()
        {
            return GetCurrentMemoryUsage() > HIGH_MEMORY_THRESHOLD;
        }
    }
}