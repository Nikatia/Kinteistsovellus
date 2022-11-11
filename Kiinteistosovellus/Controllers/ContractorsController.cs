﻿using System;
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
        public ActionResult _Persons(int? contractorId)
        {
            var contractorContactList = from p in db.Persons
                                    join cnt in db.Contacts on p.PersonID equals cnt.PersonID
                                    join ctr in db.Contractors on p.ContractorID equals ctr.ContractorID
                                    where p.ContractorID == contractorId
                                    //orderby//
                                    select new AllContractorsData
                                    {
                                        ContractorID = (int)p.ContractorID,
                                        PersonID = p.PersonID,
                                        ContactID = (int)cnt.ContactID,
                                        FirstName = p.FirstName,
                                        LastName = p.LastName,
                                        PhoneNumber = cnt.PhoneNumber,
                                        Email = cnt.Email,
                                        LoginID = cnt.LoginID,
                                    };
            ViewBag.ContractorId = contractorId;
            return PartialView(contractorContactList);
        }

        // GET: Contacts/Create
        public PartialViewResult CreateContact(int? contractorId)
        {

            //---LATER ON INSTEAD OF HARD CODED ID HERE SHOULD BE CORRECT LOGINID---//
            ViewBag.LoginID = "1001";
            ViewBag.ContractorID = contractorId;
            Contacts contacts = db.Contacts.First(n => n.ContractorID == contractorId);
            ViewBag.PersonID = new SelectList(db.Persons, "PersonID", "FullName");
            return PartialView("/Views/Contractors/_ModalCreateContact.cshtml", contacts);
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

            ViewBag.ContractorID = contacts;
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
            ViewBag.LoginID = "1001";
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", person.ContractorID);
            return PartialView("/Views/Contractors/_ModalEditPerson.cshtml", person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult EditPerson([Bind(Include = "PersonID,FirstName,LastName,ContractorID,LoginID,Description")] Persons person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return null;
            }
            ViewBag.LoginID = "1001";
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", person.ContractorID);
            return PartialView("/Views/Contractors/_ModalEditPerson.cshtml", person);
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
            ViewBag.LoginID = "1001";
            ViewBag.PersonID = contact.PersonID;
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", contact.ContractorID);
            return PartialView("/Views/Contractors/_ModalEditContact.cshtml", contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult EditContact([Bind(Include = "ContactID,ContractorID,PersonID,PhoneNumber,Email,LoginID")] Contacts contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return null;
            }
            ViewBag.LoginID = "1001";
            ViewBag.PersonID = contact.PersonID;
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", contact.ContractorID);
            return PartialView("/Views/Contractors/_ModalEditContact.cshtml", contact);
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
            Persons persons = db.Persons.Find(id);

            //deleting contractor together with it's persons and their contact informations
            db.Contacts.RemoveRange(db.Contacts.Where(c => c.PersonID == id));
            db.Persons.Remove(persons);
            db.SaveChanges();
            return RedirectToAction("Index");
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
            Contacts contact = db.Contacts.Find(id);
            db.Contacts.Remove(contact);
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
