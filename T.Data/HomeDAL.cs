using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T.Model;
using T.Data.Entity;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
//using TelaCall.Models;

namespace T.Data
{
    public class HomeDAL
    {
        public bool VerifyLoginDal(ref clsSignin objSignIn)
        {
            using (var maincontext = new Entity.TelaCallEntities())
            {
                try
                {
                    string username = objSignIn.Username;
                    string password = objSignIn.password;
                    var objUsers = (from user in maincontext.tblUsers.Where(user => user.Username == username && user.Password == password && user.IsActive == true)
                                    join Urole in maincontext.mtblRoles on user.RoleId equals Urole.id
                                    select new clsSignin
                                    {
                                        FirstName = user.FirstName,
                                        LastName = user.LastName,
                                        UserId = user.id,
                                        Role = Urole.Role
                                    }).FirstOrDefault();

                    if (objUsers != null)
                    {
                        objSignIn = objUsers;
                        return true;
                    }
                   
                }
                catch (Exception ex)
                {
                  //  return new JavascriptResult() { Script = "alert('Successfully registered');" };
                    return false;
                }
                objSignIn = null;
                return false;
            }
        }
        public List<clsOnCallPeople> GetOnCallPeople(DateTime SelectedDate)
        {
            List<clsOnCallPeople> lstOnCallPeople = new List<clsOnCallPeople>();
            using(var talaCallContext = new Entity.TelaCallEntities())
	        {
                var OnCallPeopleList = (from user in talaCallContext.tblOnCalls.Where(call => call.StartsCall <= SelectedDate && call.EndsCall >= SelectedDate && call.IsActive == true)
                                        join userDetail in talaCallContext.tblOnCallPeoples on user.OnCAllPersonID equals userDetail.id
                                        join speciality in talaCallContext.tblSpecialities on userDetail.SpecialityId equals speciality.id
                                       select new clsOnCallPeople
                                       {
                                           Speciality = speciality.Speciality,
                                           FirstName = userDetail.FirstName,
                                           LastName = userDetail.LastName
                                       }).ToList();
                lstOnCallPeople = OnCallPeopleList;
	        }


            return lstOnCallPeople;
        }
        public int SaveSpeciality(clsSpeciality objspeciality)
        {
            using (var maincontext = new Entity.TelaCallEntities ())
            {
                var Specility_Result = maincontext.Web_SaveSpeciality(objspeciality.id, objspeciality.Speciality, objspeciality.Description, objspeciality.IsActive, objspeciality.UserId, objspeciality.UserId).FirstOrDefault();
                return 1;
                //return Specility_Result.SpecialityResult;
            }
        }
        public int SaveUsers(clsUser objusers)
        {
            using (var maincontext = new Entity.TelaCallEntities())
            {
                var db_Result = maincontext.Web_SaveUsers(objusers.id, objusers.FirstName, objusers.LastName,objusers.password, objusers.RoleId, objusers.Username, objusers.Email,objusers.Phone , objusers.UserId, objusers.IsActive).FirstOrDefault();
               return 1;
                //return db_Result.UserResult;
            }
        }


