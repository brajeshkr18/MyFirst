using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using T.Model;
using T.Business;
using System.Configuration;
using System.IO;
//using TelaCall.Models;



namespace TelaCall.Controllers
{
    [ValidateUser(AllowedRoles = "Admin")]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult AddSpeciality()
        {
            clsSpeciality objSpeciality = new clsSpeciality();
            return View(objSpeciality);
        }
        [HttpPost]
        public ActionResult AddSpeciality(clsSpeciality objspeciality )
        {
            objspeciality.UserId = 101;
            var Specility_Result = (new HomeBLL()).SaveSpeciality(objspeciality);
                if (Specility_Result == 1)
                    return Json(new { Status = "Success", Message = "Specility has been added successfully." });
                else if (Specility_Result == 2)
                    return Json(new { Status = "Success", Message = "Specility has been updated successfully." });
                else if (Specility_Result == 0)
                    return Json(new { Status = "Failure", Message = "There is already a specility with this name, Try with a different name. " });
                else
                    return Json(new { Status = "Failure", Message = "Seems something is wrong! Please try again later." });
        }

        public ActionResult AddUser()
        {
            clsUser objUser = new clsUser();
            objUser.lstRoles = (new AdminBLL()).GetRoles();
            return View(objUser);
        }

        [HttpPost]
        public ActionResult AddUser(clsUser objusers)
        {
            var Savruser_Result = (new HomeBLL()).SaveUsers(objusers);
            if (Savruser_Result != null)
            {
                if (Savruser_Result == 1)
                    return Json(new { Status = "Success", Message = "Congrats! User details saved successfully" });
                else if (Savruser_Result == 2)
                    return Json(new { Status = "Success", Message = "Congrats! User details saved successfully" });
                else
                    return Json(new { Status = "Failure", Message = "Username you have entered is already in use for another account. Tryout with different username." });
            }
            return Json(new { Status = "Failure", Message = "Oops! Something went wrong while processing your request. Please try again later." });
        }

        public ActionResult Manage()
        {
            clsManage objmange = new clsManage();

            //clsUser objUser = new clsUser();
            //clsOnCallPeople oclsOnCallPeople = new clsOnCallPeople();
            objmange.lstSpeciality = (new AdminBLL()).GetSpeciality(0);
            objmange.lstPractices = (new AdminBLL()).GetPractices(0);
            objmange.lstOnCallPeople = (new AdminBLL()).GetOnCallPeople(0);
            objmange.lstUsers = (new AdminBLL()).GetUsers(0);
           objmange.oclsEmergency = new EmergencyModel();
           objmange.oclsEmergency.lstOnCall = new List<clsOnCall>();
            return View(objmange);
        }

