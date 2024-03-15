using Dfinance.LogViewer;
using Newtonsoft.Json;
using Serilog.Events;
using Serilog.Formatting;

public class LogFormatterService : ITextFormatter
{
    public void Format(LogEvent logEvent, TextWriter output)
    {
        if (logEvent.Level != LogEventLevel.Error)
        {
            return;
        }
        if (logEvent != null)
        {
            List<LogEntryModel> logs = new List<LogEntryModel>
        {
            new LogEntryModel { Timestamp = logEvent?.Timestamp.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK"),
            Level = logEvent.Level.ToString(),
            MessageTemplate = logEvent.MessageTemplate.Text,
            Exception = logEvent?.Exception?.ToString(),
            Properties = logEvent?.Properties?.ToDictionary(p => p.Key, p => p.Value?.ToString()),}
        };

            var json = JsonConvert.SerializeObject(logs, Formatting.Indented);
            output.Write($"{json}");
        }

    }
}
