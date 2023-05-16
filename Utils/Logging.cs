using Serilog;

namespace BeginSEO.Utils
{
    public static class Logging
    {
        private static ILogger logger;

        static Logging()
        {
            ConfigureLogger();
        }

        private static void ConfigureLogger()
        {
            logger = new LoggerConfiguration()
                .WriteTo.File("log/log.txt")
                .CreateLogger();
        }

        public static void Success(string message) => Info(message);
        public static void Info(string message)
        {
            logger.Information(message);
        }

        public static void Warning(string message)
        {
            logger.Warning(message);
        }

        public static void Error(string message)
        {
            logger.Error(message);
        }

        // 可以添加其他日志级别和自定义方法

        public static void CloseLogger()
        {
            Log.CloseAndFlush();
        }
    }
}
