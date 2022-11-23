using Kiinteistosovellus.Models;
using Kiinteistosovellus.ViewModels;
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

        public ActionResult Chart(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OtherSpendingTypes otherSpendingTypes = db.OtherSpendingTypes.Find(id);
            if (otherSpendingTypes == null)
            {
                return HttpNotFound();
            }

            //CHART 1
            string dateList;
            string priceList;
            List<ForOtherSpendingTypeChartsClass> spendingList = new List<ForOtherSpendingTypeChartsClass>();

            var spendingListData = from sld in db.ForOtherSpendingTypeCharts
                                   select sld;
            
            foreach (ForOtherSpendingTypeCharts spending in spendingListData)
            {
                if (spending.OtherSpendingTypeId == id)
                {
                    
                    ForOtherSpendingTypeChartsClass OneRow = new ForOtherSpendingTypeChartsClass();
                    OneRow.DateBegin = spending.DateBegin.ToString("dd.MM.yyyy");
                    OneRow.DailySpendings = (int)spending.DailySpendings;
                    spendingList.Add(OneRow);
                }
            }

            dateList = "'" + string.Join("','", spendingList.Select(n => n.DateBegin).ToList()) + "'";
            priceList = string.Join(",", spendingList.Select(n => n.DailySpendings).ToList());

            OtherSpendingTypes othSpendType = db.OtherSpendingTypes.Find(id);

            ViewBag.TypeName = othSpendType.TypeName;
            ViewBag.Dates = dateList;
            ViewBag.Price = priceList;

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
