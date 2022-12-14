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
            if (Session["UserName"] != null)
            {
                var othSpendtype = db.OtherSpendingTypes;
                return View(await othSpendtype.ToListAsync());
            }
            else { return null; }
        }

        public ActionResult ChartContainer(int? id)
        {
            if (Session["UserName"] != null)
            {
                OtherSpendingTypes spendingTypes = db.OtherSpendingTypes.Find(id);
                ViewBag.Vuosi = new SelectList(db.OtherTypeSpendingsByMonth.Where(i => i.Tyyppi == id), "Vuosi", "Vuosi");
                ViewBag.TypeName = spendingTypes.TypeName;
                ViewBag.TypeID = id;
                return PartialView();
            }
            else { return null; }
        }

        [Route("OtherSpendingTypes/Chart/{id?}/{year?}")]
        public ActionResult Chart(int id, int year)
        {
            if (Session["UserName"] != null)
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
                ViewBag.TypeName = spendingTypes.TypeName;
                return PartialView();
            }
            else { return null; }
        }

        // GET: OtherSpendingTypes/Create
        public PartialViewResult Create()
        {
            if (Session["UserName"] != null)
            {
                return PartialView("/Views/OtherSpendingTypes/_CreateModal.cshtml");
            }
            else { return null; }
        }

        // POST: Plans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "OtherSpendingTypeId,TypeName")] OtherSpendingTypes othSpendType)
        {
            if (Session["UserName"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.OtherSpendingTypes.Add(othSpendType);
                    await db.SaveChangesAsync();
                    return null;
                }
                return PartialView("/Views/OtherSpendingTypes/_CreateModal.cshtml", othSpendType);
            }
            else { return null; }
        }

        // GET: OtherSpendingTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (Session["UserName"] != null)
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
                return PartialView("/Views/OtherSpendingTypes/_EditModal.cshtml", othSpendType);
            }
            else { return null; }
        }

        // POST: Plans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> Edit([Bind(Include = "OtherSpendingTypeId,TypeName")] OtherSpendingTypes othSpendType)
        {
            if (Session["UserName"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(othSpendType).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return null;
                }
                return PartialView("/Views/OtherSpendingTypes/_EditModal.cshtml", othSpendType);
            }
            else { return null; }
        }

        // GET: OtherSpendingTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (Session["UserName"] != null)
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
            else { return null; }
        }

        // POST: Plans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (Session["UserName"] != null)
            {
                OtherSpendingTypes othSpendType = await db.OtherSpendingTypes.FindAsync(id);
                db.OtherSpendingTypes.Remove(othSpendType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else { return null; }
        }
    }
}
