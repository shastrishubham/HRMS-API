
/*
--UpsertAsset.Sql

--select * from MS_Asset

DECLARE @Id int = 1;
DECLARE @CompId uniqueidentifier = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
DECLARE @AssetName nvarchar(MAX) = 'name23';
DECLARE @IsSubmitToBack bit = 1;
DECLARE @Active bit = 1;

*/

MERGE INTO MS_Asset as Target
USING (select @Id as Id, @AssetName as AssetName) AS source
    ON (Target.Id = source.Id) 

WHEN MATCHED THEN
	UPDATE SET
		 AssetName = @AssetName
		,IsSubmitToBack = @IsSubmitToBack
		,[Active] = @Active


WHEN NOT MATCHED THEN
        INSERT (
             CompId
			,AssetName
			,IsSubmitToBack
			,Active)
        VALUES
           (
            @CompId
		   ,@AssetName
		   ,@IsSubmitToBack
		   ,@Active)

OUTPUT INSERTED.Id;


