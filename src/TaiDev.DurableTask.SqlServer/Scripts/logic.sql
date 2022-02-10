CREATE OR ALTER FUNCTION dt.CurrentTaskHub()
    RETURNS varchar(50)
    WITH EXECUTE AS CALLER
AS
BEGIN
    -- Task Hub modes:
    -- 0: Task hub names are set by the app
    -- 1: Task hub names are inferred from the user credential
    DECLARE @taskHubMode sql_variant = (SELECT TOP 1 [Value] FROM dt.GlobalSettings WHERE [Name] = 'TaskHubMode');

    DECLARE @taskHub varchar(150)

    IF @taskHubMode = 0
        SET @taskHub = APP_NAME()
    IF @taskHubMode = 1
        SET @taskHub = USER_NAME()

    IF @taskHub IS NULL
        SET @taskHub = 'default'

    -- if the name is too long, keep the first 16 characters and hash the rest
    IF LEN(@taskHub) > 50
        SET @taskHub = CONVERT(varchar(16), @taskHub) + '__' + CONVERT(varchar(32), HASHBYTES('MD5', @taskHub), 2)

    RETURN @taskHub
END
GO

