using ServerModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Model.Employee
{
    public class EmployeeInformation : EMP_Info
    {
        public string BranchName { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string CompanyName { get; set; }

        public EMP_PersonalInfo employeePersonalInformation { get; set; }

        public List<EMP_Addr> employeeAddressesInformations { get; set; }

        public List<EMP_Qualification> employeeQualificationInformations { get; set; }

        public List<EMP_WorkExp> employeeWorkExperienceInformations { get; set; }

        public List<EMP_FamilyInfo> employeeFamilyInformations { get; set; }

        public List<EMP_Docs> employeeDocuments { get; set; }

        public EMP_AcctInfo employeeAccountInformation { get; set; }

        public List<EMP_Images> employeeImages { get; set; }
    }
}
