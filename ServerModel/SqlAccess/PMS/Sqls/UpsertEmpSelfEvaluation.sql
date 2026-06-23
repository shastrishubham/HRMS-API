
--select * from PMS_SelfEvaluations

--DECLARE @SelfEvaluationId                 INT = 0;
--DECLARE @MachineId                        NVARCHAR(50) = 'Id'
--DECLARE @MachineIp                        NVARCHAR(50) = 'Ip'
--DECLARE @CompId                           UNIQUEIDENTIFIER = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680'
--DECLARE @MS_PerformanceCycles_CycleId     INT = 1;
--DECLARE @EMP_Info_Id                      UNIQUEIDENTIFIER = 'EA186978-58F9-4153-9863-DCD4E7AF4F18';
--DECLARE @SubmittedDate                    DATETIME2 = GETDATE();
--DECLARE @OverallComments                  NVARCHAR(MAX) = 'cmt';
--DECLARE @Status                           NVARCHAR(50) = 'Draft'
--DECLARE @Active                           BIT = 1;
--DECLARE @CreatedBy                        UNIQUEIDENTIFIER = NEWID();
--DECLARE @ModifiedBy                       UNIQUEIDENTIFIER = NEWID();


MERGE [HRMS].[dbo].[PMS_SelfEvaluations] AS TARGET
USING
(
    SELECT
         @SelfEvaluationId                 AS SelfEvaluationId
        ,@MachineId                        AS MachineId
        ,@MachineIp                        AS MachineIp
        ,@CompId                           AS CompId
        ,@MS_PerformanceCycles_CycleId     AS MS_PerformanceCycles_CycleId
        ,@EMP_Info_Id                      AS EMP_Info_Id
        ,@SubmittedDate                    AS SubmittedDate
        ,@OverallComments                  AS OverallComments
        ,@Status                           AS Status
        ,@Active                           AS Active
        ,@CreatedBy                        AS CreatedBy
        ,@ModifiedBy                       AS ModifiedBy
) AS SOURCE

ON TARGET.SelfEvaluationId = SOURCE.SelfEvaluationId

WHEN MATCHED THEN
    UPDATE SET
         TARGET.MS_PerformanceCycles_CycleId = SOURCE.MS_PerformanceCycles_CycleId
        ,TARGET.EMP_Info_Id                  = SOURCE.EMP_Info_Id
        ,TARGET.SubmittedDate                = SOURCE.SubmittedDate
        ,TARGET.OverallComments              = SOURCE.OverallComments
        ,TARGET.Status                       = SOURCE.Status
        ,TARGET.Active                       = SOURCE.Active
        ,TARGET.ModifiedBy                   = SOURCE.ModifiedBy
        ,TARGET.ModifiedOn                   = GETDATE()

WHEN NOT MATCHED THEN
    INSERT
    (
         [FormDate]
        ,[MachineId]
        ,[MachineIp]
        ,[CompId]
        ,[MS_PerformanceCycles_CycleId]
        ,[EMP_Info_Id]
        ,[SubmittedDate]
        ,[OverallComments]
        ,[Status]
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
        ,SOURCE.MS_PerformanceCycles_CycleId
        ,SOURCE.EMP_Info_Id
        ,SOURCE.SubmittedDate
        ,SOURCE.OverallComments
        ,SOURCE.Status
        ,CAST(1 AS BIT)
        ,SOURCE.CreatedBy
        ,GETDATE()
        ,SOURCE.ModifiedBy
        ,GETDATE()
    )

-- Optional:
OUTPUT inserted.SelfEvaluationId;