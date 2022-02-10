-- Security
IF DATABASE_PRINCIPAL_ID('dt_runtime') IS NULL
BEGIN
    -- This is the role to which all low-privilege user accounts should be 
    CREATE ROLE dt_runtime
END

-- Each stored produce

-- Functions
GRANT EXECUTE ON OBJECT::dt.GetScaleMetric TO dt_runtime
GRANT EXECUTE ON OBJECT::dt.GetScaleRecommendation to dt_runtime

