//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Beta_Centauri.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class tblLogIn
    {
        public int LogInId { get; set; }
        [Required(ErrorMessage = "Please Enter UserName")]
        public string Username { get; set; }
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        [Required(ErrorMessage = "Email Id should be Unique")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter Password")]

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