        #region Scheduler DAL Methods
        public clsScheduler GetScheduleDetails()
        {
            clsScheduler oclsScheduler = new clsScheduler();
            oclsScheduler.lstSpecility = new List<clsSpeciality>();
            oclsScheduler.lstOnCallPeople = new List<clsOnCallPeople>();
            oclsScheduler.lstSpecility = GetAllSpecialities();
            oclsScheduler.lstOnCallPeople = GetOnCallPeople(0).Where(GOCP => GOCP.IsCredentialed == true).ToList();
            oclsScheduler.lstOnCall = GetallOncallForSchedule(DateTime.Now.AddHours(3.00));
            return oclsScheduler;
        }
        public List<clsOnCall> GetallOncallForSchedule(DateTime SearchDate)
        {
            using (var maincontext = new Entity.TelaCallEntities())
            {
                DateTime TodayStart = SearchDate.Date;
                DateTime TodayEnd = SearchDate.Date.AddDays(1).AddTicks(-1);
                var lstOnCall = (from OC in maincontext.tblOnCalls.Where(pers => pers.IsActive == true && pers.StartsCall >= TodayStart && pers.StartsCall <= TodayEnd)
                                 join OCP in maincontext.tblOnCallPeoples on OC.OnCAllPersonID equals OCP.id
                                 join Specility in maincontext.tblSpecialities on OCP.SpecialityId equals Specility.id
                                 orderby Specility.Speciality ascending
                                 select new clsOnCall
                                 {
                                     Id = OC.id,
                                     objOnCallDetails = new clsOnCallPeople
                                     {
                                         id = OCP.id,
                                         FirstName = OCP.FirstName,
                                         LastName = OCP.LastName,
                                         Email = OCP.EmailAddress,
                                         PhoneNumber = OCP.PhoneNumber ?? 0,
                                         CellNo = OCP.CellNO ?? 0,
                                         Picture = OCP.Picture,
                                         Speciality = Specility.Speciality,
                                         oclsPractices = new clsPractices(),
                                         IsCredentialed = OCP.IsCredentialed,
                                         CanRequest = OCP.CanRequest,
                                         IsEmtala=OCP.IsEmtala
                                     },
                                     StartsCall = OC.StartsCall,
                                     EndsCall = OC.EndsCall
                                 }).ToList();

                return lstOnCall;
            }
        }
        public int CreatePersonSchedule(long PersonId, DateTime StartsCall, DateTime EndsCall)
        {
            using (var maincontext = new Entity.TelaCallEntities())
            {
                var objPerson = maincontext.tblOnCallPeoples.Where(person => person.id == PersonId && person.IsActive == true).FirstOrDefault();

                var objOncall = maincontext.tblOnCalls.Where(ocp => ocp.IsActive == true && ocp.OnCAllPersonID == PersonId &&
                    !(EndsCall <= ocp.StartsCall || StartsCall >= ocp.EndsCall)).FirstOrDefault();

                if (objPerson != null)
                {
                    if (objOncall == null)
                    {
                        tblOnCall otblOnCall = new tblOnCall();
                        otblOnCall.OnCAllPersonID = objPerson.id;
                        otblOnCall.Speciality = objPerson.tblSpeciality.id;
                        otblOnCall.StartsCall = StartsCall;
                        otblOnCall.EndsCall = EndsCall;
                        otblOnCall.IsActive = true;
                        otblOnCall.CreatedDate = DateTime.Now;
                        otblOnCall.CreatedBy = "101";
                        otblOnCall.ModifiedDate = DateTime.Now;
                        otblOnCall.ModifiedBy = "101";
                        maincontext.tblOnCalls.Add(otblOnCall);
                        maincontext.SaveChanges();
                        var result = maincontext.ChangeTracker.Entries();
                        return 1;
                    }
                    return 2;
                }
            }
            return 0;
        }
        #endregion

        #region Emergency DAL Methods
        public List<clsOnCall> GetallOncallForEmergency(DateTime SelectedDate)
        {
            using (var maincontext = new Entity.TelaCallEntities())
            {
                DateTime TodayStart = SelectedDate.Date;
                DateTime TodayEnd = SelectedDate.Date.AddDays(1).AddTicks(-1);

                //var lstOnCall = (from OC in maincontext.tblOnCalls.Where(pers => ((pers.StartsCall < SelectedDate && pers.EndsCall > SelectedDate && pers.EndsCall > DateTime.Now)
                //    || (pers.StartsCall.Year == SelectedDate.Year && pers.StartsCall.Month == SelectedDate.Month && SqlFunctions.DateDiff("DAY", pers.StartsCall, SelectedDate) == 0))
                var lstOnCall = (from OC in maincontext.tblOnCalls.Where(pers => pers.IsActive == true && pers.StartsCall <= SelectedDate && pers.EndsCall > SelectedDate)
                                 join OCP in maincontext.tblOnCallPeoples on OC.OnCAllPersonID equals OCP.id
                                 join Specility in maincontext.tblSpecialities on OCP.SpecialityId equals Specility.id
                                 orderby Specility.Speciality ascending
                                 select new clsOnCall
                                 {
                                     Id = OC.id,
                                     objOnCallDetails = new clsOnCallPeople
                                     {
                                         id = OCP.id,
                                         FirstName = OCP.FirstName,
                                         LastName = OCP.LastName,
                                         Email = OCP.EmailAddress,
                                         PhoneNumber = OCP.PhoneNumber ?? 0,
                                         CellNo = OCP.CellNO ?? 0,
                                         Picture = OCP.Picture,
                                         Speciality = Specility.Speciality,
                                         oclsPractices = new clsPractices(),
                                         IsCredentialed = OCP.IsCredentialed,
                                         IsEmtala=OCP.IsEmtala,
                                         CanRequest = OCP.CanRequest,
                                     },
                                     StartsCall = OC.StartsCall,
                                     EndsCall = OC.EndsCall
                                 }).ToList();

                return lstOnCall;
            }
        }

