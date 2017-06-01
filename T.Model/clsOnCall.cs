using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T.Model
{
    public class clsOnCall
    {

        public clsOnCall()
        {
            this.objOnCallDetails = new clsOnCallPeople();
        }

        public long Id { get; set; }
        public DateTime StartsCall { get; set; }
        public DateTime EndsCall { get; set; }
        public bool IsActive { get; set; }
        public bool IsInActiveCall { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public clsOnCallPeople objOnCallDetails { get; set; }
    }
}
