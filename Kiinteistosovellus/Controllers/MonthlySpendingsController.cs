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

        // GET: MonthlySpendings/Create
        public ActionResult Create()
        {
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name");
            ViewBag.LoginID = new SelectList(db.Logins, "LoginID", "UserName");
            ViewBag.SpendingTypeID = new SelectList(db.MonthlySpendingTypes, "SpendingTypeID", "TypeName");
            return View();
        }

        // POST: MonthlySpendings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MonthlySpendingID,DateBegin,DateEnd,SpendingTypeID,AmountOfUnits,PricePerUnit,TransferPayment,FullPrice,ContractorID,LoginID")] MonthlySpendings monthlySpendings)
        {
            if (ModelState.IsValid)
            {
                db.MonthlySpendings.Add(monthlySpendings);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", monthlySpendings.ContractorID);
            ViewBag.LoginID = new SelectList(db.Logins, "LoginID", "UserName", monthlySpendings.LoginID);
            ViewBag.SpendingTypeID = new SelectList(db.MonthlySpendingTypes, "SpendingTypeID", "TypeName", monthlySpendings.SpendingTypeID);
            return View(monthlySpendings);
        }

        // GET: MonthlySpendings/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.LoginID = new SelectList(db.Logins, "LoginID", "UserName", monthlySpendings.LoginID);
            ViewBag.SpendingTypeID = new SelectList(db.MonthlySpendingTypes, "SpendingTypeID", "TypeName", monthlySpendings.SpendingTypeID);
            return View(monthlySpendings);
            
        }

        // POST: MonthlySpendings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MonthlySpendingID,DateBegin,DateEnd,SpendingTypeID,AmountOfUnits,PricePerUnit,TransferPayment,FullPrice,ContractorID,LoginID")] MonthlySpendings monthlySpendings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(monthlySpendings).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", monthlySpendings.ContractorID);
            ViewBag.LoginID = new SelectList(db.Logins, "LoginID", "UserName", monthlySpendings.LoginID);
            ViewBag.SpendingTypeID = new SelectList(db.MonthlySpendingTypes, "SpendingTypeID", "TypeName", monthlySpendings.SpendingTypeID);
            return View(monthlySpendings);
        }

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
            ViewBag.LoginID = new SelectList(db.Logins, "LoginID", "UserName", monthlySpendings.LoginID);
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
            ViewBag.LoginID = new SelectList(db.Logins, "LoginID", "UserName", monthlySpendings.LoginID);
            ViewBag.SpendingTypeID = new SelectList(db.MonthlySpendingTypes, "SpendingTypeID", "TypeName", monthlySpendings.SpendingTypeID);
            return PartialView("_EditModal",monthlySpendings);
        }

        // GET: MonthlySpendings/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: MonthlySpendings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
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
