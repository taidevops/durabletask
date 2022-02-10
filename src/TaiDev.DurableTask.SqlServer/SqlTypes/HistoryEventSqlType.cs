
namespace TaiDev.DurableTask.SqlServer;

static class HistoryEventSqlType
{
    static readonly SqlMetaData[] HistoryEventSchema = new SqlMetaData[]
    {
        new SqlMetaData("InstanceID", SqlDbType.Varchar, 100),
        new SqlMetaData("ExecutionID", SqlDbType.Varchar, 50),
    };

    static class ColumnOrdinals
    {
        public const int InstanceID = 0;
        public const int ExecutionID = 1;
    };

    public static SqlParameter AddHistoryEventsParameter(
        this SqlParameterCollection commandParameters,
        string paramName,
        IEnumerable<HistoryEvent> newEventCollection,
        
    )
}