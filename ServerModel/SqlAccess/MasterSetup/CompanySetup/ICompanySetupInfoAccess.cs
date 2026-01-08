using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.MasterSetup.CompanySetup
{
    public interface ICompanySetupInfoAccess
    {
        bool UploadCompanyDocument(CompanyDocument companyDocument);

        List<CompanyDocument> GetCompDocsByCompId(Guid companyId);

        bool UpsertCompanyPlanDetail(CompanyPlanDetail companyPlanDetail);

        List<CompanyPlanDetail> GetCompanyPlanDetailsByCompId(Guid compId);
    }

    public interface ICompanySetupInfoAccess<T> where T : CompanyRegistration
    {
        Guid UpsertCompanyRegistration(CompanyRegistration companyRegistration);

        List<CompanyRegistration> GetCompanyRegistrationDetailsById(Guid companyId);

        List<CompanyRegistration> GetCompanyRegistrationDetails();
    }
}
