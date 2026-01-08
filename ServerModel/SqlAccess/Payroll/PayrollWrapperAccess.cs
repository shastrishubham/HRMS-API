using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerModel.Model.Payroll;

namespace ServerModel.SqlAccess.Payroll
{
    public class PayrollWrapperAccess : IPayrollAccess
    {
        public List<dynamic> GetEmpPayrollDetailsByBranchId(int year, int month, int branchId, Guid employeeId)
        {
            return PayrollAccess.GetEmpPayrollDetailsByBranchId(year, month, branchId, employeeId);
        }

        public List<PayrollInformation> GetCalculatedPayrollDetailsByBranchId(int year, int month, int branchId, Guid compId)
        {
            return PayrollAccess.GetCalculatedPayrollDetailsByBranchId(year, month, branchId, compId);
        }

        public void UpsertPayrollCreation(List<PayrollInformation> payrollInformation)
        {
             PayrollAccess.UpsertPayrollCreation(payrollInformation);
        }

        public void UpsertPayrollReimbursements(List<PayrollReimbursement> payrollReimbursements)
        {
            PayrollAccess.UpsertPayrollReimbursements(payrollReimbursements);
        }

        public List<EmployeePayrollInformation> GetEmployeePayrollInformation(Guid employeeId, int month, int year)
        {
            return PayrollAccess.GetEmployeePayrollInformation(employeeId, month, year);
        }

        public List<EmployeePayrollInformation> GetEmployeeSalaryHeadsDetails(Guid employeeId, int month, int year)
        {
            return PayrollAccess.GetEmployeeSalaryHeadsDetails(employeeId, month, year);
        }

        public List<PayrollReimbursement> GetEmployeeReimbursementsByBranchAndMonth(int year, int month, int branchId, Guid compId)
        {
            return PayrollAccess.GetEmployeeReimbursementsByBranchAndMonth(year, month, branchId, compId);
        }

        public bool UpsertSalaryAdjustment(List<SalaryAdjustment> salaryAdjustments)
        {
            return PayrollAccess.UpsertSalaryAdjustment(salaryAdjustments);
        }

        public List<SalaryAdjustment> GetSalaryAdjustmentEmployeesByCompId(Guid compId)
        {
            return PayrollAccess.GetSalaryAdjustmentEmployeesByCompId(compId);
        }

        public List<SalaryAdjustment> GetCalcSalaryAdjustmentsEmployeesByAdjustmentId(List<Guid> employeeIds, int adjustmentId)
        {
            return PayrollAccess.GetCalcSalaryAdjustmentsEmployeesByAdjustmentId(employeeIds, adjustmentId);
        }

        public List<SalaryAdjustment> GetSalaryAdjustmentsByPayrollMonthYear(DateTime payrollDate)
        {
            return PayrollAccess.GetSalaryAdjustmentsByPayrollMonthYear(payrollDate);
        }
    }
}
