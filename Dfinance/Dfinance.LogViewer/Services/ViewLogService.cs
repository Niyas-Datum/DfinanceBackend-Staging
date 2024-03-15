using Dfinance.LogViewer.Services.Interface;
using Dfinance.Shared.Domain;
using Newtonsoft.Json;
using System.IO.Compression;

namespace Dfinance.LogViewer.Services
{
    public class ViewLogService : IViewLogService
    {
        private readonly string? _serilogLogFilePath;
        public ViewLogService(IConfiguration configuration)
        {
            _serilogLogFilePath = configuration["Serilog:WriteTo:0:Args:Path"];
        }

        public CommonResponse ViewLogs(DateOnly date, string method = null)
        {
            try
            {
                var logDirectory = Path.GetDirectoryName(_serilogLogFilePath);
                string logFilePath;
                if (date == default(DateOnly))
                {
                    date = DateOnly.FromDateTime(DateTime.Today);
                }
                var formattedDate = date.ToString("yyyyMMdd");

                logFilePath = Path.Combine(logDirectory, $"{formattedDate}.json");
                if (File.Exists(logFilePath))
                {
                    var logsContent = File.ReadAllText(logFilePath);
                    //var readline = File.ReadAllLines(logFilePath);
                    var modifiedLogs = logsContent.Replace("][", ",");

                    var logObjects = JsonConvert.DeserializeObject<List<LogEntryModel>>(modifiedLogs);

                    if (string.IsNullOrEmpty(method))
                    {
                        return CommonResponse.Ok(logObjects);
                    }
                    else
                    {
                        var filteredLogs = logObjects.Where(log => log.Level == method).ToList();
                        return CommonResponse.Ok(filteredLogs);
                    }
                }

                else
                {
                    // Check if the log file exists inside a ZIP file
                    var currentYear = date.Year;
                    string zipFilePath = Path.Combine(logDirectory, $"{currentYear}.zip");
                    if (File.Exists(zipFilePath))
                    {
                        using (ZipArchive archive = ZipFile.OpenRead(zipFilePath))
                        {
                            var logFileEntry = archive.GetEntry($"{formattedDate}.json");
                          
                            if (logFileEntry != null)
                            {

                                using (StreamReader reader = new StreamReader(logFileEntry.Open()))
                                {
                                    var logsContent1 = reader.ReadToEnd();
                                    var modifiedLogs = logsContent1.Replace("][", ",");
                                    var logObjects = JsonConvert.DeserializeObject<List<LogEntryModel>>(modifiedLogs);
                                    if (string.IsNullOrEmpty(method))
                                    {
                                        return CommonResponse.Ok(logObjects);
                                    }
                                    else
                                    {
                                        var filteredLogs = logObjects.Where(log => log.Level == method).ToList();
                                        return CommonResponse.Ok(filteredLogs);
                                    }
                                }
                            }
                        }
                    }
                    return CommonResponse.NotFound("Log file not found for the given date!");
                }
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }
    }
}














































