using Microsoft.Data.SqlClient;
using System.Reflection;

namespace TaiDev.DurableTask.SqlServer;

class SqlDbManager
{
    readonly SqlOrchestrationServiceSettings settings;
    readonly LogHelper traceHelper;

    public SqlDbManager(SqlOrchestrationServiceSettings settings, LogHelper traceHelper)
    {
        this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        this.traceHelper = traceHelper ?? throw new ArgumentNullException(nameof(traceHelper));
    }

    async Task ExecuteSqlScriptAsync(string scriptName, DatabaseLock dbLock)
    {
        // We don't actually use the lock here, but want to make sure the caller is holding it.
        if (dbLock == null)
        {
            throw new ArgumentNullException(nameof(dbLock));
        }

        if (!dbLock.IsHeld)
        {
            throw new ArgumentException("This database lock has already been released!", nameof(dbLock));
        }

        string schemaCommands = await GetScriptTextAsync(scriptName);

        using SqlConnection scriptRunnerConnection = 
    }

    static Task<string> GetScriptTextAsync(string scriptName, Assembly? assembly = null)
    {
        if (assembly == null)
        {
            assembly = typeof(SqlOrchestrationService).Assembly;
        }

        string assemblyName = assembly.GetName().Name!;
        if (!scriptName.StartsWith(assemblyName))
        {
            scriptName = $"{assembly.GetName().Name}.Scripts.{scriptName}";
        }

        using Stream? resourceStream = assembly.GetManifestResourceStream(scriptName);
        if (resourceStream == null)
        {
            throw new ArgumentException($"Could not find assembly resource named '{scriptName}'.");
        }

        using var reader = new StreamReader(resourceStream);
        return reader.ReadToEndAsync();
    }

    sealed class DatabaseLock : IAsyncDisposable
    {
        readonly SqlConnection connection;
        readonly SqlTransaction transaction;

        bool committed;

        public DatabaseLock(SqlConnection connection, SqlTransaction transaction)
        {
            this.connection = connection;
            this.transaction = transaction;
        }

        public bool IsHeld => !this.committed;

        public SqlCommand CreateCommand()
        {
            SqlCommand command = this.connection.CreateCommand();
            command.Transaction = this.transaction;
            return command;
        }

        public Task CommitAsync()
        {
            this.committed = true;
            return this.transaction.CommitAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (!this.committed)
            {
                await this.transaction.RollbackAsync();
            }

            await this.connection.CloseAsync();
        }
    }
}
