

--DECLARE @CheckInId                 INT = 0;
--DECLARE @MachineId                 NVARCHAR(50) = 'ID';
--DECLARE @MachineIp                 NVARCHAR(50) = 'Ip';
--DECLARE @CompId                    UNIQUEIDENTIFIER = NEWID();
--DECLARE @PMS_EmployeeGoals_GoalId  INT = 5;
--DECLARE @ProgressPercentage        DECIMAL(5,2) = 22;
--DECLARE @Comments                  NVARCHAR(MAX) = 'test';
--DECLARE @AttachmentPath            NVARCHAR(MAX) = 'test';
--DECLARE @Active                    BIT = 1;
--DECLARE @CreatedBy                 UNIQUEIDENTIFIER = NEWID();
--DECLARE @ModifiedBy                UNIQUEIDENTIFIER = NEWID();

MERGE PMS_PerformanceCheckIns AS TARGET
USING
(
    SELECT
         @CheckInId                    AS CheckInId
        ,@MachineId                    AS MachineId
        ,@MachineIp                    AS MachineIp
        ,@CompId                       AS CompId
        ,@PMS_EmployeeGoals_GoalId     AS PMS_EmployeeGoals_GoalId
        ,@ProgressPercentage           AS ProgressPercentage
        ,@Comments                     AS Comments
        ,@AttachmentPath               AS AttachmentPath
        ,@Active                       AS Active
        ,@CreatedBy                    AS CreatedBy
        ,@ModifiedBy                   AS ModifiedBy
) AS SOURCE
ON TARGET.CheckInId = SOURCE.CheckInId

WHEN MATCHED THEN
    UPDATE SET
         TARGET.PMS_EmployeeGoals_GoalId = SOURCE.PMS_EmployeeGoals_GoalId
        ,TARGET.ProgressPercentage       = SOURCE.ProgressPercentage
        ,TARGET.Comments                 = SOURCE.Comments
        ,TARGET.AttachmentPath           = SOURCE.AttachmentPath
        ,TARGET.Active                   = SOURCE.Active
        ,TARGET.ModifiedBy               = SOURCE.ModifiedBy
        ,TARGET.ModifiedOn               = GETDATE()

WHEN NOT MATCHED THEN
    INSERT
    (
         [FormDate]
        ,[MachineId]
        ,[MachineIp]
        ,[CompId]
        ,[PMS_EmployeeGoals_GoalId]
        ,[ProgressPercentage]
        ,[Comments]
        ,[AttachmentPath]
        ,[Active]
        ,[CreatedBy]
        ,[CreatedOn]
        ,[ModifiedBy]
        ,[ModifiedOn]
    )
    VALUES
    (
         GETDATE()
        ,SOURCE.MachineId
        ,SOURCE.MachineIp
        ,SOURCE.CompId
        ,SOURCE.PMS_EmployeeGoals_GoalId
        ,SOURCE.ProgressPercentage
        ,SOURCE.Comments
        ,SOURCE.AttachmentPath
        ,CAST(1 AS BIT)
        ,SOURCE.CreatedBy
        ,GETDATE()
        ,SOURCE.ModifiedBy
        ,GETDATE()
    )

-- Optional:
OUTPUT inserted.CheckInId;