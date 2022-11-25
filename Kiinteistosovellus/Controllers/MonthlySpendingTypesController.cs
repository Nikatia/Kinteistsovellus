using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kiinteistosovellus.Models;
using Kiinteistosovellus.ViewModels;
using Newtonsoft.Json;

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

        public ActionResult Chart(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlySpendingTypes spendingTypes = db.MonthlySpendingTypes.Find(id);
            if (spendingTypes == null)
            {
                return HttpNotFound();
            }

            //CHART
            int? thisYear = DateTime.Now.Year;
            var monthSpendData = from sld in db.MonthlyTypeSpendingsByMonth
                                    where sld.Vuosi == thisYear && sld.Tyyppi == id
                                    select sld;

            var yearObject = monthSpendData.FirstOrDefault();

            decimal[] yearValues = new decimal[12];
            yearValues[0] = yearObject.Tammikuu;
            yearValues[1] = yearObject.Helmikuu;
            yearValues[2] = yearObject.Maaliskuu;
            yearValues[3] = yearObject.Huhtikuu;
            yearValues[4] = yearObject.Toukokuu;
            yearValues[5] = yearObject.Kesäkuu;
            yearValues[6] = yearObject.Heinäkuu;
            yearValues[7] = yearObject.Elokuu;
            yearValues[8] = yearObject.Syyskuu;
            yearValues[9] = yearObject.Lokakuu;
            yearValues[10] = yearObject.Marraskuu;
            yearValues[11] = yearObject.Joulukuu;

            ViewBag.Year = JsonConvert.SerializeObject(yearValues);


            ViewBag.TypeName = spendingTypes.TypeName;

            return PartialView();
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
