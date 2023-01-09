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
        public List<OtherSpendings> GetSpendings()
        {
            if (Session["UserName"] != null)
            {
                List<OtherSpendings> spendings = db.OtherSpendings.ToList();
                return spendings;
            }
            else { return null; }
        }

        // GET: OtherSpendingTypes
        public async Task<ActionResult> Index()
        {
            if (Session["UserName"] != null)
            {
                var othSpendtype = db.OtherSpendingTypes;
                ViewBag.Spendings = GetSpendings();
                return View(await othSpendtype.ToListAsync());
            }
            else { return RedirectToAction("Index", "Home"); }
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
                decimal[] yearValues = new decimal[12];
                var yearObject = monthSpendData.FirstOrDefault();
                if (yearObject == null)
                {
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
                }
                else
                {
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
                }

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
            return PartialView("/Views/OtherSpendingTypes/_CreateModal.cshtml");
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
                var typeExists = from l in db.OtherSpendingTypes
                                 where othSpendType.TypeName.ToString() == l.TypeName.ToString()
                                 select l;
                int count = typeExists.Count();
                if (ModelState.IsValid)
                {
                    if (count > 0)
                    {
                        ViewBag.Error = "Kulutustyyppi on jo olemassa!";
                        return PartialView("/Views/OtherSpendingTypes/_CreateModal.cshtml", othSpendType);
                    }
                    else
                    {
                        db.OtherSpendingTypes.Add(othSpendType);
                        await db.SaveChangesAsync();
                        return null;
                    }
                }
                return PartialView("/Views/OtherSpendingTypes/_CreateModal.cshtml", othSpendType);
            }
            else { return null; }
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
            return PartialView("/Views/OtherSpendingTypes/_EditModal.cshtml", othSpendType);
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
            if (id == null || id == 1000)
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
            if (Session["UserName"] != null)
            {
                OtherSpendingTypes othSpendType = await db.OtherSpendingTypes.FindAsync(id);
                foreach (var item in db.OtherSpendings)
                {
                    if (item.OtherSpendingTypeID == id)
                    {
                        item.OtherSpendingTypeID = 1000;
                    }
                }
                db.OtherSpendingTypes.Remove(othSpendType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else { return null; }
        }
    }
}
