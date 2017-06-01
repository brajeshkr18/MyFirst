using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T.Model
{
    public class clsScheduler
    {
        public List<clsSpeciality> lstSpecility { get; set; }
        public List<clsOnCallPeople> lstOnCallPeople { get; set; }
        public List<clsOnCall> lstOnCall { get; set; }
    }
}
