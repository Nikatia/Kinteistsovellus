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
using System.Globalization;
using System.Threading;
using System.Drawing;

namespace Kiinteistosovellus.Controllers
{
    public class PlansController : BaseController
    {
        private KiinteistoDBEntities db = new KiinteistoDBEntities();

        // GET: Plans
        public async Task<ActionResult> Index()
        {
            if (Session["UserName"] != null)
            {
                var plans = db.Plans;
                return View(await plans.ToListAsync());
            }
            else { return null; }
        }

        // GET: Plans/Create
        public PartialViewResult Create()
        {
            if (Session["UserName"] != null)
            {
                ViewBag.MonthlyOrOther = new SelectList(db.KuukausittainenVaiMuu, "MonthOrOtherID", "MonthOrOtherName");
                return PartialView("/Views/Plans/_CreateModal.cshtml");
            }
            else { return null; }
        }

        // POST: Plans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PlandID,Name,DateBegin,DateEnd,Desciption,Price,MonthOrOtherID")] Plans plans)
        {
            if (Session["UserName"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Plans.Add(plans);
                    await db.SaveChangesAsync();
                    return null;
                }
                ViewBag.MonthlyOrOther = new SelectList(db.KuukausittainenVaiMuu, "MonthOrOtherID", "MonthOrOtherName", plans.MonthOrOtherID);
                return PartialView("/Views/Plans/_CreateModal.cshtml", plans);
            }
            else { return null; }
        }

        // GET: Plans/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (Session["UserName"] != null)
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

                ViewBag.MonthlyOrOther = new SelectList(db.KuukausittainenVaiMuu, "MonthOrOtherID", "MonthOrOtherName", plans.MonthOrOtherID);
                return PartialView("/Views/Plans/_EditModal.cshtml", plans);
            }
            else { return null; }
        }

        // POST: Plans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> Edit([Bind(Include = "PlandID,Name,DateBegin,DateEnd,Desciption,Price,MonthOrOtherID")] Plans plans)
        {
            if (Session["UserName"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(plans).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return null;
                }
                ViewBag.MonthlyOrOther = new SelectList(db.KuukausittainenVaiMuu, "MonthOrOtherID", "MonthOrOtherName", plans.MonthOrOtherID);
                return PartialView("/Views/Plans/_EditModal.cshtml", plans);
            }
            else { return null; }
        }

        // GET: Plans/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (Session["UserName"] != null)
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
            else { return null; }
        }

        // POST: Plans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (Session["UserName"] != null)
            {
                Plans plans = await db.Plans.FindAsync(id);
                db.Plans.Remove(plans);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else { return null; }
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
