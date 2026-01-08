using ServerModel.Model.Masters;
using ServerModel.ServerModel.Helper;
using ServerModel.SqlAccess.MasterSetup.CompanySetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.ServerModel.Masters.CompanySetup
{
    public class CompanySetupServer : CompanyRegistration
    {
        #region Properties Interface

        public static ICompanySetupInfoAccess<CompanyRegistration> mCompanySetupAccessT
            = new CompanySetupInfoAccessWrapper<CompanyRegistration>();

        public static ICompanySetupInfoAccess mCompanySetupGeneralAccessT
           = new CompanySetupInfoAccessWrapper();
        #endregion

        public static Guid UpsertCompanyRegistration(CompanyRegistration companyRegistration)
        {
            if(companyRegistration.FinancialYearFrom == null || companyRegistration.FinancialYearFrom == DateTime.MinValue)
            {
                companyRegistration.FinancialYearFrom = FinancialYearHelper.GetFinancialYearStart();
            }

            if (companyRegistration.FinancialYearTo == null || companyRegistration.FinancialYearTo == DateTime.MinValue)
            {
                companyRegistration.FinancialYearTo = FinancialYearHelper.GetFinancialYearEnd();
            }
            return mCompanySetupAccessT.UpsertCompanyRegistration(companyRegistration);
        }

        public static bool UploadCompanyDocument(CompanyDocument companyDocument)
        {
            return mCompanySetupGeneralAccessT.UploadCompanyDocument(companyDocument);
        }

        public static bool UpsertCompanyPlanDetail(CompanyPlanDetail companyPlanDetail)
        {
            return mCompanySetupGeneralAccessT.UpsertCompanyPlanDetail(companyPlanDetail);
        }

        public static List<CompanyRegistration> GetCompanyRegistrationDetailsById(Guid companyId)
        {
            return mCompanySetupAccessT.GetCompanyRegistrationDetailsById(companyId);
        }

        public static List<CompanyRegistration> GetCompanyRegistrationDetails()
        {
            return mCompanySetupAccessT.GetCompanyRegistrationDetails();
        }

        public static List<CompanyDocument> GetCompDocsByCompId(Guid compId)
        {
            return mCompanySetupGeneralAccessT.GetCompDocsByCompId(compId);
        }

        public static List<CompanyPlanDetail> GetCompanyPlanDetailsByCompId(Guid compId)
        {
            return mCompanySetupGeneralAccessT.GetCompanyPlanDetailsByCompId(compId);
        }

        public static List<Countries> GetCountries()
        {
            return CompanySetupInfoAccess.GetCountries();
        }

        public static List<States> GetStatesByCountryId(int countryId)
        {
            return CompanySetupInfoAccess.GetStatesByCountryId(countryId);
        }

        public static List<Cities> GetCitiesByStateId(int stateId)
        {
            return CompanySetupInfoAccess.GetCitiesByStateId(stateId);
        }
    }
}
