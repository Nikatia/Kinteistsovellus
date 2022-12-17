using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kiinteistosovellus.Models;
using Kiinteistosovellus.ViewModels;
using Microsoft.Ajax.Utilities;

namespace Kiinteistosovellus.Controllers
{
    public class ContractorsController : BaseController
    {
        private KiinteistoDBEntities db = new KiinteistoDBEntities();

        // ----------------------------------------------- GET LISTS -----------------------------------------------

        public List<Contractors> GetContractors()
        {
            if (Session["UserName"] != null)
            {
                List<Contractors> contractors = db.Contractors.ToList();
                return contractors;
            }
            else { return null; }
        }

        public List<Persons> GetPersons()
        {
            if (Session["UserName"] != null)
            {
                List<Persons> persons = db.Persons.ToList();
                return persons;
            }
            else
            {
                return null;
            }

        }

        public List<Persons> GetNoContactPersons()
        {
            if (Session["UserName"] != null)
            {
                var noContactPerson = from p in db.Persons where !(from c in db.Contacts select c.PersonID).Contains(p.PersonID) select p;
                List<Persons> persons = noContactPerson.ToList();
                return persons;
            }
            else
            {
                return null;
            }

        }

        public List<Contacts> GetContacts()
        {
            if (Session["UserName"] != null)
            {
                List<Contacts> contacts = db.Contacts.ToList();
                return contacts;
            }
            else { return null; }
        }

        [HttpPost]
        public JsonResult GetList()
        {
            if (Session["UserName"] != null)
            {
                var itemlist = db.Persons.ToList();
                var itemList = itemlist.Select(item => new SelectListItem { Text = item.FullName, Value = Convert.ToString(item.PersonID) }).ToList();
                return Json(new SelectList(itemList, "Value", "Text"));
            }
            else { return null; }
        }

        // ----------------------------------------------- INDEX -----------------------------------------------

        // GET: Contractors
        public ActionResult Index()
        {
            if (Session["UserName"] != null)
            {
                ViewBag.Persons = GetPersons();
                ViewBag.Contacts = GetContacts();
                ViewBag.NoContactPersons = GetNoContactPersons();

                var wholeContractorsList = from ctr in db.Contractors
                                           select new AllContractorsData
                                           {
                                               ContractorID = (int)ctr.ContractorID,
                                               Name = ctr.Name,
                                               ContractorsDescription = ctr.Description,
                                               StreetAdress = ctr.StreetAdress,
                                               PostCode = ctr.PostCode,
                                               City = ctr.City,
                                               Country = ctr.Country,
                                           };

                return View(wholeContractorsList.ToList());
            }
            else { return RedirectToAction("Index", "Home"); }
        }

        public PartialViewResult _NoContactPersons(int? id)
        {
            var personexist = db.Contacts.Any(x => x.PersonID == id);
            if (personexist)
            {
                ViewBag.PersonExists = true;
            }
            else
            {
                ViewBag.PersonExists = false;
            }

            return PartialView("/Views/Contractors/_NoContactPersons.cshtml");
        }
        // ----------------------------------------------- CREATE PART -----------------------------------------------

        //------------------------------------Contractors------------------------------------
        // GET: Contractors/Create
        public PartialViewResult CreateContractor()
        {
            return PartialView("/Views/Contractors/_ModalCreate.cshtml");
        }

        // POST: Contractors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateContractor([Bind(Include = "ContractorID,Name,Description,StreetAdress, PostCode, City, Country")] Contractors contractors)
        {

            if (Session["UserName"] != null)
            {

                if (ModelState.IsValid)
                {
                    db.Contractors.Add(contractors);
                    await db.SaveChangesAsync();
                    return null;
                }
                return PartialView("/Views/Contractors/_ModalCreate.cshtml", contractors);
            }
            else
            {
                return null;
            }
        }


        //------------------------------------Contacts------------------------------------

