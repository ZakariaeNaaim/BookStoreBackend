using Serilog;

namespace WebApi.Extensions
{
    public static class SerilogExtensions
    {
        public static void ConfigureSerilog()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}

