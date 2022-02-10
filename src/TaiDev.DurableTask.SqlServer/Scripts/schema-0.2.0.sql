
IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'dt')
    EXEC('CREATE SCHEMA dt');

-- CREATE custom types
IF TYPE_ID(N'dt.InstanceIDs') IS NULL
    CREATE TYPE dt.InstanceIDs AS TABLE (
        [InstanceID] varchar(100) NOT NULL
    )
GO

IF TYPE_ID(N'dt.HistoryEvents') IS NULL
    -- WARNING: Reordering fields is a breaking change!
    CREATE TYPE dt.HistoryEvents AS TABLE (
        [InstanceID] varchar(100) NULL,
    )