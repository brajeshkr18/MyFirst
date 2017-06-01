using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using T.Model;
using T.Business;
using System.Configuration;
using System.Globalization;

namespace TelaCall.Controllers
{
    public class HomeController : Controller
    {
       
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                if (Session["UserInfo"] == null)
                    return RedirectToAction("Logout");
            }
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }


        #region Scheduler Methods
        [ValidateUser(AllowedRoles = "Schedular,Admin")]
        public ActionResult Scheduler()
        {
            if (Session["UserInfo"] != null)
            {
                var UserRole = ((T.Model.clsSignin)Session["UserInfo"]).Role;
                if (UserRole == "Schedular" || UserRole == "Admin")
                {
                    clsScheduler oclsscheduler = new clsScheduler();
                    oclsscheduler = new clsScheduler();
                    oclsscheduler.lstOnCall = new List<clsOnCall>();
                    oclsscheduler.lstSpecility = new List<clsSpeciality>();
                    oclsscheduler.lstOnCallPeople = new List<clsOnCallPeople>();
                    oclsscheduler = (new HomeBLL()).GetSchedularDetailsBLL();
                    return View(oclsscheduler);
                }
            }
            return RedirectToAction("Logout"); 
        }

        [HttpPost]
        public ActionResult GetPersonDetails(long personId)
        {
            if (personId > 0)
            {
                clsOnCallPeople oclsOnCallPeople = new clsOnCallPeople();
                oclsOnCallPeople = (new HomeBLL()).GetOnCallPeopleDetailBLL(personId);
                if (oclsOnCallPeople != null)
                    return PartialView("~/Views/Partials/_PersonDetails.cshtml", oclsOnCallPeople);
            }
            return Content("");
        }

        [HttpPost]
        public ActionResult CreateSchedule(long PersonId, string StartsCall, string EndsCall)
        {
            DateTime D_StartCall = new DateTime();
            DateTime D_EndsCall = new DateTime();
            string format = "yyyy-MM-dd HH:mm";
            bool isstartconverted = DateTime.TryParseExact(StartsCall, format, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out D_StartCall);
            bool isendconverted = DateTime.TryParseExact(EndsCall, format, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out D_EndsCall);
            if (D_StartCall.GetType() == typeof(DateTime) && D_EndsCall.GetType() == typeof(DateTime))
            {
                int Result = (new HomeBLL()).CreateSchedule(PersonId, D_StartCall, D_EndsCall);
                if (Result == 1)
                    return Json(new { Status = "Success", Message = "Schedule has been created successfully." });
                else if (Result == 2)
                    return Json(new { Status = "Failure", Message = "There is already a schedule present for this user between selected date and time." });
                else if (Result == 0)
                    return Json(new { Status = "Failure", Message = "User not found or something is wrong. please try again." });
            }
            return Json(new { Status = "Failure", Message = "Something went wrong while creating schedule." });
        }
        //Scheduler Post
        [HttpPost]
        public ActionResult GetOnCallList(string StartDate)
        {
            DateTime D_StartCall = new DateTime();
            string format = "MM/dd/yyyy";
            bool isstartconverted = DateTime.TryParseExact(StartDate, format, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out D_StartCall);
            if (D_StartCall.GetType() == typeof(DateTime))
            {
                //if (D_StartCall.Date == DateTime.Now.Date)
                //    D_StartCall = DateTime.Now.AddHours(3.00);
                clsScheduler oclsScheduler = new clsScheduler();
                oclsScheduler.lstOnCall = (new HomeBLL()).GetallOncallForScheduleBLL(D_StartCall);
                if (oclsScheduler.lstOnCall != null)
                    return PartialView("~/Views/Partials/_OnCallList.cshtml", oclsScheduler);
                else
                    return Json(new { Status = "" });
            }
            return Json(new { Status = "" });

        }

        [HttpPost]
        public ActionResult GetOnCallDetails(long PeopleId)
        {
            clsOnCall oclsOnCall = (new HomeBLL()).OnCallPersonDetails(PeopleId);
            return PartialView("~/Views/Partials/_OnCallPersonDetail.cshtml", oclsOnCall);
        }
        #endregion

        #region Emergency Methods
        [ValidateUser(AllowedRoles = "Emergency,Admin")]
        public ActionResult Emergency()
        {
            if (Session["UserInfo"] != null)
            {
                var UserRole = ((T.Model.clsSignin)Session["UserInfo"]).Role;
                if (UserRole == "Emergency" || UserRole == "Admin")
                {
                    DateTime D_StartCall = new DateTime();
                    string format = "MM/dd/yyyy";
                    bool isstartconverted = DateTime.TryParseExact(DateTime.Now.ToString("MM/dd/yyyy"), format, CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out D_StartCall);
                    if (D_StartCall.Date == DateTime.Now.AddHours(3.00).Date)
                        D_StartCall = DateTime.Now.AddHours(3.00);
                    ViewBag.SelectedDate = D_StartCall;
                    EmergencyModel objEmergencymodel = new EmergencyModel();
                    objEmergencymodel.lstOnCall = (new HomeBLL()).GetallOncallForEmergencyBLL(D_StartCall);
                    objEmergencymodel.lstRequestingPeople = (new HomeBLL()).CanRequestPeople();
                    objEmergencymodel.lstActiveCalls = (new HomeBLL()).GetActiveCalls(0);
                    objEmergencymodel.lstNotes = (new HomeBLL()).GetActiveCallNotes(0);
                    return View(objEmergencymodel);
                }
            }
            return RedirectToAction("Logout"); 
        }
        [HttpPost]
        public ActionResult GetOnCallListEmergency(string SelectedDate, bool IsManage = false)
        {
            DateTime D_StartCall = new DateTime();
            string format = "MM/dd/yyyy";
            bool isstartconverted = DateTime.TryParseExact(SelectedDate, format, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out D_StartCall);
            
            
            if (D_StartCall.GetType() == typeof(DateTime))
            {
                if (D_StartCall.Date == DateTime.Now.AddHours(3.00).Date)
                    D_StartCall = DateTime.Now.AddHours(3.00);
                else
                {
                    DateTime ActualESTTime = DateTime.Now.AddHours(3.00);
                    D_StartCall = D_StartCall.Add(ActualESTTime.Subtract(ActualESTTime.Date));
                }



                EmergencyModel oclsEmergency = new EmergencyModel();
                oclsEmergency.lstNotes = (new HomeBLL()).GetActiveCallNotes(0);
                oclsEmergency.lstOnCall = (new HomeBLL()).GetallOncallForEmergencyBLL(D_StartCall);
                ViewBag.SelectedDate = D_StartCall;
                if (oclsEmergency.lstOnCall != null)
                    if (IsManage)
                        return PartialView("~/Views/Partials/Manage/_OnCallListManage.cshtml", oclsEmergency);
                    else
                        return PartialView("~/Views/Partials/Emergency/_OnCallListEmergency.cshtml", oclsEmergency);
                else
                    return Json(new { Status = "" });
            }
            return Json(new { Status = "" });
        }
        [HttpPost]
        public ActionResult CreateCallIn(clsActiveCall oclsActiveCall)
        {

            if (oclsActiveCall.OnCallPersonId == 0 || oclsActiveCall.RequestorId == 0)
            {
                return Json(new { Status = "Failure", Message = "Oops! something is wrong." });
            }

            if ((new HomeBLL()).CreateCallIn(ref oclsActiveCall) == 1)
            {
                //clsOnCall oclsOnCall = (new HomeBLL()).OnCallPersonDetails(oclsActiveCall.Id);
                Mailer Emailer = new Mailer();
                
                var lstActive = (new HomeBLL()).GetActiveCalls(oclsActiveCall.Id);
              
                Emailer.SendContactMailOnCallCreation(lstActive[0].oclsOnCallPeople.Email, (lstActive[0].oclsOnCallPeople.FirstName) + " "+(lstActive[0].oclsOnCallPeople.LastName), lstActive[0].RequestorName, "Request from Emergency ward", ConfigurationManager.AppSettings["EmailFrom"].ToString(), "", "Dear doctor ! a Call In has been created. Details are As Follows :-", ConfigurationManager.AppSettings["CC"].ToString(), ConfigurationManager.AppSettings["BCC"].ToString(), true, Server.MapPath(ConfigurationManager.AppSettings["EmailTemplatePath"].ToString() + "OnCallCreation.html"), oclsActiveCall);
              
                return Json(new { Status = "Success", Message = "Call-In has been created successfully." });
            }
            else
            {
                return Json(new { Status = "Failure", Message = "Selected Person already is in call." });
            }
            
        }
        [HttpPost]
        public ActionResult GetActiveCallDetails(long ActiceCallId)
        {
            if (ActiceCallId != 0)
            {
                clsActiveCall oclsActiveCall = new clsActiveCall();
                List<clsActiveCall> lstActiveCall = (new HomeBLL()).GetActiveCalls(ActiceCallId);
                if (lstActiveCall.Count > 0)
                {
                    oclsActiveCall = lstActiveCall.FirstOrDefault();
                   // oclsActiveCall.lstNotes = (new HomeBLL()).GetActiveCallNotes(ActiceCallId);
                    oclsActiveCall.lstonCallPeople = (new HomeBLL()).CanRequestPeople();
                    return PartialView("~/Views/Partials/Emergency/_ActiveCallDetails.cshtml", oclsActiveCall);
                }
            }
                return null;
        }

      

        [HttpPost]
        public ActionResult UpdateCallIn(clsActiveCall oclsActiveCall)
        {
            if (oclsActiveCall.Id == 0)
                return Json(new { Status = "Failure", Message = "Something is wrong. Please try again." });

            var Result = (new HomeBLL()).UpdateCallin(oclsActiveCall);
            if (Result == 1)
                return Json(new { Status = "Success", Message = "Call-In has been updated successfully." });
            else
                return Json(new { Status = "Failure", Message = "Something is wrong. Please try again." });
        }
        [HttpPost]
        public ActionResult EndCall(long ActivaCallId,string Reason)
        {
            if (ActivaCallId == 0)
                return Json(new { Status = "Failure", Message = "Something is wrong. Please try again." });

            var Result = (new HomeBLL()).EndCallin(ActivaCallId, Reason);
            if (Result == 1)
                return Json(new { Status = "Success", Message = "Call-In has been ended successfully." });
            else
                return Json(new { Status = "Failure", Message = "Something is wrong. Please try again." });
        }
        public ActionResult GetActiveCalls(long Id)
        {
            EmergencyModel objEmergencymodel = new EmergencyModel();
            objEmergencymodel.lstActiveCalls = (new HomeBLL()).GetActiveCalls(0);
            if (objEmergencymodel.lstActiveCalls.Count > 0)
            {
                ViewBag.Id = Id;
                return PartialView("~/Views/Partials/Emergency/_ActiveCallList.cshtml", objEmergencymodel);
            }
            return null;
        }
        
        #endregion


        [HttpPost]
        public ActionResult GetOnCallDetailsEmergency(long PeopleId)
        {
            clsOnCall oclsOnCall = (new HomeBLL()).OnCallPersonDetails(PeopleId) ?? (new clsOnCall());
            var lstActive = (new HomeBLL()).GetActiveCalls(0);
            clsActiveCall oclsActiveCall = lstActive.Where(ac => ac.OnCallPersonId == oclsOnCall.Id).FirstOrDefault();
            oclsOnCall.IsInActiveCall = oclsActiveCall == null ? false : true;
            return PartialView("~/Views/Partials/Emergency/_OnCallDetailsEmergency.cshtml", oclsOnCall);
        }
        public ActionResult SignIn()
        {
            return View();
        }
        public ActionResult DashBoard()
        {
            if (Request.IsAuthenticated && Session["UserInfo"] != null)
            {
                var UserRole = ((T.Model.clsSignin)Session["UserInfo"]).Role;
                switch(UserRole){
                    case "Admin":
                        return RedirectToAction("Manage", "Admin");
                    case "Schedular":
                        return RedirectToAction("Scheduler", "Home");
                    case "Emergency":
                        return RedirectToAction("Emergency", "Home");
                    default:
                        return RedirectToAction("Logout"); 
                }
                    
            }
            return RedirectToAction("Logout");
        }
        public ActionResult Logout()
        {
            KillUser();
            return RedirectToAction("Index", "Home");
        }
        private void KillUser()
        {
            FormsAuthentication.SetAuthCookie("username", false);
            FormsAuthentication.SignOut();
            Session.Abandon();
        }


        #region Postback Methods
        [HttpPost]
        public ActionResult SignIn(clsSignin objSignin)
        {
            try
            {
                if ((new HomeBLL()).VerifyLogin(ref objSignin))
                {
                    FormsAuthentication.SetAuthCookie(objSignin.Username, false);
                    Response.Cookies[objSignin.Username].Expires = DateTime.Now.AddSeconds(10);
                    Session["UserInfo"] = objSignin;
                    return Json(new { Status = "Success" });
                }
                return Json(new { Status = "Failure", Message = "Username/Password does not match" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Failure", Message = "A network related issue occered. Please try again to login" });
            }

        }
        private void GetPeopleDetails(long Id)
        {

        }
        [HttpPost]
        public ActionResult ContactUs(clsContact contactProperties)
        {

            if (contactProperties != null && !string.IsNullOrEmpty(contactProperties.Email) && !string.IsNullOrEmpty(contactProperties.Message))
            {
                Mailer Emailer = new Mailer();
                Emailer.SendContactMailToTeam(ConfigurationManager.AppSettings["EmailTo"].ToString(), contactProperties.Name, contactProperties.Email, ConfigurationManager.AppSettings["Subject"].ToString(), ConfigurationManager.AppSettings["EmailFrom"].ToString(), contactProperties.phone, contactProperties.Message, ConfigurationManager.AppSettings["CC"].ToString(), ConfigurationManager.AppSettings["BCC"].ToString(), true, Server.MapPath(ConfigurationManager.AppSettings["EmailTemplatePath"].ToString() + "Template.html"));
                return Json(new { Status = "Success", Message = "Congratulation ! We got your contact request. We will soon get in touch with you." });
            }
            return Json(new { Status = "Failure", Message = "oops Something went wrong!" });
        }
        #endregion
        [HttpPost]
        public ActionResult AddTocart(clsContact contactProperties)
        {

            //if (contactProperties != null && !string.IsNullOrEmpty(contactProperties.Email) && !string.IsNullOrEmpty(contactProperties.Message))
            //{
            //    Mailer Emailer = new Mailer();
            //    Emailer.SendContactMailToTeam(ConfigurationManager.AppSettings["EmailTo"].ToString(), contactProperties.Name, contactProperties.Email, ConfigurationManager.AppSettings["Subject"].ToString(), ConfigurationManager.AppSettings["EmailFrom"].ToString(), contactProperties.phone, contactProperties.Message, ConfigurationManager.AppSettings["CC"].ToString(), ConfigurationManager.AppSettings["BCC"].ToString(), true, Server.MapPath(ConfigurationManager.AppSettings["EmailTemplatePath"].ToString() + "Template.html"));
            //    return Json(new { Status = "Success", Message = "Congratulation ! We got your contact request. We will soon get in touch with you." });
            //}
            return Json(new { Status = "Failure", Message = "oops Something went wrong!" });
        }

        // [HttpPost]
        //public ActionResult ForgetPassword(string EmailorUsername)
        //{
        //    var objDetails = (new LoginBLL()).ForgetPassword(EmailorUsername);
        //    if (objDetails != null)
        //    {
        //        string Receiversaddress = objDetails.Email + "," + objDetails.ClusterEmailId;
        //        string baseUrl = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port + Request.ApplicationPath.TrimEnd('/') + "/" + "Home/Reset/" + EncodeDecode.Base64Encode(objDetails.UserName);
        //        if ((new LoginBLL()).SendForgetPasswordMail(Receiversaddress, objDetails.FirstName + " " + objDetails.LastName, "Password reset link", ConfigurationManager.AppSettings["EmailFrom"].ToString(), ConfigurationManager.AppSettings["CC"].ToString(), ConfigurationManager.AppSettings["BCC"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSSLOnMail"].ToString()), Server.MapPath(ConfigurationManager.AppSettings["filepath"].ToString()) + "ForgotPassword.html", baseUrl))
        //            return Json(new { Status = "Success", Message = "<strong>Congrats</strong> ! An email with reset password link has been sent to your email address(es) <strong>" + Receiversaddress + "</strong>." });
        //        else
        //            return Json(new { Status = "Failure", Message = "Oops! something went wrong while sending email. please try again." });
        //    }
        //    return Json(new { Status = "Failure", Message = "The username or email does not exists, please enter correct details." });
        //}

        public ActionResult Emtala()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddNotes(long ActivaCallId, string Notes)
        {
            if (ActivaCallId == 0)
                return Json(new { Status = "Failure", Message = "Something is wrong. Please try again." });

            var Result = (new HomeBLL()).AddNotes(ActivaCallId, Notes);
            if (Result == 1)
                return Json(new { Status = "Success", Message = "Notes has been added successfully." });
            else
                return Json(new { Status = "Failure", Message = "Something is wrong. Please try again." });
        }
        public ActionResult GetActiveCallNotes(long ActiceCallId)
        {
            if (ActiceCallId != 0)
            {
                clsNotes objnotes = new clsNotes();
                objnotes.lstNotes = (new HomeBLL()).GetActiveCallNotes(ActiceCallId);
               
                return PartialView("~/Views/Partials/Emergency/_Notes.cshtml", objnotes);
            }
            return View();
        }
        [HttpPost]
        public ActionResult CallInAgain(long ActiceCallId)
        {
            if (ActiceCallId != 0)
            {
                clsActiveCall oclsActiveCall = new clsActiveCall();
                List<clsActiveCall> lstActiveCall = (new HomeBLL()).GetActiveCalls(ActiceCallId);
                if (lstActiveCall.Count > 0)
                {
                    oclsActiveCall = lstActiveCall.FirstOrDefault();
                    // oclsActiveCall.lstNotes = (new HomeBLL()).GetActiveCallNotes(ActiceCallId);
                    oclsActiveCall.lstonCallPeople = (new HomeBLL()).CanRequestPeople();
                   // return PartialView("~/Views/Partials/Emergency/_ActiveCallDetails.cshtml", oclsActiveCall);
                }

                if (oclsActiveCall.OnCallPersonId == 0 || oclsActiveCall.RequestorId == 0)
                {
                    return Json(new { Status = "Failure", Message = "Oops! something is wrong." });
                }

                if (((new HomeBLL()).CreateCallIn(ref oclsActiveCall) == 1)==false)
                {
                    //clsOnCall oclsOnCall = (new HomeBLL()).OnCallPersonDetails(oclsActiveCall.Id);
                    Mailer Emailer = new Mailer();

                    var lstActive = (new HomeBLL()).GetActiveCalls(oclsActiveCall.Id);

                    Emailer.SendContactMailOnCallCreation(lstActive[0].oclsOnCallPeople.Email, (lstActive[0].oclsOnCallPeople.FirstName) + " " + (lstActive[0].oclsOnCallPeople.LastName), lstActive[0].RequestorName, "Request from Emergency ward", ConfigurationManager.AppSettings["EmailFrom"].ToString(), "", "Dear doctor ! a Call In has been created. Details are As Follows :-", ConfigurationManager.AppSettings["CC"].ToString(), ConfigurationManager.AppSettings["BCC"].ToString(), true, Server.MapPath(ConfigurationManager.AppSettings["EmailTemplatePath"].ToString() + "OnCallCreation.html"), oclsActiveCall);

                    return Json(new { Status = "Success", Message = "Call-Again successfully." });
                }
                else
                {
                    return Json(new { Status = "Failure", Message = "Selected Person already is in call." });
                }

            }
            return null;
        }
        public ActionResult Products()
        {
            return View();
        }
    }
}
