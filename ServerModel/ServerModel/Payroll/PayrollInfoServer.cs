using ServerModel.Model;
using ServerModel.Model.Payroll;
using ServerModel.SqlAccess.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.ServerModel.Payroll
{
    public class PayrollInfoServer
    {
        #region Properties Interface

        public static IPayrollAccess mPayrollAccessT = new PayrollWrapperAccess();

        #endregion

        public List<dynamic> GetEmpPayrollDetailsByBranchId(int year, int month, int branchId, Guid employeeId)
        {
            return mPayrollAccessT.GetEmpPayrollDetailsByBranchId(year, month, branchId, employeeId);
        }

        public List<PayrollInformation> GetCalculatedPayrollDetailsByBranchId(int year, int month, int branchId, Guid compId)
        {
            return mPayrollAccessT.GetCalculatedPayrollDetailsByBranchId(year, month, branchId, compId);
        }

        public DataResult UpsertPayrollCreation(List<PayrollInformation> payrolls)
        {
            try
            {
                mPayrollAccessT.UpsertPayrollCreation(payrolls);
                return new DataResult { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new DataResult { ErrorMessage = "Something Went Wrong, Unable to process payroll creation", IsSuccess = false };
            }
        }

        public List<EmployeePayrollInformation> GetEmployeePayrollInformation(Guid employeeId, int month, int year)
        {
            return mPayrollAccessT.GetEmployeePayrollInformation(employeeId, month, year);
        }

        public List<EmployeePayrollInformation> GetEmployeeSalaryHeadsDetails(Guid employeeId, int month, int year)
        {
            return mPayrollAccessT.GetEmployeeSalaryHeadsDetails(employeeId, month, year);
        }

        public List<PayrollReimbursement> GetEmployeeReimbursementsByBranchAndMonth(int year, int month, int branchId, Guid compId)
        {
            return mPayrollAccessT.GetEmployeeReimbursementsByBranchAndMonth(year, month, branchId, compId);
        }

        public List<SalaryAdjustment> GetSalaryAdjustmentEmployeesByCompId(Guid compId)
        {
            return mPayrollAccessT.GetSalaryAdjustmentEmployeesByCompId(compId);
        }

        public List<SalaryAdjustment> GetSalaryAdjustmentsByPayrollMonthYear(DateTime payrollDate)
        {
            return mPayrollAccessT.GetSalaryAdjustmentsByPayrollMonthYear(payrollDate);
        }

        public void UpsertPayrollReimbursements(List<PayrollReimbursement> payrollReimbursements)
        {
            mPayrollAccessT.UpsertPayrollReimbursements(payrollReimbursements);
        }

        public bool UpsertSalaryAdjustment(List<SalaryAdjustment> salaryAdjustments)
        {
            List<SalaryAdjustment> calcSalaryAdjustments = new List<SalaryAdjustment>();

            List<Guid> empIds = salaryAdjustments.Select(x => x.EMP_Info_Id).Distinct().ToList();
            int adjustmentTypeId = salaryAdjustments.Select(x => x.MS_Payroll_AdjstType_Id).Distinct().FirstOrDefault();

            List<SalaryAdjustment> calcAdjustments = mPayrollAccessT.GetCalcSalaryAdjustmentsEmployeesByAdjustmentId(empIds, adjustmentTypeId);

            foreach(SalaryAdjustment salaryAdjustment in salaryAdjustments)
            {
                SalaryAdjustment calAdj = calcAdjustments.FirstOrDefault(x => x.EMP_Info_Id == salaryAdjustment.EMP_Info_Id);
                if(calAdj != null)
                {
                    salaryAdjustment.Amount = calAdj.CalculatedAdjustmentAmount;
                    calcSalaryAdjustments.Add(salaryAdjustment);
                }
            }

            return mPayrollAccessT.UpsertSalaryAdjustment(calcSalaryAdjustments);
        }
    }
}
