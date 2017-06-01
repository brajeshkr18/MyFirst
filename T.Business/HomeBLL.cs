using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T.Model;
using T.Data;
using BizBrolly.Util;
//using TelaCall.Models;

namespace T.Business
{
    public class HomeBLL
    {
        public bool VerifyLogin(ref clsSignin objSignIn)
        {
            if ((new HomeDAL()).VerifyLoginDal(ref objSignIn))
            {
                return true;
            }
            return false;
        }
        public int SaveSpeciality(clsSpeciality objspeciality)
        {
            return (new HomeDAL()).SaveSpeciality(objspeciality);
        }
        public int SaveUsers(clsUser objusers)
        {
            return (new HomeDAL()).SaveUsers(objusers);
        }


        public List<clsOnCallPeople> GetOnCallList(DateTime Date)
        {
            return (new HomeDAL()).GetOnCallPeople(Date);
        }

        public clsScheduler GetSchedularDetailsBLL()
        {
            return (new HomeDAL()).GetScheduleDetails();
        }

        public clsOnCallPeople GetOnCallPeopleDetailBLL(long PersonId)
        {
            return (new HomeDAL()).GetOnCallPeopleDetails(PersonId);
        }

        public int CreateSchedule(long PersonId, DateTime StartsCall, DateTime EndsCall)
        {
           return  (new HomeDAL()).CreatePersonSchedule(PersonId, StartsCall, EndsCall);
        }

        public List<clsOnCall> GetOnCallBLL(DateTime Date, long onCallId)
        {
            return (new HomeDAL()).GetallOncallForSchedule(Date);
        }

        public List<clsOnCall> GetallOncallForScheduleBLL(DateTime Date)
        {
            return (new HomeDAL()).GetallOncallForSchedule(Date);
        }

        public List<clsOnCall> GetallOncallForEmergencyBLL(DateTime StartsCall)
        {
            return (new HomeDAL()).GetallOncallForEmergency(StartsCall);
        }

        public int CreateCallIn(ref clsActiveCall oclsActiveCall)
        {
            return (new HomeDAL()).CreateCallIn( ref oclsActiveCall);
        }

        public List<clsOnCallPeople> CanRequestPeople()
        {
            return (new HomeDAL()).GetAllOnCallPeoplerequestors();
        }

        public List<clsActiveCall> GetActiveCalls(long CallId)
        {
            return (new HomeDAL()).GetActiveCallsDAL(CallId);
        }

        public List<clsNotes> GetActiveCallNotes(long CallId)
        {
            return (new HomeDAL()).GetActiveNotes(CallId);
        }

        public int UpdateCallin(clsActiveCall oclsActiveCall)
        {
            return (new HomeDAL()).UpdateCallIn(oclsActiveCall);
        }

        public int EndCallin(long ActivaCallId, string Reason)
        {
            return (new HomeDAL()).EndCallIn(ActivaCallId, Reason);
        }

        public clsOnCall OnCallPersonDetails(long OnCallId)
        {
            return (new HomeDAL()).GetallOncallDetails(OnCallId);
        }

        public bool SendMailOnCallInCreation(string To, string Subject, string Body,string from,string CC, string BCC, bool enableSSl)
        {
            try
            {
                Emailer em = new Emailer();
                var IsSent = em.SendMail(To, Subject, Body, from, CC, BCC, enableSSl);
                return IsSent;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
        public int DeleteSpeciality(clsSpeciality objspeciality)
        {
            return (new HomeDAL()).DeleteSpeciality(objspeciality);
        }
        public int DeleteOnCallPeople(clsOnCallPeople objOnCall)
        {
            return (new HomeDAL()).DeleteOnCallPeople(objOnCall);
        }


        public bool RemoveOnCallPersonBLL(clsOnCall objOnCall)
        {
            return (new HomeDAL()).RemoveOnCallPerson(objOnCall);
        }

        public int DeleteUsers(clsUser objusers)
        {
            return (new HomeDAL()).DeleteUsers(objusers);
        }
            public int DeletePractices(clsPractices objPractice)
        {
            return (new HomeDAL()).DeletePractices(objPractice);
        }
         public int AddNotes(long ActivaCallId, string Notes)
        {
           return (new HomeDAL()).AddNotes(ActivaCallId, Notes);
        }
        }
    
}
