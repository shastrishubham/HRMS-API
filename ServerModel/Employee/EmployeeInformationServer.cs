using ServerModel.Database;
using ServerModel.Model.Employee;
using ServerModel.SqlAccess.EmployeeSetup.PersonalInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Employee
{
    public class EmployeeInformationServer
    {
        HRMSEntities dbContext;
        public EmployeeInformationServer()
        {
            dbContext = new HRMSEntities();
        }

        public void AddUpdateEmployeeInformation(EmployeeInformation employeeInformation)
        {
            var employeeDetails = dbContext.EMP_Info.Where(x => x.Id == employeeInformation.Id).FirstOrDefault();
            if (employeeDetails == null)
            {
                // create new record
                EMP_Info newEmployee = new EMP_Info();
                newEmployee.Id = employeeInformation.Id;
                newEmployee.FormDate = employeeInformation.FormDate;
                newEmployee.MachineId = employeeInformation.MachineId;
                newEmployee.MachineIp = employeeInformation.MachineIp;
                newEmployee.CompId = employeeInformation.CompId;
                newEmployee.CreatedBy = employeeInformation.CreatedBy;
                newEmployee.EmpId = employeeInformation.EmpId;
                newEmployee.TicketNo = employeeInformation.TicketNo;
                newEmployee.FirstName = employeeInformation.FirstName;
                newEmployee.MiddleName = employeeInformation.MiddleName;
                newEmployee.LastName = employeeInformation.LastName;
                newEmployee.FullName = employeeInformation.FullName;
                newEmployee.Gender = employeeInformation.Gender;
                newEmployee.FatherHusbandName = employeeInformation.FatherHusbandName;
                newEmployee.DOB = employeeInformation.DOB;
                newEmployee.DateOfJoining = employeeInformation.DateOfJoining;
                newEmployee.ProbationDuration = employeeInformation.ProbationDuration;
                newEmployee.ConfirmationDate = employeeInformation.ConfirmationDate;
                newEmployee.DateOfLeaving = employeeInformation.DateOfLeaving;
                newEmployee.PFNo = employeeInformation.PFNo;
                newEmployee.ESINo = employeeInformation.ESINo;
                newEmployee.PANNo = employeeInformation.PANNo;
                newEmployee.AadharNo = employeeInformation.AadharNo;
                newEmployee.MS_Branch_Id = employeeInformation.MS_Branch_Id;
                newEmployee.MS_Grade_Id = employeeInformation.MS_Grade_Id;
                newEmployee.MS_Dept_Id = employeeInformation.MS_Dept_Id;
                newEmployee.MS_Designation_Id = employeeInformation.MS_Designation_Id;
                newEmployee.PaymentMode = employeeInformation.PaymentMode;
                newEmployee.ModifiedBy = employeeInformation.ModifiedBy;
                newEmployee.ModifiedOn = employeeInformation.ModifiedOn;
                newEmployee.IsActive = employeeInformation.IsActive;

                dbContext.EMP_Info.Add(newEmployee);
                dbContext.SaveChanges();
            }
            else
            {
                // update record
                employeeDetails.Id = employeeInformation.Id;
                employeeDetails.FormDate = employeeInformation.FormDate;
                employeeDetails.MachineId = employeeInformation.MachineId;
                employeeDetails.MachineIp = employeeInformation.MachineIp;
                employeeDetails.CompId = employeeInformation.CompId;
                employeeDetails.CreatedBy = employeeInformation.CreatedBy;
                employeeDetails.EmpId = employeeInformation.EmpId;
                employeeDetails.TicketNo = employeeInformation.TicketNo;
                employeeDetails.FirstName = employeeInformation.FirstName;
                employeeDetails.MiddleName = employeeInformation.MiddleName;
                employeeDetails.LastName = employeeInformation.LastName;
                employeeDetails.FullName = employeeInformation.FullName;
                employeeDetails.Gender = employeeInformation.Gender;
                employeeDetails.FatherHusbandName = employeeInformation.FatherHusbandName;
                employeeDetails.DOB = employeeInformation.DOB;
                employeeDetails.DateOfJoining = employeeInformation.DateOfJoining;
                employeeDetails.ProbationDuration = employeeInformation.ProbationDuration;
                employeeDetails.ConfirmationDate = employeeInformation.ConfirmationDate;
                employeeDetails.DateOfLeaving = employeeInformation.DateOfLeaving;
                employeeDetails.PFNo = employeeInformation.PFNo;
                employeeDetails.ESINo = employeeInformation.ESINo;
                employeeDetails.PANNo = employeeInformation.PANNo;
                employeeDetails.AadharNo = employeeInformation.AadharNo;
                employeeDetails.MS_Branch_Id = employeeInformation.MS_Branch_Id;
                employeeDetails.MS_Grade_Id = employeeInformation.MS_Grade_Id;
                employeeDetails.MS_Dept_Id = employeeInformation.MS_Dept_Id;
                employeeDetails.MS_Designation_Id = employeeInformation.MS_Designation_Id;
                employeeDetails.PaymentMode = employeeInformation.PaymentMode;
                employeeDetails.ModifiedBy = employeeInformation.ModifiedBy;
                employeeDetails.ModifiedOn = employeeInformation.ModifiedOn;
                employeeDetails.IsActive = employeeInformation.IsActive;

                dbContext.SaveChanges();
            }
        }
    }
}
