using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.EmployeeSetup.DocumentInfo
{
    public interface IEmployeeDocumentAccess
    {
        bool UpsertEmployeeDocument(EmployeeDocument employeeDocument);

        List<EmployeeDocument> GetEmployeeDocuments(Guid employeeId);

        EmployeeDocument GetEmployeeDocument(Guid empDocId);
    }
}