        public int CreateCallIn(ref clsActiveCall oclsActiveCall)
        {
            using (var maincontext = new Entity.TelaCallEntities())
            {
                long PersonId = oclsActiveCall.OnCallPersonId;
                var objActiveCall = maincontext.tblActivecalls.Where(Ac => Ac.OnCAllPersonID == PersonId && Ac.CallEndedTime == null).FirstOrDefault();

                if (objActiveCall == null)
                {
                    tblActivecall otblActivecalls = new tblActivecall();
                    otblActivecalls.OnCAllPersonID = oclsActiveCall.OnCallPersonId;
                    otblActivecalls.RequestPersonId = oclsActiveCall.RequestorId;
                    otblActivecalls.CallReqTime = DateTime.Now;
                    otblActivecalls.EMATALA = oclsActiveCall.EMTALA;
                    otblActivecalls.TAT = oclsActiveCall.TAT;
                    otblActivecalls.PatientNumber = oclsActiveCall.PatientNumber;
                    otblActivecalls.RoomNumber = oclsActiveCall.RoomNumber;
                    otblActivecalls.IsActive = true;
                    otblActivecalls.CreatedDate = DateTime.Now; ;
                    otblActivecalls.CreatedBy = "";
                    otblActivecalls.ModifiedDate = DateTime.Now; ;
                    otblActivecalls.ModifiedBy = "";
                    maincontext.tblActivecalls.Add(otblActivecalls);
                    int Status = maincontext.SaveChanges();
                    oclsActiveCall.Id = otblActivecalls.id;
                    return Status;
                }

                return 0;
            }
        }

        public int UpdateCallIn(clsActiveCall oclsActiveCall)
        {
            using (var maincontext = new Entity.TelaCallEntities())
            {
                var objActiveCall = maincontext.tblActivecalls.Where(Ac => Ac.id == oclsActiveCall.Id).FirstOrDefault();

                if (objActiveCall != null)
                {
                    objActiveCall.RequestPersonId = oclsActiveCall.RequestorId == 0 ? objActiveCall.RequestPersonId : oclsActiveCall.RequestorId;
                    objActiveCall.EMATALA = oclsActiveCall.EMTALA == objActiveCall.EMATALA ? objActiveCall.EMATALA : oclsActiveCall.EMTALA;
                    objActiveCall.TAT = oclsActiveCall.TAT == null ? objActiveCall.TAT : oclsActiveCall.TAT;
                    maincontext.SaveChanges();
                    return 1;
                }
                return 0;
            }
        }
        public int EndCallIn(long ActivaCallId, string Reason)
        {
            using (var maincontext = new Entity.TelaCallEntities())
            {
                var objActiveCall = maincontext.tblActivecalls.Where(Ac => Ac.id == ActivaCallId).FirstOrDefault();
                if (objActiveCall != null)
                {
                    objActiveCall.CallEndedTime = DateTime.Now;
                    objActiveCall.ReasonToEndCall = Reason;
                    maincontext.SaveChanges();
                    return 1;
                }
                return 0;
            }
        }



