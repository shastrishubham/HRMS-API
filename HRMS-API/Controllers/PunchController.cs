using ServerModel.Data;
using ServerModel.Model.Employee;
using ServerModel.Model.Punch;
using ServerModel.Repository;
using ServerModel.ServerModel.Punch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace HRMS_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PunchController : ApiController
    {
        PunchInfoServer punchInfoServer;

        EmployeePunchRepository employeePunchRespository;

        public PunchController()
        {
            DbContext.ConnectionString = WebApiConfig.ConnectionString;
            punchInfoServer = new PunchInfoServer();
            employeePunchRespository = new EmployeePunchRepository();
        }

        [Route("api/Punch/AddPunchInTime")]
        [HttpGet]
        public int AddPunchInTime(PunchInOut punchInOut)
        {
            return punchInfoServer.AddPunchInTime(punchInOut);
        }

        [Route("api/Punch/CreatePunchEntry")]
        [HttpPost]
        public void CreatePunchEntry(EmployeePunchInformation employeePunchInformation)
        {
            employeePunchRespository.AddUpdateEmployeePunch(employeePunchInformation);
        }

        [Route("api/Punch/GetEmployeesPunchesByComIdAndShiftId")]
        [HttpGet]
        public List<EmployeePunchInformation> GetEmployeesPunchesByComIdAndShiftId(Guid compId, int shiftId, DateTime filterDate)
        {
            return employeePunchRespository.GetEmployeesPunchesByComIdAndShiftId(compId, shiftId, filterDate);
        }


        [Route("api/Punch/GetEmployeePunchesById")]
        [HttpGet]
        public List<EmployeeAttendanceInfo> GetEmployeePunchesById(Guid employeeId, int month, int year)
        {
            return punchInfoServer.GetEmployeePunchesById(employeeId, month, year);
        }
    }
}
