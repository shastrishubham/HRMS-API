

--UpsertHelpDeskTicketReplies.Sql

--select * from HR_HelpDeskReplies

--DECLARE @Id INT = 0;
--DECLARE @CompId uniqueidentifier = '00000000-0000-0000-0000-000000000000';
--DECLARE @HR_HelpDesk_Id INT = 4;
--DECLARE @RepliedByEmpId uniqueidentifier = '3E78B576-0307-48FC-8CC3-13D2C6C68A15';
--DECLARE @Message NVARCHAR(MAX) = 'tick 1-2';
--DECLARE @InterviewStatus NVARCHAR(50) = 'InProgress';



MERGE INTO HR_HelpDeskReplies as Target
USING (select @Id as Id) AS source 
    ON (Target.Id = source.Id) 

WHEN MATCHED THEN
	UPDATE SET
		 HR_HelpDesk_Id = @HR_HelpDesk_Id
		,RepliedByEmpId = @RepliedByEmpId
		,Message = @Message
		,ReplyDate = GETDATE()

WHEN NOT MATCHED THEN
        INSERT (
			 FormDate
			,CompId
            ,HR_HelpDesk_Id
			,RepliedByEmpId
			,Message
			,ReplyDate
			)
        VALUES
           (
		     GETDATE()
			,@CompId
            ,@HR_HelpDesk_Id
			,@RepliedByEmpId
			,@Message
			,GETDATE()
			)

OUTPUT INSERTED.Id;


-- ✅ Update HR_HelpDesk status to 'InProgress' only if current status is 'Open'
UPDATE HR_HelpDesk
SET TicketStatus = @InterviewStatus
WHERE Id = @HR_HelpDesk_Id


