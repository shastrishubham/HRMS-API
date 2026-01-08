

--UpsetLoanTypes.Sql
/*
--select * from MS_LN

DECLARE @Id int = 0;
DECLARE @CompId uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @LNTypeName nvarchar(255) = 'name2';
DECLARE @InterestRate DECIMAL(18, 4) = 10;
DECLARE @IsMaxAmtManual BIT = 1;
DECLARE @MaxAmount DECIMAL(18, 4) = 100;
DECLARE @MS_SLHead_Id INT = 1;
DECLARE @Percentage DECIMAL(18, 4) = 0;
DECLARE @TenureMonths DECIMAL(18, 0) = 12;
DECLARE @AllowPartialPay BIT = 1;

**/



MERGE INTO MS_LN as Target
USING (select @Id as Id, @LNTypeName as LNTypeName) AS source
    ON (Target.Id = source.Id) 

WHEN MATCHED THEN
	UPDATE SET 
		 LNTypeName = @LNTypeName
		,InterestRate = @InterestRate
		,IsMaxAmtManual = @IsMaxAmtManual
		,MaxAmount = @MaxAmount
		,MS_SLHead_Id = @MS_SLHead_Id
		,Percentage = @Percentage
		,TenureMonths = @TenureMonths
		,AllowPartialPay = @AllowPartialPay


WHEN NOT MATCHED THEN
        INSERT (
             CompId
			,LNTypeName
			,InterestRate
			,IsMaxAmtManual
			,MaxAmount
			,MS_SLHead_Id
			,Percentage
			,TenureMonths
			,AllowPartialPay)
        VALUES
           (
            @CompId
		   ,@LNTypeName
		   ,@InterestRate
		   ,@IsMaxAmtManual
		   ,@MaxAmount
		   ,@MS_SLHead_Id
		   ,@Percentage
		   ,@TenureMonths
		   ,@AllowPartialPay)

OUTPUT INSERTED.Id;


