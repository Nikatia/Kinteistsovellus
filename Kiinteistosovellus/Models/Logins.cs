//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kiinteistosovellus.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Logins
    {
        public int LoginID { get; set; }
        [Required(ErrorMessage = "vaaditaan!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "vaaditaan!")]
        public string UserPassword { get; set; }

        [Required(ErrorMessage = "vaaditaan!")]
        public int RoleID { get; set; }

        public string LoginErrorMessage { get; set; }

        public virtual Roles Roles { get; set; }
    }
}
