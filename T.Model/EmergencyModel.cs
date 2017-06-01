using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T.Model
{
    public class EmergencyModel
    {
        public List<clsOnCallPeople> lstOnCallPeople { get; set; }
        public List<clsOnCallPeople> lstRequestingPeople { get; set; }
        public List<clsOnCall> lstOnCall { get; set; }
        public List<clsActiveCall> lstActiveCalls { get; set; }
        public List<clsNotes> lstNotes { get; set; }
    }
}
