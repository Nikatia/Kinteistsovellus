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
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime DateBegin { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateEnd { get; set; }

        public string Desciption { get; set; }

        [Required(ErrorMessage = "Hinta vaaditaan")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }


        public int LoginID { get; set; }
        public Nullable<int> MonthOrOtherID { get; set; }
    
        public virtual Logins Logins { get; set; }
        public virtual KuukausittainenVaiMuu KuukausittainenVaiMuu { get; set; }
    }
}
