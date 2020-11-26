using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Beta_Centauri.Models;
using System.Web.Security;
namespace Beta_Centauri.Controllers
{
    public class IndexController : Controller
    {
        private BetaDBEntities2 db = new BetaDBEntities2();

        // GET: Index
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Enquiry()
        {
            ViewBag.CourseId = new SelectList(db.tblCourses, "CourseId", "CourseName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Enquiry([Bind(Include = "Name,Phone,Email,RefferBy,Query,EnquiryId,CourseId ")]  string name, string phone, string email, string RefferBy ,tblEnquiry tblEnquiry)
        {
            if (tblEnquiry.Name !=null)
            {
                MailMessage mail = new MailMessage();
                mail.To.Add("training@betacentauri.in");
                mail.From = new MailAddress("betaemail17@gmail.com");
                mail.Subject = "Notification about Enquiry" + "training@betacentauri.in";
                string userMessage = "<b>Some one wants to join<b><hr/>";
                mail.Body = userMessage
                + "<b> Name:</b>" + name + "<br/>"
                + "<b>Phone no:</b>" + phone + "<br/>"
                + "<b>Email:</b>" + email + "<br/>"
                + "<b>Reffer by:</b>" + RefferBy + "<br/>";


                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("betaemail17@gmail.com", "betademo123");
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
           
            if (ModelState.IsValid)
            {
                db.tblEnquiries.Add(tblEnquiry);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(db.tblCourses, "CourseId", "CourseName", tblEnquiry.CourseId);
            return View(tblEnquiry);
        }
       
        public ActionResult Registration()
        {
            ViewBag.BloodGroupId = new SelectList(db.tblBloodGroups, "BloodGroupId", "BloodGroup");
            ViewBag.BranchId = new SelectList(db.tblBranches, "BranchId", "BranchName");
            ViewBag.CollegeId = new SelectList(db.tblColleges, "CollegeId", "CollegeName");
            ViewBag.CourseId = new SelectList(db.tblCourses, "CourseId", "CourseName");
            ViewBag.DistrictId = new SelectList(db.tblDistricts, "DistrictId", "DistrictName");
            ViewBag.StateId = new SelectList(db.tblStates, "StateId", "StateName");
            ViewBag.StreamId = new SelectList(db.tblStreams, "StreamId", "StreamName");
            return View();
        }

        // POST: tblRegistrations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Include = "StudentId,CenterCode,Name,StateId,DistrictId,CollegeId,Year,Semester,StreamId,BranchId,CourseId,Mobile,DOB,Gender,BloodGroupId,EmailId,FatherName,ContactNo,Address,UploadPhoto,ImageFile,CreatedBy,CreatedDate,IsApproved")] tblRegistration tblRegistration)
        {
            if (tblRegistration.ImageFile != null)
            {
                string Filename = Path.GetFileNameWithoutExtension(tblRegistration.ImageFile.FileName);
                string extension = Path.GetExtension(tblRegistration.ImageFile.FileName);
                Filename = Filename + DateTime.Now.ToString("yymmssfff") + extension;
                tblRegistration.UploadPhoto = "~/StudentImages/" + Filename;
                Filename = Path.Combine(Server.MapPath("~/StudentImages/"), Filename);
                tblRegistration.ImageFile.SaveAs(Filename);
            }
            
            if (ModelState.IsValid)
            {
                db.tblRegistrations.Add(tblRegistration);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BloodGroupId = new SelectList(db.tblBloodGroups, "BloodGroupId", "BloodGroup", tblRegistration.BloodGroupId);
            ViewBag.BranchId = new SelectList(db.tblBranches, "BranchId", "BranchName", tblRegistration.BranchId);
            ViewBag.CollegeId = new SelectList(db.tblColleges, "CollegeId", "CollegeName", tblRegistration.CollegeId);
            ViewBag.CourseId = new SelectList(db.tblCourses, "CourseId", "CourseName", tblRegistration.CourseId);
            ViewBag.DistrictId = new SelectList(db.tblDistricts, "DistrictId", "DistrictName", tblRegistration.DistrictId);
            ViewBag.StateId = new SelectList(db.tblStates, "StateId", "StateName", tblRegistration.StateId);
            ViewBag.StreamId = new SelectList(db.tblStreams, "StreamId", "StreamName", tblRegistration.StreamId);
            return View(tblRegistration);
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Basic()
        {
            return View();
        }
        public ActionResult Web()
        {
            return View();
        }
        public ActionResult Mobile()
        {
            return View();
        }
        public ActionResult Embad()
        {
            return View();
        }
        public ActionResult Database()
        {
            return View();
        }
        public ActionResult Other()
        {
            return View();
        }
        public ActionResult Internship()
        {
            return View();
        }
        public ActionResult Team()
        {
            return View();
        }
        public ActionResult Event()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(string name, string phone, string email, string message)
        {
            if(name != null && phone != null)
            {
                MailMessage mail = new MailMessage();
                mail.To.Add("training@betacentauri.in");
                mail.From = new MailAddress("sangram.developer@gmail.com");
                mail.Subject = "Enquiry" + "training@betacentauri.in";

                mail.Body = "<b>Name:</b>" + name + "<br/>"
                + "<b>Email:</b>" + email + "<br/>"
                + "<b>Phone no:</b>" + phone + "<br/>"
                + "<b>Message:</b>" + message;

                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("sangram.developer@gmail.com", "Sangram@9632");

                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
           
           

            return View("Index");
        }

       
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(tblLogIn model)
        {
            using (BetaDBEntities2 db = new BetaDBEntities2())
            {
                var UserTypeId = db.tblLogIns.Where(a => a.Username == model.Username && a.Password == model.Password).FirstOrDefault();
                if (UserTypeId == null)
                {
                    ViewBag.LoginErrorMessage = "Invalid Username or Password";
                    return View("LogIn");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(model.Username, false);
                    Session["UserName"] = model.Username;
                    return RedirectToAction("Index" ,"Admin");
                    
                }
            }
            
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
    
}