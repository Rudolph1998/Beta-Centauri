using Beta_Centauri.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Beta_Centauri.ViewModels
{
    public class IssueDetails
    {
       
        public Nullable<int> StudentId { get; set; }

        public string Name { get; set; }

        public Nullable<int> CourseId { get; set; }

        public string CertificateNo { get; set; }
        public string Certificate { get; set; }
       
           
        public HttpPostedFileBase ImageFile { get; set; }

        public virtual tblCourse tblCourse { get; set; }

    }
}