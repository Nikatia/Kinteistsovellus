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
    public class MonthlySpendingTypesController : Controller
    {
        private KiinteistoDBEntities db = new KiinteistoDBEntities();

        // GET: MonthlySpendingTypes
        public ActionResult Index()
        {
            var monthlySpendingTypes = db.MonthlySpendingTypes.Include(m => m.Logins);
            return View(monthlySpendingTypes.ToList());
        }

        // GET: MonthlySpendingTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlySpendingTypes monthlySpendingTypes = db.MonthlySpendingTypes.Find(id);
            if (monthlySpendingTypes == null)
            {
                return HttpNotFound();
            }
            return View(monthlySpendingTypes);
        }

        // GET: MonthlySpendingTypes/Create
        public ActionResult _CreateModal()
        {
            ViewBag.LoginID = new SelectList(db.Logins, "LoginID", "UserName");
            return PartialView("_CreateModal");
        }

        // POST: MonthlySpendingTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _CreateModal([Bind(Include = "SpendingTypeID,TypeName,Unit,LoginID")] MonthlySpendingTypes monthlySpendingTypes)
        {
            if (ModelState.IsValid)
            {
                db.MonthlySpendingTypes.Add(monthlySpendingTypes);
                db.SaveChanges();
                return null;
                //return RedirectToAction("Index");
            }

            ViewBag.LoginID = new SelectList(db.Logins, "LoginID", "UserName", monthlySpendingTypes.LoginID);
            return PartialView("_CreateModal",monthlySpendingTypes);
        }

        // GET: MonthlySpendingTypes/Edit/5
        public ActionResult _EditModal(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlySpendingTypes monthlySpendingTypes = db.MonthlySpendingTypes.Find(id);
            if (monthlySpendingTypes == null)
            {
                return HttpNotFound();
            }
            ViewBag.LoginID = new SelectList(db.Logins, "LoginID", "UserName", monthlySpendingTypes.LoginID);
            return PartialView("_EditModal",monthlySpendingTypes);
        }

        // POST: MonthlySpendingTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _EditModal([Bind(Include = "SpendingTypeID,TypeName,Unit,LoginID")] MonthlySpendingTypes monthlySpendingTypes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(monthlySpendingTypes).State = EntityState.Modified;
                db.SaveChanges();
                return null;
                //return RedirectToAction("Index");
            }
            ViewBag.LoginID = new SelectList(db.Logins, "LoginID", "UserName", monthlySpendingTypes.LoginID);
            return PartialView("_EditModal",monthlySpendingTypes);
        }

        // GET: MonthlySpendingTypes/Delete/5
        public ActionResult _DeleteModal(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlySpendingTypes monthlySpendingTypes = db.MonthlySpendingTypes.Find(id);
            if (monthlySpendingTypes == null)
            {
                return HttpNotFound();
            }
            return PartialView("_DeleteModal",monthlySpendingTypes);
        }

        // POST: MonthlySpendingTypes/Delete/5
        [HttpPost, ActionName("_DeleteModal")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MonthlySpendingTypes monthlySpendingTypes = db.MonthlySpendingTypes.Find(id);
            db.MonthlySpendingTypes.Remove(monthlySpendingTypes);
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
