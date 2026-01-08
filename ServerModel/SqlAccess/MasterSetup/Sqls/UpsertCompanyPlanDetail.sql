
/*
--UpsertCompanyPlanDetail.Sql

--select * from MS_CompPlan

DECLARE @Id int = 0;
DECLARE @CompId uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @MS_Module_Id int = 0;
DECLARE @ActiveFrom DATETIME() = GETDATE();
DECLARE @ActiveTo DATETIME() = GETDATE();
DECLARE @NosOfUser int = 0;
DECLARE @Active bit = 1;

*/

MERGE INTO MS_CompPlan as Target
USING (select @Id as Id, @AssetName as AssetName) AS source
    ON (Target.Id = source.Id) 

WHEN MATCHED THEN
	UPDATE SET
		 MS_Module_Id = @MS_Module_Id
		,ActiveFrom = @ActiveFrom
		,ActiveTo = @ActiveTo
		,[Active] = @Active


WHEN NOT MATCHED THEN
        INSERT (
			 FormDate
            ,CompId
			,MS_Module_Id
			,ActiveFrom
			,ActiveTo
			,Active)
        VALUES
           (
		    GETDATE()
           ,@CompId
		   ,@MS_Module_Id
		   ,@ActiveFrom
		   ,@ActiveTo
		   ,@Active)

OUTPUT INSERTED.Id;


