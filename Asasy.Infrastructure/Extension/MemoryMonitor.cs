using System.Diagnostics;

namespace Asasy.Infrastructure.Extension
{
    public interface IMemoryMonitor
    {
        long GetCurrentMemoryUsage();
        void LogMemoryUsage(string context = "");
        bool IsMemoryUsageHigh();
    }

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

        public void LogMemoryUsage(string context = "")
        {
            var memoryUsage = GetCurrentMemoryUsage();
            var memoryUsageMB = memoryUsage / (1024 * 1024);
            
            _logger.LogInformation($"Memory Usage {context}: {memoryUsageMB} MB ({memoryUsage} bytes)");
            
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