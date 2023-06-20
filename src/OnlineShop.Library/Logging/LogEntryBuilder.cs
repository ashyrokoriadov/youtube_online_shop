namespace OnlineShop.Library.Logging
{
    public static class LogEntryBuilder
    {
        public static LogEntry WithClass(this LogEntry logEntry, string @class)
        {
            logEntry.Class = @class;
            return logEntry;
        }

        public static LogEntry WithMethod(this LogEntry logEntry, string method)
        {
            logEntry.Method = method;
            return logEntry;
        }

        public static LogEntry WithComment(this LogEntry logEntry, string comment)
        {
            logEntry.Comment = comment;
            return logEntry;
        }

        public static LogEntry WithOperation(this LogEntry logEntry, string operation)
        {
            logEntry.Operation = operation;
            return logEntry;
        }

        public static LogEntry WithParameters(this LogEntry logEntry, string parameters)
        {
            logEntry.Parameters = parameters;
            return logEntry;
        }
    }
}
