
/*

--GetCompanyRegistrationDetailsById.Sql



DECLARE @Id UNIQUEIDENTIFIER = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680'
*/

SELECT Id
      ,FormDate
      ,MachineId
      ,MachineIp
      ,CompanyName
      ,CompanyDomain
      ,CompanyAddress
      ,MS_Country_Id
      ,MS_State_Id
      ,MS_City_Id
      ,Pincode
      ,PFNo
      ,TANNo
      ,PANNo
      ,ESINo
      ,LINNo
      ,GSTNo
      ,FinancialYearFrom
      ,FinancialYearTo
      ,RegCertNo
	  ,IndustryType_Id
	  ,Website
	  ,Timezone_Id
      ,ReportingCnt
      ,ReportingCntMail
      ,ReportingCntDesg
      ,Active
  FROM MS_CompReg
  WHERE Id = @Id