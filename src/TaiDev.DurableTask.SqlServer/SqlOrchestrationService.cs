using Microsoft.Data.SqlClient;
using System.Diagnostics;
using TaiDev.DurableTask.SqlServer.Utils;

namespace TaiDev.DurableTask.SqlServer;

public class SqlOrchestrationService : OrchestrationServiceBase
{
    readonly SqlOrchestrationServiceSettings settings;
    readonly LogHelper traceHelper;
    readonly SqlDbManager dbManager;
    readonly string lockedByValue;
    readonly string userId;

    public SqlOrchestrationService(SqlOrchestrationServiceSettings? settings)
    {
        this.settings = ValidateSettings(settings) ?? throw new ArgumentNullException(nameof(settings));
        this.traceHelper = new LogHelper(this.settings.LoggerFactory.CreateLogger("DurableTask.SqlServer"));
        this.dbManager = new SqlDbManager(this.settings, this.traceHelper);
        this.lockedByValue = $"{this.settings.AppName},{Process.GetCurrentProcess().Id}";
        this.userId = new SqlConnectionStringBuilder(this.settings.TaskHubConnectionString).UserID ?? string.Empty;
    }

    static SqlOrchestrationServiceSettings? ValidateSettings(SqlOrchestrationServiceSettings? settings)
    {
        if (settings != null)
        {
            if (string.IsNullOrEmpty(settings.TaskHubConnectionString))
            {
                throw new ArgumentException($"A non-empty connection string value must be provided.", nameof(settings));
            }

            if (settings.WorkItemLockTimeout < TimeSpan.FromSeconds(10))
            {
                throw new ArgumentException($"The {nameof(settings.WorkItemLockTimeout)} property value must be at least 10 seconds.", nameof(settings));
            }

            if (settings.WorkItemBatchSize < 10)
            {
                throw new ArgumentException($"The {nameof(settings.WorkItemBatchSize)} property value must be at least 10.", nameof(settings));
            }
        }

        return settings;
    }

    async Task<SqlConnection> GetAndOpenConnectionAsync(CancellationToken cancelToken = default)
    {
        if (cancelToken == default)
        {
            cancelToken = this.ShutdownToken;
        }

        SqlConnection connection = this.settings.CreateConnection();
        await connection.OpenAsync(cancelToken);
        return connection;
    }
}