        public ActionResult AddOnCallPeople()
        {
            clsOnCallPeople objOnCall = new clsOnCallPeople();
            objOnCall.lstPractices = (new AdminBLL()).GetPractices(0);
            objOnCall.lstSpeciality = (new AdminBLL()).GetSpeciality(0);
            return View(objOnCall);
        }
        [HttpPost]
        public ActionResult AddOnCallPeople(clsOnCallPeople objOnCall)
        {
            objOnCall.UserId = "101";
            objOnCall.OnCallPersonID = 101;
            if ((new AdminBLL()).SaveOnCallPeople(ref objOnCall))
            {
                return Json(new { Status = "Success", Message = "Data Saved Sucessfully." });
            }
            return Json(new { Status = "Failure", Message = "oops Something went wrong!" });
        }
        //public ActionResult UpdateSpeciality(int Id)
        //{
        //    if (Id > 0)
        //    {
        //        clsUser objUser = new clsUser();
        //        objUser.lstSpeciality = (new AdminBLL()).GetSpeciality(Id);
        //        if (objUser.lstSpeciality.Count > 0)
        //            return View("~/Views/Admin/AddSpeciality.cshtml", objUser);
        //    }
        //    return RedirectToAction("Dashboard", "Admin");
        //}
        public ActionResult AddPractices()
        {
            clsPractices oclsPractices = new clsPractices();
            return View(oclsPractices);
        }
        [HttpPost]
        public ActionResult AddPractices(clsPractices objPractice)
        {

            if ((new AdminBLL()).SavePractices(ref objPractice))
            {
                return Json(new { Status = "Success", Message = "data save Successfully" });
            }
            return Json(new { Status = "Failure", Message = "oops Something went wrong!" });
        }
        public ActionResult ManageSpeciality(int Id)
        {
            if (Id > 0)
            {
                clsSpeciality oclsSpeciality = new clsSpeciality();
                oclsSpeciality = (new AdminBLL()).GetSpeciality(Id).FirstOrDefault();
                if (oclsSpeciality != null)
                    return View("~/Views/Admin/AddSpeciality.cshtml", oclsSpeciality);
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ManagePractices(int Id)
        {
            if (Id > 0)
            {
                clsPractices oclsPractices = new clsPractices();
                oclsPractices = (new AdminBLL()).GetPractices(Id).FirstOrDefault();
                if (oclsPractices != null)
                    return View("~/Views/Admin/AddPractices.cshtml", oclsPractices);
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ManageUsers(int Id)
        {
            if (Id > 0)
            {
                clsUser oclsUser = new clsUser();
                List<clsUser> lstUsers = (new AdminBLL()).GetUsers(Id);
                if (lstUsers.Count > 0)
                {
                    oclsUser = (new AdminBLL()).GetUsers(Id).FirstOrDefault();
                    oclsUser.password = "pass";
                    oclsUser.lstRoles = (new AdminBLL()).GetRoles();
                    return View("~/Views/Admin/AddUser.cshtml", oclsUser);
                }
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ManageOnCallPeople(int Id)
        {
            if (Id > 0)
            {
                clsOnCallPeople oclsOnCallPeople = new clsOnCallPeople();
                oclsOnCallPeople = (new HomeBLL()).GetOnCallPeopleDetailBLL(Id);
                if (oclsOnCallPeople != null)
                {
                    oclsOnCallPeople.lstPractices = (new AdminBLL()).GetPractices(0);
                    oclsOnCallPeople.lstSpeciality = (new AdminBLL()).GetSpeciality(0);
                    if (oclsOnCallPeople != null)
                        return View("~/Views/Admin/AddOnCallPeople.cshtml", oclsOnCallPeople);
                }
                
            }
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public ActionResult UploadFile()
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    Fileexists();
                    var file = Request.Files[0];
                    var fileName = Path.GetFileName(file.FileName);
                    fileName = CreateFileName(fileName);
                    var Fullpath = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority) + ConfigurationManager.AppSettings["ImagePath"].ToString().Replace("~", "") + fileName;
                    var rootfilepath = ConfigurationManager.AppSettings["ImagePath"].ToString() + fileName;
                    var path = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ImagePath"].ToString()), fileName);
                    file.SaveAs(path);
                    return Json(new { Status = "Success", Message = "File uploaded Successfully.", path = Fullpath });
                }
                return Json(new { Status = "Failure", Message = "Please select a file to upload." });
            }
            catch (Exception)
            {
                return Json(new { Status = "Failure", Message = "A network related issue occered while saving file please try again." });

            }
        }

        private void Fileexists()
        {
            string subPath = ConfigurationManager.AppSettings["ImagePath"].ToString(); // your code goes here
            bool exists = System.IO.Directory.Exists(Server.MapPath(subPath));

            if (!exists)
                System.IO.Directory.CreateDirectory(Server.MapPath(subPath));
        }
        private string CreateFileName(string filename)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                var Fname = filename.Split('.')[0];
                var Fext = filename.Split('.')[1];

                return (Fname + "_" + DateTime.Now.ToString("yyyyMMddmmss")) + "." + Fext;
            }

            return "";

        }

        public ActionResult DeleteSpeciality(clsSpeciality objspeciality)
        {
            objspeciality.UserId = 101;
            var Specility_Result = (new HomeBLL()).DeleteSpeciality(objspeciality);
            if (Specility_Result == 1)
                return Json(new { Status = "Success", Message = "Specility has been Delected successfully." });
            else if (Specility_Result == 2)
                return Json(new { Status = "Success", Message = "Specility has been updated successfully." });
            else if (Specility_Result == 0)
                return Json(new { Status = "Failure", Message = "There is already a specility with this name, Try with a different name. " });
            else
                return Json(new { Status = "Failure", Message = "Seems something is wrong! Please try again later." });
        }
        public ActionResult DeleteOnCallPeople(clsOnCallPeople objOnCall)
        {
            //objOnCall.UserId = 101;
            var Specility_Result = (new HomeBLL()).DeleteOnCallPeople(objOnCall);
            if (Specility_Result == 1)
                return Json(new { Status = "Success", Message = "OnCallPeople has been Delected successfully." });
            else if (Specility_Result == 2)
                return Json(new { Status = "Success", Message = "Specility has been updated successfully." });
            else if (Specility_Result == 0)
                return Json(new { Status = "Failure", Message = "There is already a specility with this name, Try with a different name. " });
            else
                return Json(new { Status = "Failure", Message = "Seems something is wrong! Please try again later." });
        }

        public ActionResult RemoveOnCallPerson(clsOnCall objOnCall)
        {
            var Specility_Result = (new HomeBLL()).RemoveOnCallPersonBLL(objOnCall);
            if (Specility_Result)
                return Json(new { Status = "Success"});
            else
                return Json(new { Status = "Failure", Message = "Seems something is wrong! Please try again later." });
        }



        public ActionResult DeleteUsers(clsUser objusers)
        {
           // objspeciality.UserId = 101;
            var Specility_Result = (new HomeBLL()).DeleteUsers(objusers);
            if (Specility_Result == 1)
                return Json(new { Status = "Success", Message = "User has been Delected successfully." });
            else if (Specility_Result == 2)
                return Json(new { Status = "Success", Message = "Specility has been updated successfully." });
            else if (Specility_Result == 0)
                return Json(new { Status = "Failure", Message = "There is already a specility with this name, Try with a different name. " });
            else
                return Json(new { Status = "Failure", Message = "Seems something is wrong! Please try again later." });
        }
        public ActionResult DeletePractices(clsPractices objPractice)
        {
            //objspeciality.UserId = 101;
            var Specility_Result = (new HomeBLL()).DeletePractices(objPractice);
            if (Specility_Result == 1)
                return Json(new { Status = "Success", Message = "Practices has been Delected successfully." });
            else if (Specility_Result == 2)
                return Json(new { Status = "Success", Message = "Specility has been updated successfully." });
            else if (Specility_Result == 0)
                return Json(new { Status = "Failure", Message = "There is already a specility with this name, Try with a different name. " });
            else
                return Json(new { Status = "Failure", Message = "Seems something is wrong! Please try again later." });
        }

        [HttpPost]
        public ActionResult Contact(clsContact contactProperties)
        {

            if (contactProperties != null && !string.IsNullOrEmpty(contactProperties.Email) && !string.IsNullOrEmpty(contactProperties.Message))
            {
                Mailer Emailer = new Mailer();
                Emailer.SendContactMailToTeam(ConfigurationManager.AppSettings["EmailTo"].ToString(), contactProperties.Name, contactProperties.Email, ConfigurationManager.AppSettings["Subject"].ToString(), ConfigurationManager.AppSettings["EmailFrom"].ToString(), contactProperties.phone, contactProperties.Message, ConfigurationManager.AppSettings["CC"].ToString(), ConfigurationManager.AppSettings["BCC"].ToString(), true, Server.MapPath(ConfigurationManager.AppSettings["EmailTemplatePath"].ToString() + "Template.html"));
                return Json(new { Status = "Success", Message = "Congratulation ! We got your contact request. We will soon get in touch with you." });
            }
            return Json(new { Status = "Failure", Message = "oops Something went wrong!" });
        }
    }
}
