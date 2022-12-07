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
    public class MonthlySpendingTypesController : BaseController
    {
        private KiinteistoDBEntities db = new KiinteistoDBEntities();

        // GET: MonthlySpendingTypes
        public ActionResult Index()
        {
            var monthlySpendingTypes = db.MonthlySpendingTypes.Include(m => m.Logins);
            return View(monthlySpendingTypes.ToList());
        }

        public ActionResult ChartContainerUnits(int? id)
        {
            MonthlySpendingTypes spendingTypes = db.MonthlySpendingTypes.Find(id);
            ViewBag.Vuosi = new SelectList(db.MonthlyTypeSpendingsByMonthUnitsChart.Where(i => i.Tyyppi == id), "Vuosi", "Vuosi");
            ViewBag.TypeName = spendingTypes.TypeName;

            ViewBag.TypeID = id;
            return PartialView();
        }
        [Route("MonthlySpendingTypes/ChartSpending/{id?}/{year?}")]
        public ActionResult ChartSpending(int id, int year)
        {
            MonthlySpendingTypes spendingTypes = db.MonthlySpendingTypes.Find(id);
            if (spendingTypes == null)
            {
                return HttpNotFound();
            }
            ViewBag.UnitName = spendingTypes.Unit;
            var monthSpendDataUnits = from sld in db.MonthlyTypeSpendingsByMonthUnitsChart
                                      where sld.Vuosi == year && sld.Tyyppi == id
                                      select sld;

            var monthSpendDataUnits2 = from sld in db.MonthlyTypeSpendingsByMonthUnitsChart
                                       where sld.Vuosi == (year - 1) && sld.Tyyppi == id
                                       select sld;

            var yearObject = monthSpendDataUnits.FirstOrDefault();
            if (yearObject != null)
            {
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
            }
            else
            {
                decimal[] yearValues = new decimal[12];
                yearValues[0] = 0;
                yearValues[1] = 0;
                yearValues[2] = 0;
                yearValues[3] = 0;
                yearValues[4] = 0;
                yearValues[5] = 0;
                yearValues[6] = 0;
                yearValues[7] = 0;
                yearValues[8] = 0;
                yearValues[9] = 0;
                yearValues[10] = 0;
                yearValues[11] = 0;
                ViewBag.Year = JsonConvert.SerializeObject(yearValues);
            }
            var yearObject2 = monthSpendDataUnits2.FirstOrDefault();
            if (yearObject2 != null)
            {
                decimal[] yearValues2 = new decimal[12];
                yearValues2[0] = yearObject2.Tammikuu;
                yearValues2[1] = yearObject2.Helmikuu;
                yearValues2[2] = yearObject2.Maaliskuu;
                yearValues2[3] = yearObject2.Huhtikuu;
                yearValues2[4] = yearObject2.Toukokuu;
                yearValues2[5] = yearObject2.Kesäkuu;
                yearValues2[6] = yearObject2.Heinäkuu;
                yearValues2[7] = yearObject2.Elokuu;
                yearValues2[8] = yearObject2.Syyskuu;
                yearValues2[9] = yearObject2.Lokakuu;
                yearValues2[10] = yearObject2.Marraskuu;
                yearValues2[11] = yearObject2.Joulukuu;
                ViewBag.Year2 = JsonConvert.SerializeObject(yearValues2);
            }
            else
            {
                decimal[] yearValues2 = new decimal[12];
                yearValues2[0] = 0;
                yearValues2[1] = 0;
                yearValues2[2] = 0;
                yearValues2[3] = 0;
                yearValues2[4] = 0;
                yearValues2[5] = 0;
                yearValues2[6] = 0;
                yearValues2[7] = 0;
                yearValues2[8] = 0;
                yearValues2[9] = 0;
                yearValues2[10] = 0;
                yearValues2[11] = 0;
                ViewBag.Year2 = JsonConvert.SerializeObject(yearValues2);
            }




            ViewBag.ThisYear = year;
            ViewBag.LastYear = year - 1;

            return PartialView();
        }
        public ActionResult ChartContainer(int? id)
        {
            MonthlySpendingTypes spendingTypes = db.MonthlySpendingTypes.Find(id);
            ViewBag.Vuosi = new SelectList(db.MonthlyTypeSpendingsByMonth.Where(i => i.Tyyppi == id), "Vuosi", "Vuosi");
            ViewBag.TypeName = spendingTypes.TypeName;
            ViewBag.TypeID = id;
            return PartialView();
        }

        [Route("MonthlySpendingTypes/Chart/{id?}/{year?}")]
        public ActionResult Chart(int id, int year)
        {
            MonthlySpendingTypes spendingTypes = db.MonthlySpendingTypes.Find(id);
            if (spendingTypes == null)
            {
                return HttpNotFound();
            }

            var monthSpendData = from sld in db.MonthlyTypeSpendingsByMonth
                                 where sld.Vuosi == year && sld.Tyyppi == id
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

            ViewBag.ThisYear = year;

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
            return PartialView("_CreateModal", monthlySpendingTypes);
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
            return PartialView("_EditModal", monthlySpendingTypes);
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
            return PartialView("_EditModal", monthlySpendingTypes);
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
            return PartialView("_DeleteModal", monthlySpendingTypes);
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
