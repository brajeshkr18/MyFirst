using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T.Model
{
    public class clsActiveCall
    {
        public clsActiveCall()
        {
            this.oclsOnCallPeople = new clsOnCallPeople();
            this.lstNotes = new List<string>();
        }
        public long Id { get; set; }
        public long OnCallPersonId { get; set; }
        public long RequestorId { get; set; }
        public string RequestorName { get; set; }
        public DateTime CallReqTime { get; set; }
        public string TAT { get; set; }
        public bool EMTALA { get; set; }
        public string PatientNumber { get; set; }
        public string RoomNumber { get; set; }
        public DateTime CallendedTime { get; set; }
        public string CallEndedBy { get; set; }
        public string Notes { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        public clsOnCallPeople oclsOnCallPeople { get; set; }
        public List<clsOnCallPeople> lstonCallPeople { get; set; }
        public List<string> lstNotes { get; set; }
        public clsOnCall oclsOnCall { get; set; }
        
    }
}
