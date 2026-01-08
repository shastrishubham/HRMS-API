using ServerModel.Model.Employee;
using ServerModel.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Masters
{
    public class PunchHelper
    {
        EmployeePunchRepository employeePunchRepository;

        public PunchHelper()
        {
            employeePunchRepository = new EmployeePunchRepository();
        }

        
    }
}
