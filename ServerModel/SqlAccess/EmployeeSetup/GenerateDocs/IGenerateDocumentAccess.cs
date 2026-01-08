using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.EmployeeSetup.GenerateDocs
{
    public interface IGenerateDocumentAccess
    {
        Guid UpsertGenerateDocument(EmployeeGenerateDocInfo generateDocInfo);

        List<EmployeeGenerateDocInfo> GetEmployeeGenerateDocsByEmpId(Guid companyId, Guid employeeId);
    }
}
