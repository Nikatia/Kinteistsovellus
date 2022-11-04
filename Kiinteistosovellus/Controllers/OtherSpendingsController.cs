using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kiinteistosovellus.Models;

namespace Kiinteistosovellus.Controllers
{
    public class OtherSpendingsController : Controller
    {
        private KiinteistoDBEntities db = new KiinteistoDBEntities();

        // GET: OtherSpendings
        public ActionResult Index()
        {
            var otherSpendings = db.OtherSpendings.Include(o => o.Contractors).Include(o => o.Logins).Include(o => o.OtherSpendingTypes);
            return View(otherSpendings.ToList());
        }

        // GET: OtherSpendings/Create
        public ActionResult Create()
        {
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name");
            ViewBag.OtherSpendingTypeID = new SelectList(db.OtherSpendingTypes, "OtherSpendingTypeId", "TypeName");

            //---LATER ON INSTEAD OF HARD CODED ID HERE SHOULD BE CORRECT LOGINID---//
            ViewBag.LoginID = "1001";

            return View();
        }

        public ActionResult _ModalCreate()
        {
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name");
            ViewBag.OtherSpendingTypeID = new SelectList(db.OtherSpendingTypes, "OtherSpendingTypeId", "TypeName");

            //---LATER ON INSTEAD OF HARD CODED ID HERE SHOULD BE CORRECT LOGINID---//
            ViewBag.LoginID = "1001";

            return PartialView(); ;
        }


        // POST: OtherSpendings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult _ModalCreate([Bind(Include = "OtherSpendingsID,DateBegin,DateEnd,Description,OtherSpendingTypeID,ContractorID,Price,LoginID")] OtherSpendings otherSpendings)
        {

            if (ModelState.IsValid)
            {
                Console.WriteLine("IsValid");
                db.OtherSpendings.Add(otherSpendings);
                db.SaveChanges();
                return null; //palauttaa nullin, jotta ajax voi erotella onnistuneen lisäyksen epäonnistumisesta
            }

            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", otherSpendings.ContractorID);
            ViewBag.OtherSpendingTypeID = new SelectList(db.OtherSpendingTypes, "OtherSpendingTypeId", "TypeName", otherSpendings.OtherSpendingTypeID);
            return PartialView("/Views/OtherSpendings/_ModalCreate.cshtml", otherSpendings);
        }

        public PartialViewResult _ModalCreateOthSpendingType()//vain sitä varten, että modaalin avautuessa ajax-pyynnöllä luodaan partial view
        {
            //---LATER ON INSTEAD OF HARD CODED ID HERE SHOULD BE CORRECT LOGINID---//
            ViewBag.LoginID = "1001";

            return PartialView("/Views/OtherSpendings/_PartialOthSpendType.cshtml");
        }

        // POST: OtherSpendings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult _ModalCreateOthSpendingType([Bind(Include = "OtherSpendingTypeId,TypeName,LoginID")] OtherSpendingTypes otherSpendingType)
        {
            if (ModelState.IsValid)//On aina true jostain syystä PITÄÄ KORJATA!!!
            {
                Console.WriteLine("IsValid");
                db.OtherSpendingTypes.Add(otherSpendingType);
                db.SaveChanges();
                return null;
            }

            //---LATER ON INSTEAD OF HARD CODED ID HERE SHOULD BE CORRECT LOGINID---//
            ViewBag.LoginID = "1001";
            return PartialView("/Views/OtherSpendings/_PartialOthSpendType.cshtml", otherSpendingType);
        }

        // GET: OtherSpendings/Edit/5
        public ActionResult _ModalEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OtherSpendings otherSpendings = db.OtherSpendings.Find(id);
            if (otherSpendings == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", otherSpendings.ContractorID);
            ViewBag.OtherSpendingTypeID = new SelectList(db.OtherSpendingTypes, "OtherSpendingTypeId", "TypeName", otherSpendings.OtherSpendingTypeID);
            //decimal hinta = otherSpendings.Price;
            

            //---LATER ON INSTEAD OF HARD CODED ID HERE SHOULD BE CORRECT LOGINID---//
            ViewBag.LoginID = "1001";
            return PartialView("_ModalEdit", otherSpendings);
        }

        // POST: OtherSpendings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OtherSpendingsID,DateBegin,DateEnd,Description,OtherSpendingTypeID,ContractorID,Price,LoginID")] OtherSpendings otherSpendings)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(otherSpendings).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", otherSpendings.ContractorID);
            ViewBag.OtherSpendingTypeID = new SelectList(db.OtherSpendingTypes, "OtherSpendingTypeId", "TypeName", otherSpendings.OtherSpendingTypeID);
            ViewBag.DateBegin = otherSpendings.DateBegin;
            ViewBag.DateEnd = otherSpendings.DateEnd;
            //---LATER ON INSTEAD OF HARD CODED ID HERE SHOULD BE CORRECT LOGINID---//
            ViewBag.LoginID = "1001";

            return View("Edit", otherSpendings);
        }

        //GET: OtherSpendings/Delete/5
        public ActionResult _ModalDelete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OtherSpendings otherSpendings = db.OtherSpendings.Find(id);
            if (otherSpendings == null)
            {
                return HttpNotFound();
            }
            return PartialView("_ModalDelete", otherSpendings);
            
        }

        // POST: OtherSpendings/Delete/5
        [HttpPost, ActionName("_ModalDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult _ModalDeleteConfirmed(int id)
        {
            OtherSpendings otherSpendings = db.OtherSpendings.Find(id);
            db.OtherSpendings.Remove(otherSpendings);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public JsonResult GetList()
        {

            var itemlist = db.OtherSpendingTypes.ToList();
            var itemList = itemlist.Select(item => new SelectListItem { Text = item.TypeName, Value = Convert.ToString(item.OtherSpendingTypeId) }).ToList();
            return Json(new SelectList(itemList, "Value", "Text"));
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
