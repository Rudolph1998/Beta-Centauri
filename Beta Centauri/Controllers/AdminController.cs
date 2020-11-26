using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.text.html.simpleparser;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Beta_Centauri.Models;
using Beta_Centauri.ViewModels;

namespace Beta_Centauri.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private BetaDBEntities2 db = new BetaDBEntities2();

        public object XMLWorkerHelper { get; private set; }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ApprovedStudent()
        {
            var tblRegistrations = db.tblRegistrations.Include(t => t.tblBloodGroup).Include(t => t.tblBranch).Include(t => t.tblCollege).Include(t => t.tblCourse).Include(t => t.tblDistrict).Include(t => t.tblState).Include(t => t.tblStream);
            return View(tblRegistrations.ToList());
        }
        public ActionResult ManageStudent()
        {
            var tblRegistrations = db.tblRegistrations.Include(t => t.tblBloodGroup).Include(t => t.tblBranch).Include(t => t.tblCollege).Include(t => t.tblCourse).Include(t => t.tblDistrict).Include(t => t.tblState).Include(t => t.tblStream);
            return View(tblRegistrations.ToList());
        }
        public ActionResult IssueCertificate(int? id)
        {
            IssueDetails issueDetails = new IssueDetails();


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCertificate tblCertificate = db.tblCertificates.Where(x => x.StudentId == id).FirstOrDefault();

            var registrationData = db.tblRegistrations.Where(a => a.StudentId == id).FirstOrDefault();



            issueDetails = (from a in db.tblRegistrations

                            where a.StudentId == id
                            select new IssueDetails
                            {
                                StudentId = a.StudentId,

                                Name=a.Name,

                                CourseId=a.CourseId,

                                Certificate = a.Certificate,

                                CertificateNo = a.CertificateNo
                            }).FirstOrDefault();


            if (issueDetails == null)
            {
                return HttpNotFound();
            }
           
            
            ViewBag.CourseId = new SelectList(db.tblCourses, "CourseId", "CourseName", issueDetails.CourseId);
           
            return View(issueDetails);
        }

        // POST: tblRegistrations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IssueCertificate(IssueDetails registrationData)
        {
            var img = registrationData.ImageFile.FileName;
            string Filename = Path.GetFileNameWithoutExtension(img);
            string extension = Path.GetExtension(registrationData.ImageFile.FileName);
            Filename = Filename + DateTime.Now.ToString("yymmssfff") + extension;
            registrationData.Certificate = "~/Certificate/" + Filename;
            Filename = Path.Combine(Server.MapPath("~/Certificate/"), Filename);
            registrationData.ImageFile.SaveAs(Filename);
            if (registrationData != null)
            {
                tblCertificate certificate = new tblCertificate();
                certificate.StudentId = registrationData.StudentId;
                certificate.CertificateNo = registrationData.CertificateNo;
                certificate.Certificate = registrationData.Certificate;

                db.tblCertificates.Add(certificate);
                db.SaveChanges();
                return RedirectToAction("ApprovedStudent");
            }

           
            return View(registrationData);
        }
        public ActionResult DownlordCertificate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult DownlordCertificate(int? Studentid)
        {
            using (BetaDBEntities2 db = new BetaDBEntities2())
            {
                var User = db.tblCertificates.Where(a => a.StudentId == Studentid).FirstOrDefault();
                string fullPath = Path.Combine(User.Certificate);
                //using (MemoryStream stream = new System.IO.MemoryStream())
                //{
                //    StringReader sr = new StringReader(fullPath);
                //    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                //    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                //    pdfDoc.Open();
                //    XMLWorkerHelper.GetType().(writer, pdfDoc, sr);
                //    pdfDoc.Close();
                    return File(fullPath, "image/jpg", "Certificate.jpg");
                
            }
        }

        // GET: tblRegistrations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblRegistration tblRegistration = db.tblRegistrations.Find(id);
            if (tblRegistration == null)
            {
                return HttpNotFound();
            }
            return View(tblRegistration);
        }

        // GET: tblRegistrations/Create
      

        // GET: tblRegistrations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblRegistration tblRegistration = db.tblRegistrations.Find(id);
            if (tblRegistration == null)
            {
                return HttpNotFound();
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

        // POST: tblRegistrations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentId,CenterCode,Name,StateId,DistrictId,CollegeId,Year,Semester,StreamId,BranchId,CourseId,Mobile,DOB,Gender,BloodGroupId,EmailId,FatherName,ContactNo,Address,UploadPhoto,CreatedBy,CreatedDate,IsApproved")] tblRegistration tblRegistration)
        {

            if (ModelState.IsValid)
            {

                tblRegistration.CreatedBy = Session["UserName"].ToString();
                
                tblRegistration.CreatedDate = DateTime.Now;

                db.Entry(tblRegistration).State = EntityState.Modified;
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

        // GET: tblRegistrations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblRegistration tblRegistration = db.tblRegistrations.Find(id);
            if (tblRegistration == null)
            {
                return HttpNotFound();
            }
            return View(tblRegistration);
        }

        // POST: tblRegistrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblRegistration tblRegistration = db.tblRegistrations.Find(id);
            db.tblRegistrations.Remove(tblRegistration);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}