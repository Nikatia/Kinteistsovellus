using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kiinteistosovellus.Models;

namespace Kiinteistosovellus.Controllers
{
    public class PlansController : Controller
    {
        private KiinteistoDBEntities db = new KiinteistoDBEntities();

        // GET: Plans
        public async Task<ActionResult> Index()
        {
            var plans = db.Plans.Include(p => p.Logins);
            return View(await plans.ToListAsync());
        }

        // GET: Plans/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plans plans = await db.Plans.FindAsync(id);
            if (plans == null)
            {
                return HttpNotFound();
            }
            return View(plans);
        }

        // GET: Plans/Create
        public PartialViewResult Create()
        {
            ViewBag.LoginID = new SelectList(db.Logins, "LoginID", "UserName");
            List<SelectListItem> slMonthOrOth = new List<SelectListItem>();
            slMonthOrOth.Add(new SelectListItem { Value="1", Text="Kuukausittainen meno"});
            slMonthOrOth.Add(new SelectListItem { Value = "2", Text = "Muu meno" });
            ViewBag.MonthlyOrOther = slMonthOrOth;
            return PartialView("/Views/Plans/_CreateModal.cshtml");
        }

        // POST: Plans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PlandID,Name,DateBegin,DateEnd,Desciption,Price,MonthlyOrOther,LoginID")] Plans plans)
        {
            if (ModelState.IsValid)
            {
                db.Plans.Add(plans);
                await db.SaveChangesAsync();
                return null ;
            }
            List<SelectListItem> slMonthOrOth = new List<SelectListItem>();
            slMonthOrOth.Add(new SelectListItem { Value = "1", Text = "Kuukausittainen meno" });
            slMonthOrOth.Add(new SelectListItem { Value = "2", Text = "Muu meno" });
            ViewBag.MonthlyOrOther = slMonthOrOth;
            ViewBag.LoginID = new SelectList(db.Logins, "LoginID", "UserName", plans.LoginID);
            return PartialView("/Views/Plans/_CreateModal.cshtml", plans);
        }

        // GET: Plans/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plans plans = await db.Plans.FindAsync(id);
            if (plans == null)
            {
                return HttpNotFound();
            }
            ViewBag.LoginID = new SelectList(db.Logins, "LoginID", "UserName", plans.LoginID);
            return PartialView("/Views/Plans/_EditModal.cshtml", plans);
        }

        // POST: Plans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> Edit([Bind(Include = "PlandID,Name,DateBegin,DateEnd,Desciption,Price,MonthlyOrOther,LoginID")] Plans plans)
        {
            if (ModelState.IsValid)
            {
                db.Entry(plans).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return null;
            }
            ViewBag.LoginID = new SelectList(db.Logins, "LoginID", "UserName", plans.LoginID);
            return PartialView("/Views/Plans/_EditModal.cshtml", plans);
        }

        // GET: Plans/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plans plans = await db.Plans.FindAsync(id);
            if (plans == null)
            {
                return HttpNotFound();
            }
            return PartialView("/Views/Plans/_DeleteModal.cshtml", plans);
        }

        // POST: Plans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Plans plans = await db.Plans.FindAsync(id);
            db.Plans.Remove(plans);
            await db.SaveChangesAsync();
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
