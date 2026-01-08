using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerModel.Model.Masters;

namespace ServerModel.SqlAccess.MasterSetup.CompanySetup
{
    public class CompanySetupInfoAccessWrapper : ICompanySetupInfoAccess
    {
        public bool UploadCompanyDocument(CompanyDocument companyDocument)
        {
            return CompanySetupInfoAccess.UploadCompanyDocument(companyDocument);
        }

        public List<CompanyDocument> GetCompDocsByCompId(Guid companyId)
        {
            return CompanySetupInfoAccess.GetCompDocsByCompId(companyId);
        }

        public bool UpsertCompanyPlanDetail(CompanyPlanDetail companyPlanDetail)
        {
            return CompanySetupInfoAccess.UpsertCompanyPlanDetail(companyPlanDetail);
        }

        public List<CompanyPlanDetail> GetCompanyPlanDetailsByCompId(Guid compId)
        {
            return CompanySetupInfoAccess.GetCompanyPlanDetailsByCompId(compId);
        }
    }

    public class CompanySetupInfoAccessWrapper<T> : ICompanySetupInfoAccess<T> where T : CompanyRegistration
    {
        public List<CompanyRegistration> GetCompanyRegistrationDetails()
        {
            return CompanySetupInfoAccess<T>.GetCompanyRegistrationDetails();
        }

        public List<CompanyRegistration> GetCompanyRegistrationDetailsById(Guid companyId)
        {
            return CompanySetupInfoAccess<T>.GetCompanyRegistrationDetailsById(companyId);
        }

        public Guid UpsertCompanyRegistration(CompanyRegistration companyRegistration)
        {
            return CompanySetupInfoAccess<T>.UpsertCompanyRegistration(companyRegistration);
        }
    }
}
