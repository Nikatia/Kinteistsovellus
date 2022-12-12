using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiinteistosovellus.ViewModels
{
    public class AllContractorsData
    {
        public int ContractorID { get; set; }
        public string Name { get; set; }
        public string ContractorsDescription { get; set; }
        public string StreetAdress { get; set; }
        public int PostID { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public int PersonID { get; set; }
        public int ContactID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonsDescription { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}