using ServerModel.Database;
using ServerModel.Interfaces;
using ServerModel.Model;
using ServerModel.Model.Employee;
using ServerModel.Model.ESS;
using ServerModel.SqlAccess.ESS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Repository
{
    public class EmployeeRespository
    {
        private IRespository<EMP_Info> respository = null;

        public static IEssSetupAccess mEssSetupAccessT = new EssSetupAccessWrapper();

        public EmployeeRespository()
        {
            this.respository = new Repository<EMP_Info>();
        }

        public DataResult AddUpdateEmployee(EmployeeInformation employeeInformation)
        {
            DataResult dataResult = new DataResult();
            try
            {
                dataResult.IsSuccess = true;
                EMP_Info existingEmployee = this.respository.GetById(employeeInformation.Id);

                if (existingEmployee == null)
                {
                    Guid empId = Guid.NewGuid();
                    EMP_Info empInfoDb = GetEmpInfoDbFromEmployeeInformation(employeeInformation, empId);
                    this.respository.Insert(empInfoDb);
                    this.respository.Save();
                    // create employee Leave
                    EmployeeLeaves employeeLeave = new EmployeeLeaves
                    {
                        EmployeeId = empId,
                        CompId = employeeInformation.CompId,
                        CreatedBy = employeeInformation.CreatedBy ?? Guid.Empty,
                        MS_Leave_Id = 0,
                        TotalLeaves = 0,
                        AvailableLeaves = 0
                    };

                    bool isCreatedEmpLeave = mEssSetupAccessT.AddUpdateEmployeeLeaves(employeeLeave);
                    dataResult.IsSuccess = isCreatedEmpLeave;
                }
                else
                {
                    existingEmployee.EmpType = employeeInformation.EmpType;
                    existingEmployee.FirstName = employeeInformation.FirstName;
                    existingEmployee.MiddleName = employeeInformation.MiddleName;
                    existingEmployee.LastName = employeeInformation.LastName;
                    existingEmployee.FullName = employeeInformation.FirstName + ' ' + employeeInformation.MiddleName + ' ' + employeeInformation.LastName; ;
                    existingEmployee.Gender = employeeInformation.Gender;
                    existingEmployee.FatherHusbandName = employeeInformation.FatherHusbandName;
                    existingEmployee.DOB = employeeInformation.DOB;
                    existingEmployee.DateOfJoining = employeeInformation.DateOfJoining;
                    existingEmployee.ProbationDuration = employeeInformation.ProbationDuration;
                    existingEmployee.ConfirmationDate = employeeInformation.ConfirmationDate;
                    existingEmployee.DateOfLeaving = employeeInformation.DateOfLeaving;
                    existingEmployee.PFNo = employeeInformation.PFNo;
                    existingEmployee.ESINo = employeeInformation.ESINo;
                    existingEmployee.PANNo = employeeInformation.PANNo;
                    existingEmployee.AadharNo = employeeInformation.AadharNo;
                    existingEmployee.MS_Branch_Id = employeeInformation.MS_Branch_Id;
                    existingEmployee.MS_Grade_Id = employeeInformation.MS_Grade_Id;
                    existingEmployee.MS_Dept_Id = employeeInformation.MS_Dept_Id;
                    existingEmployee.MS_Designation_Id = employeeInformation.MS_Designation_Id;
                    existingEmployee.SourceOfHire = employeeInformation.SourceOfHire;
                    existingEmployee.PaymentMode = employeeInformation.PaymentMode;
                    existingEmployee.ModifiedBy = employeeInformation.ModifiedBy;
                    existingEmployee.ModifiedOn = employeeInformation.ModifiedOn;
                    this.respository.Update(existingEmployee);
                    this.respository.Save();
                }
            }
            catch (Exception ex)
            {
                dataResult.IsSuccess = false;
                dataResult.ErrorMessage = ex.Message.ToString();
            }
            return dataResult;
        }

        public IEnumerable<EMP_Info> GetAll()
        {
            IEnumerable<EMP_Info> empInfoDb = this.respository.GetAll();
            return empInfoDb;
        }

        public List<EmployeeInformation> GetAllEmployeesByCompanyBranch(Guid compId, int branchId)
        {
            var employeesByCompanyBranch = (from a in this.respository.GetAll()
                                            where a.CompId.Equals(compId) && a.MS_Branch_Id == branchId
                                                && a.IsActive == true
                                            select new EmployeeInformation
                                            {
                                                Id = a.Id,
                                                FullName = a.FullName
                                            }).ToList();
            return employeesByCompanyBranch;
        }

        public IEnumerable<EmployeeInformation> GetEmployeeListByCompanyId(Guid compId)
        {
            var employees = from a in this.respository.GetAll()
                            where a.IsActive == true && a.CompId == compId
                            select new EmployeeInformation
                            {
                                Id = a.Id,
                                FullName = a.FullName
                            };
            return employees;
        }

        public IEnumerable<EmployeeInformation> GetEmployeesByDesignationAndCompanyId(int designationId, Guid compId)
        {
            var employees = from a in this.respository.GetAll()
                            where a.IsActive == true 
                                && a.MS_Designation_Id == designationId
                                && a.CompId == compId
                            select new EmployeeInformation
                            {
                                Id = a.Id,
                                FullName = a.FullName
                            };
            return employees;
        }

        #region DB Model to Server Model Data Binding
        private EMP_Info GetEmpInfoDbFromEmployeeInformation(EmployeeInformation employeeInformation, Guid existingEmployeeId)
        {
            EMP_Info empInfoDb = new EMP_Info();
            empInfoDb.Id = existingEmployeeId;
            empInfoDb.FormDate = DateTime.Now;
            empInfoDb.MachineId = "AJINKYA-PC";
            empInfoDb.MachineIp = "0.0.0.0";
            empInfoDb.CompId = employeeInformation.CompId;
            empInfoDb.CreatedBy = employeeInformation.CreatedBy;
            empInfoDb.EmpId = employeeInformation.EmpId;
            empInfoDb.EmpType = employeeInformation.EmpType;
            empInfoDb.TicketNo = employeeInformation.TicketNo;
            empInfoDb.FirstName = employeeInformation.FirstName;
            empInfoDb.MiddleName = employeeInformation.MiddleName;
            empInfoDb.LastName = employeeInformation.LastName;
            empInfoDb.FullName = employeeInformation.FirstName + ' ' + employeeInformation.MiddleName + ' ' + employeeInformation.LastName;
            empInfoDb.Gender = employeeInformation.Gender;
            empInfoDb.FatherHusbandName = employeeInformation.FatherHusbandName;
            empInfoDb.DOB = employeeInformation.DOB;
            empInfoDb.DateOfJoining = employeeInformation.DateOfJoining;
            empInfoDb.ProbationDuration = employeeInformation.ProbationDuration;
            empInfoDb.ConfirmationDate = employeeInformation.ConfirmationDate;
            empInfoDb.DateOfLeaving = employeeInformation.DateOfLeaving;
            empInfoDb.PFNo = employeeInformation.PFNo;
            empInfoDb.ESINo = employeeInformation.ESINo;
            empInfoDb.PANNo = employeeInformation.PANNo;
            empInfoDb.AadharNo = employeeInformation.AadharNo;
            empInfoDb.MS_Branch_Id = employeeInformation.MS_Branch_Id;
            empInfoDb.MS_Grade_Id = employeeInformation.MS_Grade_Id;
            empInfoDb.MS_Dept_Id = employeeInformation.MS_Dept_Id;
            empInfoDb.MS_Designation_Id = employeeInformation.MS_Designation_Id;
            empInfoDb.SourceOfHire = employeeInformation.SourceOfHire;
            empInfoDb.PaymentMode = employeeInformation.PaymentMode;
            empInfoDb.ModifiedBy = employeeInformation.ModifiedBy;
            empInfoDb.ModifiedOn = employeeInformation.ModifiedOn;
            empInfoDb.IsActive = true;
            return empInfoDb;
        }

        private EMP_Info GetEmployeeInformationFromEmpInfoDb(EMP_Info empInfoDb)
        {
            EmployeeInformation employeeInformation = new EmployeeInformation();
            employeeInformation.FormDate = empInfoDb.FormDate;
            employeeInformation.MachineId = "AJINKYA-PC";
            employeeInformation.MachineIp = "0.0.0.0";
            employeeInformation.CompId = empInfoDb.CompId;
            employeeInformation.CreatedBy = empInfoDb.CreatedBy;
            employeeInformation.EmpId = empInfoDb.EmpId;
            employeeInformation.TicketNo = empInfoDb.TicketNo;
            employeeInformation.FirstName = empInfoDb.FirstName;
            employeeInformation.MiddleName = empInfoDb.MiddleName;
            employeeInformation.LastName = empInfoDb.LastName;
            employeeInformation.FullName = empInfoDb.FullName;
            employeeInformation.Gender = empInfoDb.Gender;
            employeeInformation.FatherHusbandName = empInfoDb.FatherHusbandName;
            employeeInformation.DOB = empInfoDb.DOB;
            employeeInformation.DateOfJoining = empInfoDb.DateOfJoining;
            employeeInformation.ProbationDuration = empInfoDb.ProbationDuration;
            employeeInformation.ConfirmationDate = empInfoDb.ConfirmationDate;
            employeeInformation.DateOfLeaving = empInfoDb.DateOfLeaving;
            employeeInformation.PFNo = empInfoDb.PFNo;
            employeeInformation.ESINo = empInfoDb.ESINo;
            employeeInformation.PANNo = empInfoDb.PANNo;
            employeeInformation.AadharNo = empInfoDb.AadharNo;
            employeeInformation.MS_Branch_Id = empInfoDb.MS_Branch_Id;
            employeeInformation.MS_Grade_Id = empInfoDb.MS_Grade_Id;
            employeeInformation.MS_Dept_Id = empInfoDb.MS_Dept_Id;
            employeeInformation.MS_Designation_Id = empInfoDb.MS_Designation_Id;
            employeeInformation.PaymentMode = empInfoDb.PaymentMode;
            employeeInformation.ModifiedBy = empInfoDb.ModifiedBy;
            employeeInformation.ModifiedOn = empInfoDb.ModifiedOn;
            employeeInformation.IsActive = true;
            return employeeInformation;
        }
        #endregion
    }
}
