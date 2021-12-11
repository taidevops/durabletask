using Microsoft.Extensions.Logging;

namespace TaiDev.DurableTask.SqlServer;

class LogHelper
{
    readonly ILogger log;

    public LogHelper(ILogger log)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
    }


}
