using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.EmployeeSetup.DocumentInfo
{
    public class EmployeeDocumentWrapperAccess : IEmployeeDocumentAccess
    {
        public EmployeeDocument GetEmployeeDocument(Guid empDocId)
        {
            return EmployeeDocumentAccess.GetEmployeeDocument(empDocId);
        }

        public List<EmployeeDocument> GetEmployeeDocuments(Guid employeeId)
        {
            return EmployeeDocumentAccess.GetEmployeeDocuments(employeeId);
        }

        public bool UpsertEmployeeDocument(EmployeeDocument employeeDocument)
        {
            return EmployeeDocumentAccess.UpsertEmployeeDocument(employeeDocument);
        }
    }
}
