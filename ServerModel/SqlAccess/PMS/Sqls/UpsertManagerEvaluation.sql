
--select * from PMS_ManagerEvaluations

--DECLARE @Id                               INT = 1;
--DECLARE @MachineId                        NVARCHAR(50) = 'Id'
--DECLARE @MachineIp                        NVARCHAR(50) = 'Ip'
--DECLARE @CompId                           UNIQUEIDENTIFIER = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680'
--DECLARE @EMP_Info_Id                      UNIQUEIDENTIFIER = 'EA186978-58F9-4153-9863-DCD4E7AF4F18';
--DECLARE @ManagerId                        UNIQUEIDENTIFIER = 'EA186978-58F9-4153-9863-DCD4E7AF4F18';
--DECLARE @MS_PerformanceCycles_CycleId     INT = 1;
--DECLARE @OverallRating                    DECIMAL(3,2) = 3.2
--DECLARE @OverallComments                  NVARCHAR(MAX) = 'cmt';
--DECLARE @Status                           NVARCHAR(50) = 'Draft'
--DECLARE @SubmittedDate                    DATETIME2= GETDATE();
--DECLARE @Active                           BIT = 1;
--DECLARE @CreatedBy                        UNIQUEIDENTIFIER = NEWID();
--DECLARE @ModifiedBy                       UNIQUEIDENTIFIER = NEWID();


MERGE PMS_ManagerEvaluations AS TARGET
USING
(
    SELECT
         @Id                               AS Id
        ,@MachineId                        AS MachineId
        ,@MachineIp                        AS MachineIp
        ,@CompId                           AS CompId
        ,@MS_PerformanceCycles_CycleId     AS MS_PerformanceCycles_CycleId
        ,@EMP_Info_Id                      AS EMP_Info_Id
        ,@ManagerId                        AS ManagerId
        ,@OverallRating                    AS OverallRating
        ,@SubmittedDate                    AS SubmittedDate
        ,@OverallComments                  AS OverallComments
        ,@Status                           AS Status
        ,@Active                           AS Active
        ,@CreatedBy                        AS CreatedBy
        ,@ModifiedBy                       AS ModifiedBy
) AS SOURCE

ON TARGET.Id = SOURCE.Id

WHEN MATCHED THEN
    UPDATE SET
         TARGET.MS_PerformanceCycles_CycleId = SOURCE.MS_PerformanceCycles_CycleId
        ,TARGET.EMP_Info_Id                  = SOURCE.EMP_Info_Id
        ,TARGET.ManagerId                    = SOURCE.ManagerId
        ,TARGET.OverallRating                = SOURCE.OverallRating
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
         ,[EMP_Info_Id]
         ,[ManagerId]
         ,[MS_PerformanceCycles_CycleId]
         ,[OverallRating]
         ,[OverallComments]
         ,[SubmittedDate]
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
        ,SOURCE.EMP_Info_Id
        ,SOURCE.ManagerId
        ,SOURCE.MS_PerformanceCycles_CycleId
        ,SOURCE.OverallRating
        ,SOURCE.OverallComments
        ,SOURCE.SubmittedDate
        ,SOURCE.Status
        ,CAST(1 AS BIT)
        ,SOURCE.CreatedBy
        ,GETDATE()
        ,SOURCE.ModifiedBy
        ,GETDATE()
    )

-- Optional:
OUTPUT inserted.Id;