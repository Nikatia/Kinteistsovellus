using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Kiinteistosovellus.Models;

namespace Kiinteistosovellus.Controllers
{
    public class MonthlySpendingsController : BaseController
    {
        private KiinteistoDBEntities db = new KiinteistoDBEntities();
     
        // GET: MonthlySpendings
        public ActionResult Index()
        {
            ViewBag.Error = 0;
            var monthlySpendings = db.MonthlySpendings.Include(m => m.Contractors).Include(m => m.Logins).Include(m => m.MonthlySpendingTypes);
            return View(monthlySpendings.ToList());
        }

        // GET: MonthlySpendings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlySpendings monthlySpendings = db.MonthlySpendings.Find(id);
            if (monthlySpendings == null)
            {
                return HttpNotFound();
            }
            return View(monthlySpendings);
        }

      

        public ActionResult _CreateModal()
        {
            ViewBag.LoginID = "1000";
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name");
            ViewBag.SpendingTypeID = new SelectList(db.MonthlySpendingTypes, "SpendingTypeID", "TypeName");
            return PartialView("/Views/MonthlySpendings/_CreateModal.cshtml");
        }
        // POST: MonthlySpendings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _CreateModal([Bind(Include = "MonthlySpendingID,DateBegin,DateEnd,SpendingTypeID,AmountOfUnits,PricePerUnit,TransferPayment,FullPrice,ContractorID,LoginID")] MonthlySpendings monthlySpendings)
        {
            ViewBag.LoginID = "1000";
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", monthlySpendings.ContractorID);

            ViewBag.SpendingTypeID = new SelectList(db.MonthlySpendingTypes, "SpendingTypeID", "TypeName", monthlySpendings.SpendingTypeID);
            if (ModelState.IsValid)
            {
                ViewBag.Error = 0;
                db.MonthlySpendings.Add(monthlySpendings);
                db.SaveChanges();
                return null;
                

            }
            ViewBag.Error = 1;
            return PartialView("/Views/MonthlySpendings/_CreateModal.cshtml", monthlySpendings);
        }

        public PartialViewResult _ModalCreateMonthSpendingType()//vain sitä varten, että modaalin avautuessa ajax-pyynnöllä luodaan partial view
        {
            //---LATER ON INSTEAD OF HARD CODED ID HERE SHOULD BE CORRECT LOGINID---//
            ViewBag.LoginID = "1001";
            ViewBag.SuccessMsg = "";
            return PartialView("/Views/MonthlySpendings/_PartialViewMonthSpendType.cshtml");
        }

        // POST: OtherSpendings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult _ModalCreateMonthSpendingType([Bind(Include = "SpendingTypeID,TypeName,Unit,LoginID")] MonthlySpendingTypes monthSpendingType)
        {
            ViewBag.SuccessMsg = "";

            if (ModelState.IsValid)//On aina true jostain syystä PITÄÄ KORJATA!!!
            {
                Console.WriteLine("IsValid");
                db.MonthlySpendingTypes.Add(monthSpendingType);
                db.SaveChanges();
                ViewBag.SuccessMsg = "successfully added";
                return PartialView("/Views/MonthlySpendings/_PartialViewMonthSpendType.cshtml"); //Tässä pitää palauttaa näkymä!!!
            }

            //---LATER ON INSTEAD OF HARD CODED ID HERE SHOULD BE CORRECT LOGINID---//
            ViewBag.LoginID = "1001";
            return PartialView("/Views/MonthlySpendings/_PartialViewMonthSpendType.cshtml", monthSpendingType);
        }
        public ActionResult _EditModal(int? id)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlySpendings monthlySpendings = db.MonthlySpendings.Find(id);
            if (monthlySpendings == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", monthlySpendings.ContractorID);
            ViewBag.LoginID = "1000";
            ViewBag.SpendingTypeID = new SelectList(db.MonthlySpendingTypes, "SpendingTypeID", "TypeName", monthlySpendings.SpendingTypeID);
            return PartialView("_EditModal",monthlySpendings);


        }

        // POST: MonthlySpendings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _EditModal([Bind(Include = "MonthlySpendingID,DateBegin,DateEnd,SpendingTypeID,AmountOfUnits,PricePerUnit,TransferPayment,FullPrice,ContractorID,LoginID")] MonthlySpendings monthlySpendings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(monthlySpendings).State = EntityState.Modified;
                db.SaveChanges();
                return null;
                //  return RedirectToAction("Index");
            }
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", monthlySpendings.ContractorID);
            ViewBag.LoginID = "1000";
            ViewBag.SpendingTypeID = new SelectList(db.MonthlySpendingTypes, "SpendingTypeID", "TypeName", monthlySpendings.SpendingTypeID);
           return PartialView("_EditModal",monthlySpendings);
           // return RedirectToAction("Index");
           //return View("Edit",monthlySpendings);
        }

       

        public ActionResult _DeleteModal(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlySpendings monthlySpendings = db.MonthlySpendings.Find(id);
            if (monthlySpendings == null)
            {
                return HttpNotFound();
            }
            return PartialView("_DeleteModal",monthlySpendings);
        }

        // POST: MonthlySpendings/Delete/5
        [HttpPost, ActionName("_DeleteModal")]
        [ValidateAntiForgeryToken]
        public ActionResult _DeleteModalConfirmed(int id)
        {
            MonthlySpendings monthlySpendings = db.MonthlySpendings.Find(id);
            db.MonthlySpendings.Remove(monthlySpendings);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetList()
        {

            var itemlist = db.MonthlySpendingTypes.ToList();
            var itemList = itemlist.Select(item => new SelectListItem { Text = item.TypeName, Value = Convert.ToString(item.SpendingTypeID) }).ToList();
            return Json(new SelectList(itemList, "Value", "Text"));
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
