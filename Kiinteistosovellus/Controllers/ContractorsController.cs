using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Kiinteistosovellus.Models;
using Kiinteistosovellus.ViewModels;

namespace Kiinteistosovellus.Controllers
{
    public class ContractorsController : Controller
    {
        private KiinteistoDBEntities db = new KiinteistoDBEntities();

        // ----------------------------------------------- GET LISTS -----------------------------------------------

        public List<Persons> GetPersons()
        {
            List<Persons> persons = db.Persons.ToList();
            return persons;
        }

        public List<Contacts> GetContacts()
        {
            List<Contacts> contacts = db.Contacts.ToList();
            return contacts;
        }

        // ----------------------------------------------- INDEX -----------------------------------------------

        // GET: Contractors
        public ActionResult Index()
        {
            ViewBag.Persons = GetPersons();
            ViewBag.Contacts = GetContacts();

            var wholeContractorsList = from ctr in db.Contractors
                                       join pst in db.Post on ctr.PostID equals pst.PostID
                                       select new AllContractorsData
                                       {
                                           ContractorID = (int)ctr.ContractorID,
                                           Name = ctr.Name,
                                           ContractorsDescription = ctr.Description,
                                           StreetAdress = ctr.StreetAdress,
                                           PostID = ctr.PostID,
                                           PostCode = pst.PostCode,
                                           City = pst.City,
                                           Country = pst.Country,
                                       };

            return View(wholeContractorsList.ToList());
        }

        // ----------------------------------------------- CREATE PART -----------------------------------------------

        //------------------------------------Contractors------------------------------------
        // GET: Contractors/Create
        public PartialViewResult CreateContractor()
        {
            ViewBag.PostID = new SelectList(db.Post, "PostID", "PostCode");

            //---LATER ON INSTEAD OF HARD CODED ID HERE SHOULD BE CORRECT LOGINID---//
            ViewBag.LoginID = "1001";

            return PartialView("/Views/Contractors/_ModalCreate.cshtml");
        }

        // POST: Contractors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateContractor([Bind(Include = "ContractorID,Name,Description,StreetAdress,PostID,LoginID")] Contractors contractors)
        {

            if (ModelState.IsValid)
            {
                db.Contractors.Add(contractors);
                await db.SaveChangesAsync();
                return null;
            }

            //---LATER ON INSTEAD OF HARD CODED ID HERE SHOULD BE CORRECT LOGINID---//
            ViewBag.LoginID = "1001";
            ViewBag.PostID = new SelectList(db.Post, "PostID", "PostCode");
            return PartialView("/Views/Contractors/_ModalCreate.cshtml", contractors);
        }


        //------------------------------------Contacts------------------------------------

        // GET: Contacts/Create
        public PartialViewResult CreateContact()
        {

            //---LATER ON INSTEAD OF HARD CODED ID HERE SHOULD BE CORRECT LOGINID---//
            ViewBag.LoginID = "1001";
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name");
            ViewBag.PersonID = new SelectList(db.Persons, "PersonID", "FullName");
            return PartialView("/Views/Contractors/_ModalCreateContact.cshtml");
        }

        // POST: Contractors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateContact([Bind(Include = "ContactID,ContractorID,PersonID,PhoneNumber,Email,LoginID")] Contacts contacts)
        {

            if (ModelState.IsValid)
            {
                db.Contacts.Add(contacts);
                await db.SaveChangesAsync();
                return null;
            }

            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", contacts);
            ViewBag.LoginID = "1001";
            ViewBag.PersonID = new SelectList(db.Persons, "PersonID", "FullName", contacts);
            //---LATER ON INSTEAD OF HARD CODED ID HERE SHOULD BE CORRECT LOGINID---//
            ViewBag.LoginID = "1001";

            return PartialView("/Views/Contractors/_ModalCreateContact.cshtml", contacts);
        }



        //------------------------------------Persons------------------------------------
        // GET: Contacts/Create
        public PartialViewResult CreatePerson()
        {

            //---LATER ON INSTEAD OF HARD CODED ID HERE SHOULD BE CORRECT LOGINID---//
            ViewBag.LoginID = "1001";
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name");
            return PartialView("/Views/Contractors/_ModalCreatePerson.cshtml");
        }

        // POST: Contractors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreatePerson([Bind(Include = "PersonID,FirstName,LastName,ContractorID,LoginID,Description")] Persons persons)
        {

            if (ModelState.IsValid)
            {
                db.Persons.Add(persons);
                await db.SaveChangesAsync();
                return null;
            }

            //---LATER ON INSTEAD OF HARD CODED ID HERE SHOULD BE CORRECT LOGINID---//
            ViewBag.LoginID = "1001";
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", persons.ContractorID);
            return PartialView("/Views/Contractors/_ModalCreatePerson.cshtml", persons);
        }

        // ----------------------------------------------- EDIT PART -----------------------------------------------

        //------------------------------------Contractors------------------------------------

        // GET: Contractors/Edit/5
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
            ViewBag.LoginID = "1001";
            ViewBag.PostID = new SelectList(db.Post, "PostID", "PostCode", contractors.PostID);
            return PartialView("/Views/Contractors/_ModalEdit.cshtml", contractors);
        }

        // POST: Contractors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult EditContractor([Bind(Include = "ContractorID,Name,Description,StreetAdress,PostID,LoginID")] Contractors contractors)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contractors).State = EntityState.Modified;
                db.SaveChanges();
                return null;
            }
            ViewBag.LoginID = "1001";
            ViewBag.PostID = new SelectList(db.Post, "PostID", "PostCode", contractors.PostID);
            return PartialView("/Views/Contractors/_ModalEdit.cshtml", contractors);
        }


        // ----------------------------------------------- DELETE PART -----------------------------------------------



        // GET: Contractors/Delete/5
        public ActionResult _ModalDelete(int? id)
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
        [HttpPost, ActionName("_ModalDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult _ModalDeleteConfirmed(int id)
        {
            Contractors contractors = db.Contractors.Find(id);

            //changing ContractorID in Monthly and Other Spendings to empty one
            foreach (var item in db.OtherSpendings)
            {
                if (item.ContractorID == id)
                {
                    item.ContractorID = 1004;
                }
            }
            foreach (var item in db.MonthlySpendings)
            {
                if (item.ContractorID == id)
                {
                    item.ContractorID = 1004;
                }
            }
            //deleting contractor together with it's persons and their contact informations
            db.Contacts.RemoveRange(db.Contacts.Where(c => c.ContractorID == id));
            db.Persons.RemoveRange(db.Persons.Where(p => p.ContractorID == id));
            db.Contractors.Remove(contractors);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Contractors/Delete/5
        public ActionResult _ModalPersonDelete(int? id)
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
            return PartialView("_ModalPersonDelete", persons);
        }

        // POST: Contractors/Delete/5
        [HttpPost, ActionName("_ModalPersonDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult _ModalPersonDeleteConfirmed(int id)
        {
            Persons persons = db.Persons.Find(id);

            //deleting contractor together with it's persons and their contact informations
            db.Contacts.RemoveRange(db.Contacts.Where(c => c.PersonID == id));
            db.Persons.Remove(persons);
            db.SaveChanges();
            return RedirectToAction("Index");
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
