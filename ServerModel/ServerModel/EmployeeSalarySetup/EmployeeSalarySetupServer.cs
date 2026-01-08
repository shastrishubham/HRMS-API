using ServerModel.Model.Employee;
using ServerModel.SqlAccess.EmployeeSalarySetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.ServerModel.EmployeeSalarySetup
{
    public class EmployeeSalarySetupServer : EmployeeSalaryHeadsSetupDetails
    {
        #region Properties Interface

        public static IEmpSalarySetupAccess<EmployeeSalarySetupDetails> mEmpSalarySetupAccessT
            = new EmpSalarySetupAccessWrapper<EmployeeSalarySetupDetails>();

        #endregion

        public static int AddUpdateEmployeeSalarySetup(List<EmployeeSalarySetupDetails> employeeSalarySetupDetails)
        {
            foreach (EmployeeSalarySetupDetails employeeSalary in employeeSalarySetupDetails)
            {
                employeeSalary.FormDate = DateTime.UtcNow;
                try
                {
                    if (!employeeSalary.isDesignationWiseSalary)
                    {
                        CalculateTotalEarningAndDeductionAmount(employeeSalary);
                        return mEmpSalarySetupAccessT.AddUpdateEmployeeSalarySetup(employeeSalary);
                    }
                    else
                    {
                        // designation wise salary for all employees
                        if (employeeSalary.isForAllDesignationEmployee)
                        {
                            // For all employees to this designation
                            // get emloyees whose salary is not setup based on designation id
                            // in such case - employeeSalarySetupDetails.count == 1
                            Guid compId = employeeSalarySetupDetails.FirstOrDefault().CompId;
                            IEnumerable<EmployeeSalarySetupDetails> unSetupSalaryEmployees = GetUnSetupSalaryEmployeesByDesignationId(compId, employeeSalary.designationId).ToList();
                            foreach (EmployeeSalarySetupDetails employee in unSetupSalaryEmployees)
                            {
                                EmployeeSalarySetupDetails employeeSalarySetup = new EmployeeSalarySetupDetails
                                {
                                    FormDate = DateTime.Now,
                                    CompId = employeeSalary.CompId,
                                    CreatedBy = employeeSalary.CreatedBy,
                                    EmpId = employee.Id,
                                    TotalEarningAmt = employeeSalary.TotalEarningAmt,
                                    TotalDeductionAmt = employeeSalary.TotalDeductionAmt,
                                    MS_PayMode_Id = employeeSalary.MS_PayMode_Id,
                                    employeeSalaryHeadsSetupDetails = employeeSalary.employeeSalaryHeadsSetupDetails
                                        .Select(head =>
                                        {
                                            head.MS_PayMode_Id = employeeSalary.MS_PayMode_Id;
                                            head.EmpId = employee.Id;
                                            return head;
                                        }).ToList(),
                                    Active = true
                                };

                                CalculateTotalEarningAndDeductionAmount(employeeSalarySetup);
                                mEmpSalarySetupAccessT.AddUpdateEmployeeSalarySetup(employeeSalarySetup);
                            }
                        }
                        else
                        {
                            // designation wise salary for selected employees
                            CalculateTotalEarningAndDeductionAmount(employeeSalary);
                            mEmpSalarySetupAccessT.AddUpdateEmployeeSalarySetup(employeeSalary);
                        }
                    }
                    return 1;
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to setup salary", ex.InnerException);
                }

            }
            return 0;
        }

        public static IEnumerable<EmployeeSalarySetupDetails> GetUnSetupSalaryEmployeesByDesignationId(Guid compId, int designationId)
        {
            return mEmpSalarySetupAccessT.GetUnSetupSalaryEmployeesByDesignationId(compId, designationId);
        }

        private static void CalculateTotalEarningAndDeductionAmount(EmployeeSalarySetupDetails employeeSalarySetupDetails)
        {
            if (employeeSalarySetupDetails != null && employeeSalarySetupDetails.employeeSalaryHeadsSetupDetails != null)
            {
                foreach (var salaryComp in employeeSalarySetupDetails.employeeSalaryHeadsSetupDetails)
                {
                    salaryComp.FormDate = DateTime.UtcNow;
                    if (salaryComp.IsEarningComponent)
                    {
                        decimal amt = salaryComp.IsEarningComponent == true && !salaryComp.SalaryHeadName.ToLower().Equals("ctc", StringComparison.CurrentCultureIgnoreCase) ? salaryComp.Amount : 0;
                        employeeSalarySetupDetails.TotalEarningAmt = employeeSalarySetupDetails.TotalEarningAmt + amt;
                    }
                    else
                    {
                        decimal amt = salaryComp.IsEarningComponent == false && !salaryComp.SalaryHeadName.ToLower().Equals("ctc", StringComparison.CurrentCultureIgnoreCase) ? salaryComp.Amount : 0;
                        employeeSalarySetupDetails.TotalDeductionAmt = employeeSalarySetupDetails.TotalDeductionAmt + amt;
                    }
                }
            }
        }

        public static List<EmployeeSalarySetupDetails> GetSalarySetupByCompId(Guid compId)
        {
            return mEmpSalarySetupAccessT.GetSalarySetupByCompId(compId);
        }

        public static List<EmployeeSalaryHeadsSetupDetails> GetEmployeeSalarySetupDetails(Guid compId, Guid empId)
        {
            return mEmpSalarySetupAccessT.GetEmpSalarySetupDetails(compId, empId);
        }
    }
}
