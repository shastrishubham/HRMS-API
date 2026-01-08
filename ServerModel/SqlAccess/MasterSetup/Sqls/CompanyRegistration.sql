/*

--CompanyRegistration.Sql



DECLARE @Id uniqueidentifier = 'B1D753C9-8A22-4390-960E-2E16ED3EE896';
DECLARE @FormDate datetime = '';
DECLARE @MachineId nvarchar(50) = '';
DECLARE @MachineIp nvarchar(50) = '';
DECLARE @CompanyName nvarchar(max) = '';
DECLARE @CompanyDomain nvarchar(max) = '';
DECLARE @CompanyAddress nvarchar(max) = 'address';
DECLARE @MS_Country_Id int = 0;
DECLARE @MS_State_Id int = 0;
DECLARE @MS_City_Id int = 0;
DECLARE @Pincode nvarchar(10) = '';
DECLARE @PFNo nvarchar(50) = '';
DECLARE @TANNo nvarchar(max) = '';
DECLARE @PANNo nvarchar(50) = '';
DECLARE @ESINo nvarchar(50) = '';
DECLARE @LINNo nvarchar(50) = '';
DECLARE @GSTNo nvarchar(50) = '';
DECLARE @FinancialYearFrom date = '';
DECLARE @FinancialYearTo date = '';
DECLARE @RegCertNo nvarchar(50) = '';
DECLARE @IndustryType_Id int = '';
DECLARE @Website nvarchar(250) = '';
DECLARE @Timezone_Id int = '';
DECLARE @ReportingCnt nvarchar(max) = '';
DECLARE @ReportingCntMail nvarchar(max) = '';
DECLARE @ReportingCntDesg nvarchar(max) = '';
DECLARE @Active bit = 0;

*/

MERGE INTO MS_CompReg as Target
USING (select @id as Id, @companyName as CompanyName) AS source
    ON (Target.Id = source.Id) 

WHEN MATCHED THEN
	UPDATE SET 
		 [CompanyName] = @companyName
		,[CompanyDomain] = @companyDomain
		,[CompanyAddress] = @companyAddress
		,[MS_Country_Id] =@MS_Country_Id
		,[MS_State_Id] =@MS_State_Id
		,[MS_City_Id] = @MS_City_Id
		,[Pincode] = @Pincode
		,[PFNo] = @PFNo
		,[TANNo] = @TANNo
		,[PANNo] = @PANNo
		,[ESINo] = @ESINo
		,[LINNo] = @LINNo
		,[GSTNo] = @GSTNo
		,[FinancialYearFrom] = @FinancialYearFrom
		,[FinancialYearTo] = @FinancialYearTo
		,[IndustryType_Id] = @IndustryType_Id
		,[Website] = @Website
		,[Timezone_Id] = @Timezone_Id
		,[RegCertNo] = @RegCertNo
		,[ReportingCnt] = @ReportingCnt
		,[ReportingCntMail] = @ReportingCntMail
		,[ReportingCntDesg] = @ReportingCntDesg
		,[Active] = @Active


WHEN NOT MATCHED THEN
        INSERT ([Id]
           ,[FormDate]
           ,[MachineId]
           ,[MachineIp]
           ,[CompanyName]
           ,[CompanyDomain]
           ,[CompanyAddress]
           ,[MS_Country_Id]
           ,[MS_State_Id]
           ,[MS_City_Id]
           ,[Pincode]
           ,[PFNo]
           ,[TANNo]
           ,[PANNo]
           ,[ESINo]
           ,[LINNo]
           ,[GSTNo]
           ,[FinancialYearFrom]
           ,[FinancialYearTo]
           ,[RegCertNo]
		   ,[IndustryType_Id]
		   ,[Website]
		   ,[Timezone_Id]
           ,[ReportingCnt]
           ,[ReportingCntMail]
           ,[ReportingCntDesg]
           ,[Active])
        VALUES
           (NEWID()
           ,@FormDate
           ,@MachineId
           ,@MachineIp
           ,@CompanyName
           ,@CompanyDomain
           ,@CompanyAddress
           ,@MS_Country_Id
           ,@MS_State_Id
           ,@MS_City_Id
           ,@Pincode
           ,@PFNo
           ,@TANNo
           ,@PANNo
           ,@ESINo
           ,@LINNo
           ,@GSTNo
           ,@FinancialYearFrom
           ,@FinancialYearTo
           ,@RegCertNo
		   ,@IndustryType_Id
		   ,@Website
		   ,@Timezone_Id
           ,@ReportingCnt
           ,@ReportingCntMail
           ,@ReportingCntDesg
           ,@Active)

OUTPUT INSERTED.Id;


