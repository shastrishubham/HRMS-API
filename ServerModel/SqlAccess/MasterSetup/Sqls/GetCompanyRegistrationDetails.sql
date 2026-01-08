
/*

--GetCompanyRegistrationDetails.Sql



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