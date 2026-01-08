
/*
--CreateBranch.Sql

--select * from MS_Branch

DECLARE @Id int = 2;
DECLARE @MachineId nvarchar(50) = '';
DECLARE @MachineIp nvarchar(50) = '';
DECLARE @CompId uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @BranchName nvarchar(255) = 'name2';
DECLARE @MailAlias nvarchar(250) = '';
DECLARE @TimeZone_Id int = 0;
DECLARE @BranchCode nvarchar(50) = '';
DECLARE @BranchAddress nvarchar(max) = '';
DECLARE @CountryId int = 0;
DECLARE @StateId int = 0;
DECLARE @CityId int = 0;
DECLARE @PostalCode nvarchar(50) = '';
DECLARE @IsMainBranch bit = 0;
DECLARE @BranchHead_Id uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @CreatedOn datetime = '';
DECLARE @CreatedBy uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @ModifiedOn datetime = '';
DECLARE @ModifiedBy uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @Active bit = 0;

*/

MERGE INTO MS_Branch as Target
USING (select @Id as Id, @BranchName as BranchName) AS source
    ON (Target.Id = source.Id) 

WHEN MATCHED THEN
	UPDATE SET 
		 [BranchName] = @BranchName
		,[MailAlias] = @MailAlias
		,[TimeZone_Id] = @TimeZone_Id
		,[BranchCode] = @BranchCode
		,[BranchAddress] = @BranchAddress
		,[CountryId] = @CountryId
		,[StateId] = @StateId
		,[CityId] = @CityId
		,[PostalCode] = @PostalCode
		,[IsMainBranch] = @IsMainBranch
		,[BranchHead_Id] = @BranchHead_Id
		,[ModifiedOn] = @ModifiedOn
		,[ModifiedBy] = @ModifiedBy
		,[Active] = @Active


WHEN NOT MATCHED THEN
        INSERT (
            [MachineId]
           ,[MachineIp]
           ,[CompId]
		   ,[BranchName]
		   ,[MailAlias]
		   ,[TimeZone_Id]
		   ,[BranchCode]
		   ,[BranchAddress]
		   ,[CountryId]
		   ,[StateId]
		   ,[CityId]
		   ,[PostalCode]
		   ,[IsMainBranch]
		   ,[BranchHead_Id]
		   ,[CreatedOn]
		   ,[CreatedBy]
		   ,[ModifiedOn]
		   ,[ModifiedBy]
		   ,[Active])
        VALUES
           (
            @MachineId
           ,@MachineIp
           ,@CompId
		   ,@BranchName
		   ,ISNULL(@MailAlias,'')
		   ,@TimeZone_Id
		   ,ISNULL(@BranchCode,'')
		   ,ISNULL(@BranchAddress, '')
		   ,@CountryId
		   ,@StateId
		   ,@CityId
		   ,ISNULL(@PostalCode, '')
		   ,@IsMainBranch
		   ,@BranchHead_Id
		   ,@CreatedOn
		   ,@CreatedBy
		   ,@ModifiedOn
		   ,@ModifiedBy
		   ,@Active)

OUTPUT INSERTED.Id;


