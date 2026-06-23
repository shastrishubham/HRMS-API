

---UpsertEmpFeedback.Sql


--select * from PMS_Feedbacks

--DECLARE @Id                               INT = 1;
--DECLARE @MachineId                        NVARCHAR(50) = 'Id11'
--DECLARE @MachineIp                        NVARCHAR(50) = 'Ip'
--DECLARE @CompId                           UNIQUEIDENTIFIER = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680'
--DECLARE @FromEmployeeId                   UNIQUEIDENTIFIER = 'EA186978-58F9-4153-9863-DCD4E7AF4F18';
--DECLARE @ToEmployeeId                     UNIQUEIDENTIFIER = 'EA186978-58F9-4153-9863-DCD4E7AF4F18';
--DECLARE @MS_PerformanceCycles_CycleId     INT = 1;
--DECLARE @FeedbackType                     NVARCHAR(50) = ''
--DECLARE @FeedbackText                     NVARCHAR(MAX) = 'cmt';
--DECLARE @Rating                           DECIMAL(3,2) = 2
--DECLARE @CreatedBy                        UNIQUEIDENTIFIER = NEWID();
--DECLARE @ModifiedBy                       UNIQUEIDENTIFIER = NEWID();


MERGE PMS_Feedbacks AS TARGET
USING
(
    SELECT
         @Id                               AS Id
        ,@MachineId                        AS MachineId
        ,@MachineIp                        AS MachineIp
        ,@CompId                           AS CompId
        ,@MS_PerformanceCycles_CycleId     AS MS_PerformanceCycles_CycleId
        ,@FromEmployeeId                      AS FromEmployeeId
        ,@ToEmployeeId                        AS ToEmployeeId
        ,@FeedbackType                    AS FeedbackType
        ,@FeedbackText                    AS FeedbackText
        ,@Rating                  AS Rating
        ,@CreatedBy                        AS CreatedBy
        ,@ModifiedBy                       AS ModifiedBy
) AS SOURCE

ON TARGET.Id = SOURCE.Id

WHEN MATCHED THEN
    UPDATE SET
         TARGET.MS_PerformanceCycles_CycleId = SOURCE.MS_PerformanceCycles_CycleId
        ,TARGET.FromEmployeeId                  = SOURCE.FromEmployeeId
        ,TARGET.ToEmployeeId                    = SOURCE.ToEmployeeId
        ,TARGET.FeedbackType                = SOURCE.FeedbackType
        ,TARGET.FeedbackText                = SOURCE.FeedbackText
        ,TARGET.Rating              = SOURCE.Rating
        ,TARGET.ModifiedBy                   = SOURCE.ModifiedBy
        ,TARGET.ModifiedOn                   = GETDATE()

WHEN NOT MATCHED THEN
    INSERT
    (
       [FormDate]
      ,[MachineId]
      ,[MachineIp]
      ,[CompId]
      ,[FromEmployeeId]
      ,[ToEmployeeId]
      ,[MS_PerformanceCycles_CycleId]
      ,[FeedbackType]
      ,[FeedbackText]
      ,[Rating]
      ,[SubmittedDate]
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
        ,SOURCE.FromEmployeeId
        ,SOURCE.ToEmployeeId
        ,SOURCE.MS_PerformanceCycles_CycleId
        ,SOURCE.FeedbackType
        ,SOURCE.FeedbackText
        ,SOURCE.Rating
        ,GETDATE()
        ,SOURCE.CreatedBy
        ,GETDATE()
        ,SOURCE.ModifiedBy
        ,GETDATE()
    )

-- Optional:
OUTPUT inserted.Id;