using System;
using System.IO;
using System.Threading;

namespace Sim2.Helper
{
    public class Log
    {
        private readonly string _filePath;
        private readonly object _lockObject = new object();

        public Log()
        {
            var directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            _filePath = Path.Combine(directory, "log.txt");
        }
        public void WriteUrlLog(string url)
        {
            using (var stream = new FileStream(_filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                lock (_lockObject)
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.WriteLine($"{DateTime.Now} - URL: {url}");
                    }
                }
            }
        }
        public void WriteResponseLog(string response)
        {
            using (var stream = new FileStream(_filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                lock (_lockObject)
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.WriteLine($"{DateTime.Now} - Response: {response}");
                    }
                }
            }
        }
        public void WriteErrorLog(Exception ex)
        {
            using (var stream = new FileStream(_filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                lock (_lockObject)
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.WriteLine($"Error: {ex.Message}");
                        writer.WriteLine($"Stack trace: {ex.StackTrace}");
                        writer.WriteLine($"Source: {ex.Source}");
                        writer.WriteLine($"Target: {ex.TargetSite}");
                    }
                }
            }
        }
        public void WriteRetryLog(int retryCount, int retryDelayInSeconds)
        {
            using (var stream = new FileStream(_filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                lock (_lockObject)
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.WriteLine($"Retry {retryCount} after {retryDelayInSeconds} seconds...");
                        Thread.Sleep(retryDelayInSeconds * 1000);
                    }
                }
            }
        }
    }
}