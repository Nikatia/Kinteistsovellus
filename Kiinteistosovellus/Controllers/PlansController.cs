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
using Kiinteistosovellus.ViewModels;

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
            else { return RedirectToAction("Index", "Home"); }
        }

        // GET: Plans/Create
        public PartialViewResult Create()
        {
            ViewBag.MonthlyOrOther = new SelectList(db.KuukausittainenVaiMuu, "MonthOrOtherID", "MonthOrOtherName");
            return PartialView("/Views/Plans/_CreateModal.cshtml");
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
            if (Session["UserName"] != null)
            {
                Plans plans = await db.Plans.FindAsync(id);
                db.Plans.Remove(plans);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else { return null; }
        }

        public ActionResult _MoveOthPlan(int? id)
        {
            Plans plans = db.Plans.Find(id);
            ViewBag.DateBegin = plans.DateBegin.ToString("yyyy-MM-dd");
            ViewBag.DateEnd = plans.DateEnd?.ToString("yyyy-MM-dd");
            ViewBag.Price = plans.Price;
            ViewBag.Description = plans.Desciption;
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name");
            ViewBag.OtherSpendingTypeID = new SelectList(db.OtherSpendingTypes, "OtherSpendingTypeId", "TypeName");
            ViewBag.PlanId = id;

            return PartialView("/Views/Plans/_MoveOthPlan.cshtml");
        }

        // POST: OtherSpendings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult _MoveOthPlan([Bind(Include = "OtherSpendingsID,DateBegin,DateEnd,Description,OtherSpendingTypeID,ContractorID,Price,PlanToDelete")] MoveOthSpendings otherSpendings)
        {
            if (Session["UserName"] != null)
            {
                if (ModelState.IsValid)
                {
                    Console.WriteLine("IsValid");
                    if (otherSpendings.PlanToDelete == "true")
                    {
                        int planID = Convert.ToInt32(TempData["PlanID"]);
                        Plans plans = db.Plans.Find(planID);
                        db.Plans.Remove(plans);
                    }
                    OtherSpendings newSpending = new OtherSpendings();
                    newSpending.Description = otherSpendings.Description;
                    newSpending.DateBegin = otherSpendings.DateBegin;
                    newSpending.DateEnd = otherSpendings.DateEnd;
                    newSpending.OtherSpendingTypeID = otherSpendings.OtherSpendingTypeID;
                    newSpending.ContractorID = otherSpendings.ContractorID;
                    newSpending.Price = otherSpendings.Price;
                    db.OtherSpendings.Add(newSpending);
                    db.SaveChanges();
                    return null;
                }
                ViewBag.DateBegin = otherSpendings.DateBegin.ToString("yyyy-MM-dd");
                ViewBag.DateEnd = otherSpendings.DateEnd?.ToString("yyyy-MM-dd");
                ViewBag.Price = otherSpendings.Price;
                ViewBag.Description = otherSpendings.Description;
                ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", otherSpendings.ContractorID);
                ViewBag.OtherSpendingTypeID = new SelectList(db.OtherSpendingTypes, "OtherSpendingTypeId", "TypeName", otherSpendings.OtherSpendingTypeID);

                return PartialView("/Views/Plans/_MoveOthPlan.cshtml", otherSpendings);
            }
            else { return null; }
        }

        public ActionResult _MoveMonthPlan(int? id)
        {
            Plans plans = db.Plans.Find(id);
            ViewBag.DateBegin = plans.DateBegin.ToString("yyyy-MM-dd");
            ViewBag.DateEnd = plans.DateEnd?.ToString("yyyy-MM-dd");
            ViewBag.Price = plans.Price;
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name");
            ViewBag.SpendingTypeID = new SelectList(db.MonthlySpendingTypes, "SpendingTypeID", "TypeName");
            ViewBag.PlanId = id;

            return PartialView("/Views/Plans/_MoveMonthPlan.cshtml");
        }

        // POST: OtherSpendings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult _MoveMonthPlan([Bind(Include = "MonthlySpendingID,DateBegin,DateEnd,SpendingTypeID,AmountOfUnits,PricePerUnit,TransferPayment,FullPrice,ContractorID,PlanToDelete")] MoveMonthSpendings monthlySpendings)
        {
            if (Session["UserName"] != null)
            {
                if (ModelState.IsValid)
                {
                    Console.WriteLine("IsValid");
                    if (monthlySpendings.PlanToDelete == "true")
                    {
                        int planID = Convert.ToInt32(TempData["PlanID"]);
                        Plans plans = db.Plans.Find(planID);
                        db.Plans.Remove(plans);
                    }
                    MonthlySpendings newSpending = new MonthlySpendings();
                    newSpending.DateBegin = monthlySpendings.DateBegin;
                    newSpending.DateEnd = monthlySpendings.DateEnd;
                    newSpending.SpendingTypeID = monthlySpendings.SpendingTypeID;
                    newSpending.AmountOfUnits = monthlySpendings.AmountOfUnits;
                    newSpending.PricePerUnit = monthlySpendings.PricePerUnit;
                    newSpending.TransferPayment = monthlySpendings.TransferPayment;
                    newSpending.FullPrice = monthlySpendings.FullPrice;
                    newSpending.ContractorID = monthlySpendings.ContractorID;
                    db.MonthlySpendings.Add(newSpending);
                    db.SaveChanges();
                    return null;
                }
                ViewBag.DateBegin = monthlySpendings.DateBegin.ToString("yyyy-MM-dd");
                ViewBag.DateEnd = monthlySpendings.DateEnd?.ToString("yyyy-MM-dd");
                ViewBag.Price = monthlySpendings.FullPrice;
                ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", monthlySpendings.ContractorID);
                ViewBag.SpendingTypeID = new SelectList(db.MonthlySpendingTypes, "SpendingTypeID", "TypeName", monthlySpendings.SpendingTypeID);
                return PartialView("/Views/Plans/_MoveMonthPlan.cshtml", monthlySpendings);
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
