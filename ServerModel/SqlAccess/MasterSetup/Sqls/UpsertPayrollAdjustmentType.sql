-- ========================================
-- Example: Upsert into MS_Payroll_AdjustType
-- ========================================

--- UpsertPayrollAdjustmentType.sql

 --DECLARE @Id INT = 2;
 --DECLARE @CompId UNIQUEIDENTIFIER = 'C225A53F-9ECB-4B9A-88DC-EAEF6115C2A4'
 --DECLARE @IsEarningHead BIT = 1;
 --DECLARE @AdjustmentType NVARCHAR(100) = 'Arrears';
 --DECLARE @PercentageAmt DECIMAL(18, 2) = 5;
 --DECLARE @MS_SLHeads_Id INT = 1;
 --DECLARE @IsRuleBased BIT = 1;

 
-- MERGE to insert or update
MERGE MS_Payroll_AdjustType AS target
USING (select @Id as Id, 
	@CompId as CompId, 
	@IsEarningHead AS IsEarningHead, 
	@AdjustmentType AS AdjustmentType,
	@PercentageAmt AS PercentageAmt,
	@MS_SLHeads_Id AS MS_SLHeads_Id,
	@IsRuleBased AS IsRuleBased) AS source
ON target.Id = source.Id
WHEN MATCHED THEN
    UPDATE SET
        IsEarningHead = source.IsEarningHead,
        AdjustmentType = source.AdjustmentType,
		PercentageAmt = source.PercentageAmt,
		MS_SLHeads_Id = source.MS_SLHeads_Id,
		IsRuleBased = source.IsRuleBased
WHEN NOT MATCHED BY TARGET THEN
    INSERT (CompId, IsEarningHead, AdjustmentType, IsRuleBased, PercentageAmt, MS_SLHeads_Id)
    VALUES (source.CompId, source.IsEarningHead, source.AdjustmentType, source.IsRuleBased, source.PercentageAmt, source.MS_SLHeads_Id)

OUTPUT INSERTED.Id;