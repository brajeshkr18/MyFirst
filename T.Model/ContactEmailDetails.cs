using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T.Model
{
    public class ContactEmailDetails
    {

        public string ContactNameOfReceiver = "TalaCall";
        public string ContactTemplateMessage = "Greetings. You have received an enquiry on TalaCall. The requester has shared following details. Details are as follows.";
        public string ContactTemplateContent = "Enquiry By: $Name <br /> Requestor Email: $EMAIL <br />Requestor Phone: $MOBILE <br />Request Date: $CurrentDate <br />Requestor Message: $CMESSAGE";
       
    }
    public class ContactOnCallDetails
    {

       // public string ContactNameOfReceiver = "TalaCall";
        //public string ContactTemplateMessage = "Greetings. You have received an enquiry on TalaCall. The requester has shared following details. Details are as follows.";
        public string ContactTemplateContent = " PatientNumber: $PatientNumber <br /> Room Number : $RoomNumber <br />Requestor Name: $RequestorName <br />Response Time : $TAT  minutes <br />";

    }
}