        public List<clsActiveCall> GetActiveCallsDAL(long ActiveCallId)
        {
            using (var talaCallContext = new Entity.TelaCallEntities())
            {
                var lst = (from Active in talaCallContext.tblActivecalls.Where(people =>
                    (people.id == (ActiveCallId == 0 ? people.id : ActiveCallId)) && people.IsActive == true && people.CallEndedTime == null)
                           join OnCall in talaCallContext.tblOnCalls on Active.OnCAllPersonID equals OnCall.id
                           join People in talaCallContext.tblOnCallPeoples on OnCall.OnCAllPersonID equals People.id
                           join Specility in talaCallContext.tblSpecialities on People.SpecialityId equals Specility.id
                           join Requestor in talaCallContext.tblOnCallPeoples on Active.RequestPersonId equals Requestor.id into X
                           from Y in X.DefaultIfEmpty()
                           orderby Specility.Speciality ascending
                           select new clsActiveCall
                           {
                               Id = Active.id,
                               OnCallPersonId = Active.OnCAllPersonID,
                               RequestorId = Active.RequestPersonId,
                               CallReqTime = Active.CallReqTime,
                               TAT = Active.TAT,
                               EMTALA = Active.EMATALA,
                               PatientNumber = Active.PatientNumber,
                               RoomNumber = Active.RoomNumber,
                               RequestorName = Y.FirstName + " " + Y.LastName,
                               oclsOnCallPeople = new clsOnCallPeople
                               {
                                   FirstName = People.FirstName,
                                   LastName = People.LastName,
                                   PhoneNumber = People.PhoneNumber,
                                   Speciality = Specility.Speciality,
                                   Picture = People.Picture,
                                   IsCredentialed = People.IsCredentialed,
                                   Email = People.EmailAddress
                               },
                               oclsOnCall = new clsOnCall
                               {
                                   StartsCall = OnCall.StartsCall,
                                   EndsCall = OnCall.EndsCall
                               }
                           }).ToList();

                return lst;
            }
        }

        public List<clsNotes> GetActiveNotes(long ActiveCallId)
        {
            using (var TalacallContext = new  Entity.TelaCallEntities())
            {
                var lst = (from ActiveNote in TalacallContext.tblNotes.Where(not => not.ActiveCallId == ActiveCallId && not.IsActive == true) 
                           select new clsNotes 
                           {
                               Notes = ActiveNote.Notes
                           }
                           ).ToList();
                return lst;
            }
        }


        #endregion

        public List<clsOnCall> GetallOncallForEmergency(DateTime SearchDate, long onCallId)
        {
            using (var maincontext = new Entity.TelaCallEntities())
            {
                var lstOnCall = (from OC in maincontext.tblOnCalls.Where(pers => pers.StartsCall <= SearchDate && pers.EndsCall >= SearchDate
                                        && pers.IsActive == true)
                                 join OCP in maincontext.tblOnCallPeoples on OC.OnCAllPersonID equals OCP.id
                                 join Specility in maincontext.tblSpecialities on OCP.SpecialityId equals Specility.id
                                 join practice in maincontext.tblPractices on OCP.PracticeId equals practice.id into R1
                                 from R2 in R1.DefaultIfEmpty()
                                 select new clsOnCall
                                 {
                                     Id = OC.id,
                                     objOnCallDetails = new clsOnCallPeople
                                     {
                                         id = OCP.id,
                                         FirstName = OCP.FirstName,
                                         LastName = OCP.LastName,
                                         Email = OCP.EmailAddress,
                                         PhoneNumber = OCP.PhoneNumber ?? 0,
                                         CellNo = OCP.CellNO ?? 0,
                                         Picture = OCP.Picture,
                                         Speciality = Specility.Speciality,
                                         oclsPractices = new clsPractices
                                         {
                                             Address = R2.Address,
                                             URL = R2.URL,
                                             FaxNumber = R2.FaxNumber
                                         },
                                         IsCredentialed = OCP.IsCredentialed,
                                         CanRequest = OCP.CanRequest,
                                     },
                                     StartsCall = OC.StartsCall,
                                     EndsCall = OC.EndsCall
                                 }).ToList();

                return lstOnCall;
            }
        }
        public clsOnCallPeople GetOnCallPeopleDetails(long PeopleId)
        {
            clsOnCallPeople oclsOnCallPeople = new clsOnCallPeople();
            var lstOnCallPeople = GetOnCallPeople(PeopleId);
            if (lstOnCallPeople.Count > 0)
            {
                oclsOnCallPeople = lstOnCallPeople[0];
                return oclsOnCallPeople;
            }
            return null;
        }
        public clsOnCall GetallOncallDetails(long onCallId)
        {
            using (var maincontext = new Entity.TelaCallEntities())
            {
                var oclsOnCall = (from OC in maincontext.tblOnCalls.Where(pers => pers.IsActive == true && pers.id == (onCallId != 0 ? onCallId : pers.id))
                                 join OCP in maincontext.tblOnCallPeoples on OC.OnCAllPersonID equals OCP.id
                                 join Specility in maincontext.tblSpecialities on OCP.SpecialityId equals Specility.id
                                 join practice in maincontext.tblPractices on OCP.PracticeId equals practice.id into R1
                                 from R2 in R1.DefaultIfEmpty()
                                 select new clsOnCall
                                 {
                                     Id = OC.id,
                                     objOnCallDetails = new clsOnCallPeople
                                     {
                                         id = OCP.id,
                                         FirstName = OCP.FirstName,
                                         LastName = OCP.LastName,
                                         Email = OCP.EmailAddress,
                                         PhoneNumber = OCP.PhoneNumber ?? 0,
                                         CellNo = OCP.CellNO ?? 0,
                                         Picture = OCP.Picture,
                                         Speciality = Specility.Speciality,
                                         oclsPractices = new clsPractices
                                         {
                                             Address = R2.Address,
                                             URL = R2.URL,
                                             FaxNumber = R2.FaxNumber
                                         },
                                         IsEmtala=OCP.IsEmtala,
                                         IsCredentialed = OCP.IsCredentialed,
                                         CanRequest = OCP.CanRequest,
                                     },
                                     StartsCall = OC.StartsCall,
                                     EndsCall = OC.EndsCall
                                 }).FirstOrDefault();

                return oclsOnCall;
            }
        }


