
--UpsertInterviewRating.Sql

/*
DECLARE @Id int = 7;
DECLARE @compId uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @InterviewRate nvarchar(max) = 'name27';
DECLARE @Active bit = 1;

*/


MERGE INTO MS_InterviewRate as Target
USING (select @Id as Id, @InterviewRate as InterviewRate) AS source
    ON (Target.Id = source.Id) 

WHEN MATCHED THEN
	UPDATE SET 
		 InterviewRate = @InterviewRate
		,[Active] = @Active


WHEN NOT MATCHED THEN
        INSERT (
             CompId
			,InterviewRate
			,Active)
        VALUES
           (
            @compId
           ,@InterviewRate
           ,@Active)

OUTPUT INSERTED.Id;

