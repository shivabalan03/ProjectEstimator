using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectEstimation.Models;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ProjectEstimation.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(user ud)
        {
            string message = "";
            try
            {
                projectEstimatorEntities pe = new projectEstimatorEntities();
                user u = new user();
                int isUser = (from us in pe.users where us.userName == ud.userName && us.password == ud.password select us).Count();
                if (isUser > 0)
                {
                    message = "Loggin Successfully..!";
                }
                else
                {
                    message = "Please Check your credentials";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddNewUser(user ud)
        {
            string message = "";
            try
            {
                projectEstimatorEntities pe = new projectEstimatorEntities();
                user u = new user();
                int checkUser = (from us in pe.users where us.userName == ud.userName select us).Count();
                if (checkUser == 0)
                {
                    u.userName = ud.userName;
                    u.password = ud.password;
                    pe.users.Add(u);
                    pe.SaveChanges();
                    message = "User Added Successfully..!";
                }
                else
                {
                    message = "User Already Exist..!";
                }

            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string addProjectDetails()
        {
            int iUploadedCnt = 0;

            // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
            string sPath = "";
            sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/");

            System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

            // CHECK THE FILE COUNT.
            for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
            {
                System.Web.HttpPostedFile hpf = hfc[iCnt];

                if (hpf.ContentLength > 0)
                {
                    // CHECK IF THE SELECTED FILE(S) ALREADY EXISTS IN FOLDER. (AVOID DUPLICATE)
                    //if (!File.Exists(sPath + Path.GetFileName(hpf.FileName)))
                    //{
                        // SAVE THE FILES IN THE FOLDER.
                        hpf.SaveAs(sPath + Path.GetFileName(hpf.FileName));
                        iUploadedCnt = iUploadedCnt + 1;
                    //}
                }
            }

            // RETURN A MESSAGE.
            if (iUploadedCnt > 0)
            {
                return iUploadedCnt + " Files Uploaded Successfully";
            }
            else
            {
                return "Upload Failed";
            }
        }
    }
}