        public List<clsSpeciality> GetAllSpecialities()
        {
            using (var maincontext = new Entity.TelaCallEntities())
            {

                return (from speciality in maincontext.tblSpecialities.Where(sp => sp.IsActive == true)
                        orderby speciality.Speciality ascending
                                                     select new clsSpeciality
                                                     {
                                                         id = speciality.id,
                                                         Speciality = speciality.Speciality,
                                                         Description = speciality.Description
                                                     }).ToList();
            }
        }

        public List<clsOnCallPeople> GetOnCallPeople(long PeopleId)
        {
            using (var maincontext = new Entity.TelaCallEntities())
            {
                clsOnCallPeople ov = new clsOnCallPeople();

                return (from OCPeople in maincontext.tblOnCallPeoples.Where(OCP => OCP.id == (PeopleId != 0 ? PeopleId : OCP.id) && OCP.IsActive == true)
                        join Specility in maincontext.tblSpecialities on OCPeople.SpecialityId equals Specility.id
                        join practice in maincontext.tblPractices on OCPeople.PracticeId equals practice.id into R1 from R2 in R1.DefaultIfEmpty()
                        select new clsOnCallPeople
                        {
                            id = OCPeople.id,
                            FirstName = OCPeople.FirstName,
                            LastName = OCPeople.LastName,
                            Email = OCPeople.EmailAddress,
                            PhoneNumber = OCPeople.PhoneNumber ?? 0,
                            CellNo = OCPeople.CellNO ?? 0,
                            Picture = OCPeople.Picture,
                            SpecialityId = OCPeople.SpecialityId,
                            Speciality = Specility.Speciality,
                            PracticeId=OCPeople.PracticeId,
                            CallCarrier = OCPeople.CallCarrier,
                            oclsPractices = new clsPractices {
                                Address = R2.Address,
                                URL = R2.URL,
                                FaxNumber = R2.FaxNumber
                            },
                            IsEmtala = OCPeople.IsEmtala,
                            IsCredentialed = OCPeople.IsCredentialed,
                            CanRequest = OCPeople.CanRequest
                        }).ToList();
            }
        }

        public List<clsOnCallPeople> GetAllOnCallPeoplerequestors()
        {
            List<clsOnCallPeople> lstOnCallPeople = new List<clsOnCallPeople>();
            using (var talaCallContext = new Entity.TelaCallEntities())
            {
                var OnCallPeopleList = (from user in talaCallContext.tblOnCallPeoples.Where(people => people.CanRequest == true)
                                        select new clsOnCallPeople
                                        {
                                            id = user.id,
                                            FirstName = user.FirstName,
                                            LastName = user.LastName
                                        }).ToList();
                lstOnCallPeople = OnCallPeopleList;
            }
            return lstOnCallPeople;
        }

        //public clsContact GetForgetPasswordDetails(string email)
        //{
        //    using (var maincontext = new Entity.TelaCallEntities())
        //    {
        //        var objuserdetails = new clsContact();
        //        var objGetForgetDetails = maincontext.Web_ForgetPassword(email).FirstOrDefault();
        //        if (objGetForgetDetails != null)
        //        {
        //            objuserdetails.UserName = objGetForgetDetails.UserId;
        //            objuserdetails.FirstName = objGetForgetDetails.FirstName;
        //            objuserdetails.LastName = objGetForgetDetails.LastName;
        //            objuserdetails.Email = objGetForgetDetails.EmailId;
        //            objuserdetails.ClusterEmailId = objGetForgetDetails.ClusterEmailId;