        [HttpGet]
        public JsonResult FetchContractors()
        {
            if (Session["UserName"] != null)
            {
                var data = GetContractors().Select(c => new { Value = c.ContractorID, Text = c.Name });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        public JsonResult FetchPersons(int ID)
        {
            if (Session["UserName"] != null)
            {
                var data = GetPersons().Where(c => c.ContractorID == ID).Select(p => new { Value = p.PersonID, Text = p.FullName });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else { return null; }
        }

        // GET: Contacts/Create
        public PartialViewResult CreateContact(int? id)
        {
            if (Session["UserName"] != null)
            {
                ViewBag.ContractorYes = id;
                Contractors contractor = db.Contractors.Find(id);
                ViewBag.ContractorName = contractor.Name;
                ViewBag.PersonID = new SelectList(db.Persons, "PersonID", "FullName");

                ViewBag.Contractors = GetContractors();
                return PartialView("/Views/Contractors/_ModalCreateContact.cshtml");
            }
            else { return PartialView("/Views/Contractors/_ModalCreateContact.cshtml"); }
        }

        // POST: Contractors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult CreateContact([Bind(Include = "ContactID,ContractorID,PersonID,PhoneNumber,Email")] Contacts contacts)
        {
            if (Session["UserName"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Contacts.Add(contacts);
                    db.SaveChanges();
                    return null;
                }

                ViewBag.Contractors = GetContractors();
                ViewBag.PersonID = new SelectList(db.Persons, "PersonID", "ContractorsPerson", contacts.PersonID);
                ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", contacts.ContractorID);

                return PartialView("/Views/Contractors/_ModalCreateContact.cshtml", contacts);
            }
            else { return null; }
        }

        [Route("Contractors/CreateContactPerson/{contractor?}/{person?}")]
        public PartialViewResult CreateContactPerson(int? contractor, int? person)
        {
            if (Session["UserName"] != null)
            {
                ViewBag.ContractorYes = contractor;
                Contractors contractors = db.Contractors.Find(contractor);
                ViewBag.ContractorName = contractors.Name;
                ViewBag.PersonID = person;
                Persons persons = db.Persons.Find(person);
                ViewBag.FullName = persons.FullName;

                ViewBag.Contractors = GetContractors();
                return PartialView("/Views/Contractors/_ModalCreateContactPerson.cshtml");
            }
            else { return PartialView("/Views/Contractors/_ModalCreateContactPerson.cshtml"); }
        }

        // POST: Contractors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult CreateContactPerson([Bind(Include = "ContactID,ContractorID,PersonID,PhoneNumber,Email")] Contacts contacts)
        {
            if (Session["UserName"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Contacts.Add(contacts);
                    db.SaveChanges();
                    return null;
                }

                ViewBag.Contractors = GetContractors();
                ViewBag.PersonID = new SelectList(db.Persons, "PersonID", "ContractorsPerson", contacts.PersonID);
                ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", contacts.ContractorID);

                return PartialView("/Views/Contractors/_ModalCreateContact.cshtml", contacts);
            }
            else { return null; }
        }


        //------------------------------------Persons------------------------------------
        // GET: Contacts/Create
        public PartialViewResult CreatePerson(int? id)
        {
            if (Session["UserName"] != null)
            {
                ViewBag.Contractor = id;
                ViewBag.SuccessMsg = "";
                return PartialView("/Views/Contractors/_ModalCreatePerson.cshtml");
            }
            else { return PartialView("/Views/Home/_NotLogged.cshtml"); }
        }

        // POST: Contractors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult CreatePerson([Bind(Include = "PersonID,FirstName,LastName,ContractorID,Description")] Persons persons)
        {
            if (Session["UserName"] != null)
            {
                ViewBag.Contractor = persons.ContractorID;
                ViewBag.SuccessMsg = "";
                if (ModelState.IsValid)
                {
                    db.Persons.Add(persons);
                    db.SaveChanges();
                    ViewBag.SuccessMsg = "successfully added";
                    return PartialView("/Views/Contractors/_ModalCreatePerson.cshtml");
                }
                ViewBag.SuccessMsg = "";
                return PartialView("/Views/Contractors/_ModalCreatePerson.cshtml", persons);
            }
            else
            {
                return PartialView("/Views/Home/_NotLogged.cshtml");
            }
        }

        // ----------------------------------------------- EDIT PART -----------------------------------------------

        //------------------------------------Contractors------------------------------------
        public ActionResult EditContractor(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contractors contractors = db.Contractors.Find(id);
            if (contractors == null)
            {
                return HttpNotFound();
            }
            return PartialView("/Views/Contractors/_ModalEdit.cshtml", contractors);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult EditContractor([Bind(Include = "ContractorID,Name,Description,StreetAdress, PostCode, City, Country")] Contractors contractors)
        {
            if (Session["UserName"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(contractors).State = EntityState.Modified;
                    db.SaveChanges();
                    return null;
                }
                return PartialView("/Views/Contractors/_ModalEdit.cshtml", contractors);
            }
            else { return null; }
        }

        //------------------------------------Persons------------------------------------
        public ActionResult EditPerson(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persons person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", person.ContractorID);
            return PartialView("/Views/Contractors/_ModalEditPerson.cshtml", person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult EditPerson([Bind(Include = "PersonID,FirstName,LastName,ContractorID,Description")] Persons person)
        {
            if (Session["UserName"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(person).State = EntityState.Modified;
                    db.SaveChanges();
                    return null;
                }
                ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", person.ContractorID);
                return PartialView("/Views/Contractors/_ModalEditPerson.cshtml", person);
            }
            else { return null; }
        }

        //------------------------------------Contacts------------------------------------
        public ActionResult EditContact(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contacts contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonID = contact.PersonID;
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", contact.ContractorID);
            return PartialView("/Views/Contractors/_ModalEditContact.cshtml", contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult EditContact([Bind(Include = "ContactID,ContractorID,PersonID,PhoneNumber,Email")] Contacts contact)
        {
            if (Session["UserName"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(contact).State = EntityState.Modified;
                    db.SaveChanges();
                    return null;
                }
                ViewBag.PersonID = contact.PersonID;
                ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", contact.ContractorID);
                return PartialView("/Views/Contractors/_ModalEditContact.cshtml", contact);
            }
            else
            {
                return null;
            }
        }

        // ----------------------------------------------- DELETE PART -----------------------------------------------


        //------------------------------------Contractors------------------------------------
        // GET: Contractors/Delete/5
        public ActionResult DeleteContractor(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contractors contractors = db.Contractors.Find(id);
            ViewBag.Persons = GetPersons();
            if (contractors == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContractorID = id;
            return PartialView("_ModalDelete", contractors);

        }

        // POST: Contractors/Delete/5
        [HttpPost, ActionName("DeleteContractor")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteContractorConfirmed(int id)
        {
            if (Session["UserName"] != null)
            {
                Contractors contractors = db.Contractors.Find(id);

                //changing ContractorID in Monthly and Other Spendings to empty one
                foreach (var item in db.OtherSpendings)
                {
                    if (item.ContractorID == id)
                    {
                        item.ContractorID = null;
                    }
                }
                foreach (var item in db.MonthlySpendings)
                {
                    if (item.ContractorID == id)
                    {
                        item.ContractorID = null;
                    }
                }
                //deleting contractor together with it's persons and their contact informations
                db.Contacts.RemoveRange(db.Contacts.Where(c => c.ContractorID == id));
                db.Persons.RemoveRange(db.Persons.Where(p => p.ContractorID == id));
                db.Contractors.Remove(contractors);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else { return PartialView("/Views/Home/_NotLoggedModal.cshtml"); }
        }

        //------------------------------------Persons------------------------------------
        // GET: Contractors/Delete/5
        public ActionResult DeletePerson(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persons persons = db.Persons.Find(id);
            ViewBag.Contacts = GetContacts();
            if (persons == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonID = id;
            return PartialView("_ModalDeletePerson", persons);
        }

        // POST: Contractors/Delete/5
        [HttpPost, ActionName("DeletePerson")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePersonConfirmed(int id)
        {
            if (Session["UserName"] != null)
            {
                Persons persons = db.Persons.Find(id);

                //deleting contractor together with it's persons and their contact informations
                db.Contacts.RemoveRange(db.Contacts.Where(c => c.PersonID == id));
                db.Persons.Remove(persons);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else { return PartialView("/Views/Home/_NotLoggedModal.cshtml"); }
        }

        //------------------------------------Contacts------------------------------------
        public ActionResult DeleteContact(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contacts contact = db.Contacts.Find(id);
            ViewBag.Persons = GetPersons();
            if (contact == null)
            {
                return HttpNotFound();
            }
            return PartialView("_ModalDeleteContact", contact);
        }

        // POST: Contractors/Delete/5
        [HttpPost, ActionName("DeleteContact")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteContactConfirmed(int id)
        {
            if (Session["UserName"] != null)
            {
                Contacts contact = db.Contacts.Find(id);
                db.Contacts.Remove(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else { return PartialView("/Views/Home/_NotLoggedModal.cshtml"); }
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
