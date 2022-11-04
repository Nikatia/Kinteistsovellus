using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiinteistosovellus.ViewModels
{
    public class PersonsContacts
    {
        public int ContractorID { get; set; }
        public int PersonID { get; set; }
        public int ContactID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int LoginID { get; set; }
    }
}