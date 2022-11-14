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


    public partial class Contractors
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Contractors()
        {
            this.Contacts = new HashSet<Contacts>();
            this.MonthlySpendings = new HashSet<MonthlySpendings>();
            this.OtherSpendings = new HashSet<OtherSpendings>();
            this.Persons = new HashSet<Persons>();
        }
    

        public int ContractorID { get; set; }
        [Required(ErrorMessage = "Nimi vaaditaan")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Osoite vaaditaan")]
        public string StreetAdress { get; set; }
        [Required(ErrorMessage = "Postinumero vaaditaan")]
        public int PostID { get; set; }
        public int LoginID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contacts> Contacts { get; set; }
        public virtual Logins Logins { get; set; }
        public virtual Post Post { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MonthlySpendings> MonthlySpendings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OtherSpendings> OtherSpendings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Persons> Persons { get; set; }
    }
}
