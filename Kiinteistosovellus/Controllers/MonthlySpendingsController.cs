using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kiinteistosovellus.Models;

namespace Kiinteistosovellus.Controllers
{
    public class MonthlySpendingsController : Controller
    {
        private KiinteistoDBEntities db = new KiinteistoDBEntities();
     
        // GET: MonthlySpendings
        public ActionResult Index()
        {
            ViewBag.Error = 0;
            var monthlySpendings = db.MonthlySpendings.Include(m => m.Contractors).Include(m => m.Logins).Include(m => m.MonthlySpendingTypes);
            return View(monthlySpendings.ToList());
        }

        // GET: MonthlySpendings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlySpendings monthlySpendings = db.MonthlySpendings.Find(id);
            if (monthlySpendings == null)
            {
                return HttpNotFound();
            }
            return View(monthlySpendings);
        }

      

        public PartialViewResult _CreateModal()
        {
            ViewBag.LoginID = "1000";
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name");
            ViewBag.SpendingTypeID = new SelectList(db.MonthlySpendingTypes, "SpendingTypeID", "TypeName");
            return PartialView("_CreateModal");
        }
        // POST: MonthlySpendings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _CreateModal([Bind(Include = "MonthlySpendingID,DateBegin,DateEnd,SpendingTypeID,AmountOfUnits,PricePerUnit,TransferPayment,FullPrice,ContractorID,LoginID")] MonthlySpendings monthlySpendings)
        {
            ViewBag.LoginID = "1000";
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", monthlySpendings.ContractorID);

            ViewBag.SpendingTypeID = new SelectList(db.MonthlySpendingTypes, "SpendingTypeID", "TypeName", monthlySpendings.SpendingTypeID);
            if (ModelState.IsValid)
            {
                ViewBag.Error = 0;
                db.MonthlySpendings.Add(monthlySpendings);
                db.SaveChanges();

                //return RedirectToAction("Index");
                return null;
            }
            ViewBag.Error = 1;
            return PartialView("_CreateModal", monthlySpendings);
        }

            //return View("Create", monthlySpendings);

            //return PartialView("_CreateModal",monthlySpendings);
        //    return RedirectToAction("Index");
        //}
        //// POST: MonthlySpendings/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult _CreateModal([Bind(Include = "MonthlySpendingID,DateBegin,DateEnd,SpendingTypeID,AmountOfUnits,PricePerUnit,TransferPayment,FullPrice,ContractorID,LoginID")] MonthlySpendings monthlySpendings)
        //{
        //    ViewBag.LoginID = "1000";
        //    ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", monthlySpendings.ContractorID);

        //    ViewBag.SpendingTypeID = new SelectList(db.MonthlySpendingTypes, "SpendingTypeID", "TypeName", monthlySpendings.SpendingTypeID);
        //    if (ModelState.IsValid)
        //    {
        //        ViewBag.Error = 0;
        //        db.MonthlySpendings.Add(monthlySpendings);
        //        db.SaveChanges();

        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.Error = 1;

        //    //return View("Create", monthlySpendings);

        //    //return PartialView("_CreateModal",monthlySpendings);
        //    return RedirectToAction("Index");
        //}

        public ActionResult _EditModal(int? id)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlySpendings monthlySpendings = db.MonthlySpendings.Find(id);
            if (monthlySpendings == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", monthlySpendings.ContractorID);
            ViewBag.LoginID = "1000";
            ViewBag.SpendingTypeID = new SelectList(db.MonthlySpendingTypes, "SpendingTypeID", "TypeName", monthlySpendings.SpendingTypeID);
            return PartialView("_EditModal",monthlySpendings);


        }

        // POST: MonthlySpendings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _EditModal([Bind(Include = "MonthlySpendingID,DateBegin,DateEnd,SpendingTypeID,AmountOfUnits,PricePerUnit,TransferPayment,FullPrice,ContractorID,LoginID")] MonthlySpendings monthlySpendings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(monthlySpendings).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", monthlySpendings.ContractorID);
            ViewBag.LoginID = "1000";
            ViewBag.SpendingTypeID = new SelectList(db.MonthlySpendingTypes, "SpendingTypeID", "TypeName", monthlySpendings.SpendingTypeID);
            //return PartialView("_EditModal",monthlySpendings);
           // return RedirectToAction("Index");
           return View("Edit",monthlySpendings);
        }

       

        public ActionResult _DeleteModal(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlySpendings monthlySpendings = db.MonthlySpendings.Find(id);
            if (monthlySpendings == null)
            {
                return HttpNotFound();
            }
            return PartialView("_DeleteModal",monthlySpendings);
        }

        // POST: MonthlySpendings/Delete/5
        [HttpPost, ActionName("_DeleteModal")]
        [ValidateAntiForgeryToken]
        public ActionResult _DeleteModalConfirmed(int id)
        {
            MonthlySpendings monthlySpendings = db.MonthlySpendings.Find(id);
            db.MonthlySpendings.Remove(monthlySpendings);
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
