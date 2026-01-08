
/*
--UpsertReimbursementTypes.Sql

--select * from MS_Reim_Types

DECLARE @Id int = 1;
DECLARE @CompId uniqueidentifier = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
DECLARE @ReimbursementType nvarchar(MAX) = 'name23';
DECLARE @MonthlyLimit DECIMAL(18,4)= 1;
DECLARE @Frequency nvarchar(50) = 'Monthly';
DECLARE @IsDesignationSpecific BIT = 1;
DECLARE @MS_Designation_Id INT = 1;

*/

MERGE INTO MS_Reim_Types as Target
USING (select @Id as Id, @ReimbursementType as ReimbursementType) AS source
    ON (Target.Id = source.Id) 

WHEN MATCHED THEN
	UPDATE SET
		 CompId = @CompId
		,ReimbursementType = @ReimbursementType
		,MonthlyLimit = @MonthlyLimit
		,Frequency = @Frequency
		,IsDesignationSpecific = @IsDesignationSpecific
		,MS_Designation_Id = @MS_Designation_Id


WHEN NOT MATCHED THEN
        INSERT (
			 CompId
            ,ReimbursementType
			,MonthlyLimit 
			,Frequency
			,IsDesignationSpecific
			,MS_Designation_Id)
        VALUES
           (
		    @CompId
           ,@ReimbursementType
		   ,@MonthlyLimit 
		   ,@Frequency
		   ,ISNULL(@IsDesignationSpecific, 0)
		   ,ISNULL(@MS_Designation_Id, 0))

OUTPUT INSERTED.Id;


