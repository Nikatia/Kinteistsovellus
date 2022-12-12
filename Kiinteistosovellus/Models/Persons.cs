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
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Persons
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Persons()
        {
            this.Contacts = new HashSet<Contacts>();
        }
    
        public int PersonID { get; set; }

        [Required(ErrorMessage = "vaaditaan!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "vaaditaan!")]
        public string LastName { get; set; }

        [NotMapped]
        public string FullName { get { return this.FirstName + " " + this.LastName; } }

        [Required(ErrorMessage = "vaaditaan!")]
        public int ContractorID { get; set; }

        public int LoginID { get; set; }

        public string Description { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contacts> Contacts { get; set; }
        public virtual Contractors Contractors { get; set; }
        public virtual Logins Logins { get; set; }
    }
}
