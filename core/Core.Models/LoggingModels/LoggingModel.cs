namespace Core.Models.LoggingModels
{
    public static class LogLevelEnum
    {
        public static string Info { get; set; }
        public static string Error { get; set; }
    }

    public class LoggingModel
    {
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public string LogLevel { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public object Context { get; set; }
    }
}