        //            return objuserdetails;
        //        }
        //        return null;
        //    }
        //}
        public int DeleteSpeciality(clsSpeciality objspeciality)
        {
            using (var maincontext = new Entity.TelaCallEntities())
            {

                var obj = new Entity.tblSpeciality();
                //var obj = maincontext.tblPractices.FirstOrDefault();
                if (obj != null)
                {
                    if (objspeciality.id > 0)
                    {
                        var specialityid = objspeciality.id;
                        var objspeci = new Entity.tblSpeciality();
                        objspeci = maincontext.tblSpecialities.Single(a => a.id == specialityid);
                        objspeci.IsActive = objspeciality.IsActive;
                         maincontext.SaveChanges();
                        return 1;

                    }
                    else
                    {
                       

                        return 1;
                    }
                }
                else
                {
                    return 0;
                }

            }
        }

        public int DeleteOnCallPeople(clsOnCallPeople objOnCall)
        {
            using (var maincontext = new Entity.TelaCallEntities())
            {
                var obj = new Entity.tblOnCallPeople();
                if (obj != null)
                {
                    if (objOnCall.id > 0)
                    {
                        var oncallpeopleid = objOnCall.id;
                        var objoncall = new Entity.tblOnCallPeople();
                        objoncall = maincontext.tblOnCallPeoples.Single(a => a.id == oncallpeopleid);
                        objoncall.IsActive = false;
                        maincontext.SaveChanges();
                        return 1;

                    }
                    else
                    {


                        return 1;
                    }
                }
                else
                {
                    return 0;
                }

            }
        }
        public int DeleteUsers(clsUser objusers)
        {
            using (var maincontext = new Entity.TelaCallEntities())
            {

                var obj = new Entity.tblUser();
                //var obj = maincontext.tblPractices.FirstOrDefault();
                if (obj != null)
                {
                    if (objusers.id > 0)
                    {
                        var objuser = objusers.id;
                        var objuse = new Entity.tblUser();
                        objuse = maincontext.tblUsers.Single(a => a.id == objuser);
                        objuse.IsActive = false;
                        maincontext.SaveChanges();
                        return 1;

                    }
                    else
                    {


                        return 1;
                    }
                }
                else
                {
                    return 0;
                }

            }
        }
        public int DeletePractices(clsPractices objPractice)
        {
            using (var maincontext = new Entity.TelaCallEntities())
            {

                var obj = new Entity.tblPractice();
                //var obj = maincontext.tblPractices.FirstOrDefault();
                if (obj != null)
                {
                    if (objPractice.id > 0)
                    {
                        var Practiceid = objPractice.id;
                        var objpract = new Entity.tblPractice();
                        objpract = maincontext.tblPractices.Single(a => a.id == Practiceid);
                        objpract.IsActive = false;
                        maincontext.SaveChanges();
                        return 1;

                    }
                    else
                    {


                        return 1;
                    }
                }
                else
                {
                    return 0;
                }

            }
        }

        public bool RemoveOnCallPerson(clsOnCall objOnCall)
        {
            using (var mainContext = new TelaCallEntities())
            {
                var objOncallPerson = mainContext.tblOnCalls.Where(Person => Person.id == objOnCall.Id && Person.IsActive == true).FirstOrDefault();

                if (objOncallPerson != null)
                {
                    objOncallPerson.IsActive = false;
                    mainContext.SaveChanges();
                    return true;
                }
            }

            return false;
        }

        public int AddNotes(long ActivaCallId, string Notes)
        {
            using (var maincontext = new Entity.TelaCallEntities())
            {

                var obj = new Entity.tblNote();
                //var obj = maincontext.tblPractices.FirstOrDefault();
                if (obj != null)
                {
                    if (ActivaCallId > 0)
                    {
                        obj.Notes = Notes;
                        obj.ActiveCallId = ActivaCallId;
                        obj.IsActive = true;
                        obj.ModifiedDate = System.DateTime.Now;
                        obj.CreatedBy = "101";
                        obj.CreatedDate = System.DateTime.Now;
                        obj.ModifiedBy = "101";
                        maincontext.tblNotes.Add(obj);
                        maincontext.SaveChanges();
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }

            }
        }
    }
}
