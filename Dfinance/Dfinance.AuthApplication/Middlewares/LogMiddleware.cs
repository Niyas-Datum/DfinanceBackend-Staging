using Microsoft.AspNetCore.Http;
using System.IO.Compression;

namespace Dfinance.AuthApplication.Middlewares
{
    public class LogMiddleware
    {
        private readonly string? _serilogLogFilePath;
        private readonly RequestDelegate _next;
        public LogMiddleware(string logFilePath, RequestDelegate next)
        {
            _serilogLogFilePath = logFilePath;
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            await CompressAndClearLogFile();
            await _next(context);
        }

        public async Task CompressAndClearLogFile()
        {
            try
            {
                var logDirectory = Path.GetDirectoryName(_serilogLogFilePath);
                var currentYear = DateTime.Now.Year;
                var currentDate = DateTime.Now.Date;
                var logFiles = Directory.GetFiles(logDirectory, "*.json")
                    .Where(file => File.GetCreationTime(file).Year == currentYear)
                    .Where(file => Path.GetFileNameWithoutExtension(file) != DateTime.Now.ToString("yyyyMMdd"))
                    .ToList();

                // Check if the current dated log file exists and is not for the current day
                var currentLogFile = Path.Combine(logDirectory, $"{DateTime.Now:yyyyMMdd}.json");
                if (File.Exists(currentLogFile) && File.GetLastWriteTime(currentLogFile).Date < DateTime.Now.Date)
                {
                    logFiles.Add(currentLogFile);
                }

                if (logFiles.Any())
                {
                    var zipFilePath = Path.Combine(logDirectory, $"{currentYear}.zip");

                    using (var stream = File.Open(zipFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    using (var archive = new ZipArchive(stream, ZipArchiveMode.Update))
                    {
                        foreach (var logFile in logFiles)
                        {
                            if (File.Exists(logFile))
                            {
                                var logContent = File.ReadAllText(logFile);
                                var entry = archive.CreateEntry(Path.GetFileName(logFile), CompressionLevel.Optimal);
                                using (var entryStream = entry.Open())
                                using (var writer = new StreamWriter(entryStream))
                                {
                                    writer.Write(logContent);
                                }
                            }
                        }
                    }

                    // Delete the log files that were processed
                    foreach (var logFile in logFiles)
                    {
                        if (File.Exists(logFile))
                        {
                            File.Delete(logFile);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception
            }
        }


    }
}

