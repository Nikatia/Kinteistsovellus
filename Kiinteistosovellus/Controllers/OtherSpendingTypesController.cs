using Kiinteistosovellus.Models;
using Kiinteistosovellus.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kiinteistosovellus.Controllers
{
    public class OtherSpendingTypesController : BaseController
    {
        private KiinteistoDBEntities db = new KiinteistoDBEntities();

        // GET: OtherSpendingTypes
        public async Task<ActionResult> Index()
        {
            var othSpendtype = db.OtherSpendingTypes.Include(o => o.Logins);
            return View(await othSpendtype.ToListAsync());
        }

        public ActionResult ChartContainer(int? id)
        {
            OtherSpendingTypes spendingTypes = db.OtherSpendingTypes.Find(id);
            ViewBag.Vuosi = new SelectList(db.OtherTypeSpendingsByMonth.Where(i => i.Tyyppi == id), "Vuosi", "Vuosi");
            ViewBag.TypeName = spendingTypes.TypeName;
            ViewBag.TypeID = id;
            return PartialView();
        }

        [Route("OtherSpendingTypes/Chart/{id?}/{year?}")]
        public ActionResult Chart(int id, int year)
        {
            OtherSpendingTypes spendingTypes = db.OtherSpendingTypes.Find(id);
            if (spendingTypes == null)
            {
                return HttpNotFound();
            }

            var monthSpendData = from sld in db.OtherTypeSpendingsByMonth
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

        // GET: OtherSpendingTypes/Create
        public PartialViewResult Create()
        {
            ViewBag.LoginID = new SelectList(db.Logins, "LoginID", "UserName");
            return PartialView("/Views/OtherSpendingTypes/_CreateModal.cshtml");
        }

        // POST: Plans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "OtherSpendingTypeId,TypeName,LoginID")] OtherSpendingTypes othSpendType)
        {
            if (ModelState.IsValid)
            {
                db.OtherSpendingTypes.Add(othSpendType);
                await db.SaveChangesAsync();
                return null;
            }
            
            ViewBag.LoginID = new SelectList(db.Logins, "LoginID", "UserName", othSpendType.LoginID);
            return PartialView("/Views/OtherSpendingTypes/_CreateModal.cshtml", othSpendType);
        }

        // GET: OtherSpendingTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OtherSpendingTypes othSpendType = await db.OtherSpendingTypes.FindAsync(id);
            if (othSpendType == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.LoginID = new SelectList(db.Logins, "LoginID", "UserName", othSpendType.LoginID);
            return PartialView("/Views/OtherSpendingTypes/_EditModal.cshtml", othSpendType);
        }

        // POST: Plans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> Edit([Bind(Include = "OtherSpendingTypeId,TypeName,LoginID")] OtherSpendingTypes othSpendType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(othSpendType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return null;
            }
            
            ViewBag.LoginID = new SelectList(db.Logins, "LoginID", "UserName", othSpendType.LoginID);
            return PartialView("/Views/OtherSpendingTypes/_EditModal.cshtml", othSpendType);
        }

        // GET: OtherSpendingTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OtherSpendingTypes othSpendType = await db.OtherSpendingTypes.FindAsync(id);
            if (othSpendType == null)
            {
                return HttpNotFound();
            }
            return PartialView("/Views/OtherSpendingTypes/_DeleteModal.cshtml", othSpendType);
        }

        // POST: Plans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            OtherSpendingTypes othSpendType = await db.OtherSpendingTypes.FindAsync(id);
            db.OtherSpendingTypes.Remove(othSpendType);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
