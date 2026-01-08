

/*

--UpsertSalaryHeads.Sql

--select * from MS_Leave

DECLARE @Id int = 1;
DECLARE @CompId uniqueidentifier = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
DECLARE @SalaryHeadName nvarchar(255) = 'name23';
DECLARE @ShortSalaryHeadName nvarchar(50) = 'name23';
DECLARE @IsEarningComponent BIT = 1;
DECLARE @IsFixedComponent BIT = 1;
DECLARE @IsTaxableComponent BIT = 1;
DECLARE @IsShowInSalarySlip BIT = 1;
DECLARE @SalaryHeadOrder BIT =999;
DECLARE @MS_SLHeads_Id INT = 1;
DECLARE @Percentage DECIMAL(18, 10) = 40;
DECLARE @Active bit = 1;


*/


MERGE INTO MS_SLHeads as Target
USING (select @Id as Id, @SalaryHeadName as SalaryHeadName) AS source
    ON (Target.Id = source.Id) 

WHEN MATCHED THEN
	UPDATE SET
		 CompId = @CompId
		,SalaryHeadName = @SalaryHeadName 
		,ShortSalaryHeadName = @ShortSalaryHeadName
		,IsEarningComponent = @IsEarningComponent
		,IsFixedComponent = @IsFixedComponent
		,IsTaxableComponent = @IsTaxableComponent
		,IsShowInSalarySlip = @IsShowInSalarySlip
		,SalaryHeadOrder = @SalaryHeadOrder
		,MS_SLHeads_Id = @MS_SLHeads_Id
		,[Percentage] = @Percentage
		,[Active] = @Active


WHEN NOT MATCHED THEN
        INSERT (
             CompId
			,SalaryHeadName
			,ShortSalaryHeadName
			,IsEarningComponent
			,IsFixedComponent
			,IsTaxableComponent
			,IsShowInSalarySlip
			,SalaryHeadOrder
			,MS_SLHeads_Id
			,[Percentage]
			,Active)
        VALUES
           (
            @CompId
		   ,@SalaryHeadName
		   ,@ShortSalaryHeadName
		   ,@IsEarningComponent
		   ,@IsFixedComponent
		   ,@IsTaxableComponent
		   ,@IsShowInSalarySlip
		   ,@SalaryHeadOrder
		   ,@MS_SLHeads_Id
		   ,@Percentage
		   ,@Active)

OUTPUT INSERTED.Id;


