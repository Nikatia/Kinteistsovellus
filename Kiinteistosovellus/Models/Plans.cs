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
    
    public partial class Plans
    {
        public int PlandID { get; set; }

        [Required(ErrorMessage = "Nimi vaaditaan")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Päivämäärä vaaditaan")]
        public System.DateTime DateBegin { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }
        [Required(ErrorMessage = "Kuvaus vaaditaan")]
        public string Desciption { get; set; }
        [Required(ErrorMessage = "Hinta vaaditaan")]
        public decimal Price { get; set; }
        public Nullable<int> MonthlyOrOther { get; set; }
        [Required(ErrorMessage = "LoginID vaaditaan")]
        public int LoginID { get; set; }
    
        public virtual Logins Logins { get; set; }
    }
}
