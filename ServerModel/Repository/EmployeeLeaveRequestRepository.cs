using ServerModel.Database;
using ServerModel.Interfaces;
using ServerModel.Model;
using ServerModel.Model.Base;
using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Repository
{
    public class EmployeeLeaveRequestRepository
    {
        private IRespository<EMP_LeaveReq> respository = null;
     

        public EmployeeLeaveRequestRepository()
        {
            this.respository = new Repository<EMP_LeaveReq>();
        }

        public DataResult AddUpdateCreateEmployeeLeaveReqest(EmployeeLeaveRequestInformation employeeLeaveRequestInformation)
        {
            DataResult dataResult = new DataResult();
            try
            {
                EMP_LeaveReq existingEmployeeLeaveRequestInfo = this.respository.GetById(employeeLeaveRequestInformation.Id);

                if (existingEmployeeLeaveRequestInfo == null)
                {
                    employeeLeaveRequestInformation.FormDate = DateTime.Now;
                    EMP_LeaveReq empLeaveInfoDb = GetEmpLeaveRequestInfoDbFromEmployeeLeaveRequestInformation(employeeLeaveRequestInformation, Guid.NewGuid());
                    this.respository.Insert(empLeaveInfoDb);
                }
                else
                {
                    if (existingEmployeeLeaveRequestInfo.IsApprovedBy != null
                       && existingEmployeeLeaveRequestInfo.LeaveStatus == (int)LeaveStatusType.Approved)
                    {
                        dataResult.ErrorMessage = "Leave is already Approved by Approver";
                        dataResult.IsSuccess = false;
                        return dataResult;
                    }

                    existingEmployeeLeaveRequestInfo.MS_Leave_Id = employeeLeaveRequestInformation.MS_Leave_Id;
                    existingEmployeeLeaveRequestInfo.FromDate = employeeLeaveRequestInformation.FromDate;
                    existingEmployeeLeaveRequestInfo.ToDate = employeeLeaveRequestInformation.ToDate;
                    existingEmployeeLeaveRequestInfo.LeaveFor = employeeLeaveRequestInformation.LeaveFor;
                    existingEmployeeLeaveRequestInfo.LeaveReason = employeeLeaveRequestInformation.LeaveReason;
                    existingEmployeeLeaveRequestInfo.IsApprovedBy = employeeLeaveRequestInformation.IsApprovedBy;
                    existingEmployeeLeaveRequestInfo.LeaveStatus = employeeLeaveRequestInformation.LeaveStatus;
                    existingEmployeeLeaveRequestInfo.Docs = employeeLeaveRequestInformation.Docs;
                    existingEmployeeLeaveRequestInfo.TotalDays = employeeLeaveRequestInformation.TotalDays;

                    this.respository.Update(existingEmployeeLeaveRequestInfo);
                }
                this.respository.Save();

                dataResult.IsSuccess = true;
            }
            catch (Exception ex)
            {
                dataResult.ErrorMessage = ex.Message.ToString();
                dataResult.IsSuccess = false;
            }
            return dataResult;
        }

        public DataResult DeleteEmployeeLeaveRequest(EmployeeLeaveRequestInformation employeeLeaveRequestInformation)
        {
            DataResult dataResult = new DataResult();
            try
            {
                if (employeeLeaveRequestInformation.Id != null)
                {
                    EMP_LeaveReq existingEmployeeLeaveRequestInfo = this.respository.GetById(employeeLeaveRequestInformation.Id);

                    if (existingEmployeeLeaveRequestInfo != null)
                    {
                        //this.respository.Delete(existingEmployeeLeaveRequestInfo);
                        this.respository.RemoveEntity(existingEmployeeLeaveRequestInfo);
                       
                    }
                }

               // this.respository.Save();

                dataResult.IsSuccess = true;
            }
            catch (Exception ex)
            {
                dataResult.ErrorMessage = ex.Message.ToString();
                dataResult.IsSuccess = false;
            }
            return dataResult;
        }

        public DataResult ApproveEmployeeLeaveRequest(EmployeeLeaveRequestInformation employeeLeaveRequestInformation, Guid approvedBy)
        {
            DataResult dataResult = new DataResult();
            try
            {
                EMP_LeaveReq existingEmployeeLeaveRequestInfo = this.respository.GetById(employeeLeaveRequestInformation.Id);
                if (existingEmployeeLeaveRequestInfo != null)
                {
                    if (existingEmployeeLeaveRequestInfo.FromDate.HasValue
                        && existingEmployeeLeaveRequestInfo.FromDate.Value < DateTime.Now.Date)
                    {
                        dataResult.ErrorMessage = "Leave Start date is greater than Current date";
                        dataResult.IsSuccess = false;
                        return dataResult;
                    }

                    if (existingEmployeeLeaveRequestInfo.IsApprovedBy != null
                       && existingEmployeeLeaveRequestInfo.LeaveStatus == (int)LeaveStatusType.Approved)
                    {
                        dataResult.ErrorMessage = "Leave is already Approved by Approver";
                        dataResult.IsSuccess = false;
                        return dataResult;
                    }


                    existingEmployeeLeaveRequestInfo.IsApprovedBy = employeeLeaveRequestInformation.IsApprovedBy;
                    existingEmployeeLeaveRequestInfo.LeaveStatus = (int)LeaveStatusType.Approved;

                    this.respository.Update(existingEmployeeLeaveRequestInfo);
                }
                this.respository.Save();

                dataResult.IsSuccess = true;
            }
            catch (Exception ex)
            {
                dataResult.ErrorMessage = ex.Message.ToString();
                dataResult.IsSuccess = false;
            }
            return dataResult;
        }

        #region DB Model to Server Model Data Binding

        private EMP_LeaveReq GetEmpLeaveRequestInfoDbFromEmployeeLeaveRequestInformation(EmployeeLeaveRequestInformation employeeLeaveRequestInformation, Guid existingEmployeeId)
        {
            EMP_LeaveReq EMP_LeaveReq = new EMP_LeaveReq();
            EMP_LeaveReq.Id = existingEmployeeId;
            EMP_LeaveReq.FormDate = employeeLeaveRequestInformation.FormDate;
            EMP_LeaveReq.MachineId = "AJINKYA-PC";
            EMP_LeaveReq.MachineIp = "0.0.0.0";
            EMP_LeaveReq.CompId = employeeLeaveRequestInformation.CompId;
            EMP_LeaveReq.CreatedBy = employeeLeaveRequestInformation.CreatedBy;
            EMP_LeaveReq.EMP_Info_Id = employeeLeaveRequestInformation.EMP_Info_Id;
            EMP_LeaveReq.MS_Leave_Id = employeeLeaveRequestInformation.MS_Leave_Id;
            EMP_LeaveReq.FromDate = employeeLeaveRequestInformation.FromDate;
            EMP_LeaveReq.ToDate = employeeLeaveRequestInformation.ToDate;
            EMP_LeaveReq.LeaveFor = employeeLeaveRequestInformation.LeaveFor;
            EMP_LeaveReq.LeaveReason = employeeLeaveRequestInformation.LeaveReason;
            EMP_LeaveReq.IsApprovedBy = employeeLeaveRequestInformation.IsApprovedBy;
            EMP_LeaveReq.LeaveStatus = employeeLeaveRequestInformation.LeaveStatus;
            EMP_LeaveReq.Docs = employeeLeaveRequestInformation.Docs;
            EMP_LeaveReq.TotalDays = employeeLeaveRequestInformation.TotalDays;

            return EMP_LeaveReq;
        }

        #endregion

    }
}
