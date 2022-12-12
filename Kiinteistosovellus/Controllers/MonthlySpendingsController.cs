using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Kiinteistosovellus.Models;
using Kiinteistosovellus.ViewModels;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

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
            var distinctYearList = db.MonthlyTypeSpendingsByMonth.DistinctBy(x => x.Vuosi).ToList();
            ViewBag.Vuosi = new SelectList(distinctYearList, "Vuosi", "Vuosi");

            var kulutyypit = db.MonthlySpendingTypes.ToArray();
            string[] monthTypeArray = new string[kulutyypit.Length];
            for (int i = 0; i < kulutyypit.Length; i++)
            {
                if (!monthTypeArray.Contains(kulutyypit[i].TypeName.ToString()))
                {
                    monthTypeArray[i] = kulutyypit[i].TypeName.ToString();
                }
            }
            ViewBag.Kulutyypit = monthTypeArray;

            return View(monthlySpendings.ToList());
        }

        public ActionResult Chart(int year)
        {
            decimal tammikuu = 0, helmikuu = 0, maaliskuu = 0, huhtikuu = 0, toukokuu = 0, kesäkuu = 0, heinäkuu = 0, elokuu = 0, syyskuu = 0, lokakuu = 0, marraskuu = 0, joulukuu = 0;
            var monthSpendData = from sld in db.MonthlyTypeSpendingsByMonth
                                 where sld.Vuosi == year
                                 select sld;

            foreach(var m in monthSpendData)
            {
                if (m.Vuosi == year)
                {
                    tammikuu = tammikuu + m.Tammikuu;
                    helmikuu = helmikuu + m.Helmikuu;
                    maaliskuu = maaliskuu + m.Maaliskuu;
                    huhtikuu = huhtikuu + m.Huhtikuu;
                    toukokuu = toukokuu + m.Toukokuu;
                    kesäkuu = kesäkuu + m.Kesäkuu;
                    heinäkuu = heinäkuu + m.Heinäkuu;
                    elokuu = elokuu + m.Elokuu;
                    syyskuu = syyskuu + m.Syyskuu;
                    lokakuu = lokakuu + m.Lokakuu;
                    marraskuu = marraskuu + m.Marraskuu;
                    joulukuu = joulukuu + m.Joulukuu;
                }
            }
            decimal[] yearValues = new decimal[12];
            yearValues[0] = tammikuu;
            yearValues[1] = helmikuu;
            yearValues[2] = maaliskuu;
            yearValues[3] = huhtikuu;
            yearValues[4] = toukokuu;
            yearValues[5] = kesäkuu;
            yearValues[6] = heinäkuu;
            yearValues[7] = elokuu;
            yearValues[8] = syyskuu;
            yearValues[9] = lokakuu;
            yearValues[10] = marraskuu;
            yearValues[11] = joulukuu;

            ViewBag.Year = JsonConvert.SerializeObject(yearValues);
            ViewBag.ThisYear = year;

            return PartialView();
        }

        public ActionResult BarChart(int year)
        {
            string typeList;
            string priceList;
            List<ForMonthlyCategorySortChartClass> spendingList = new List<ForMonthlyCategorySortChartClass>();

            var spendingListData = from sld in db.ForMonthlyCategorySortChart where sld.SpendingYear == year select sld;

            foreach (ForMonthlyCategorySortChart spending in spendingListData)
            {
                if (spending.SpendingYear == year)
                {
                    ForMonthlyCategorySortChartClass OneRow = new ForMonthlyCategorySortChartClass();
                    OneRow.Category = spending.Category;
                    OneRow.Price = (int)spending.Price;
                    spendingList.Add(OneRow);
                }
            }

            typeList = "'" + string.Join("','", spendingList.Select(n => n.Category).ToList()) + "'";
            priceList = string.Join(",", spendingList.Select(n => n.Price).ToList());

            ViewBag.Category = typeList;
            ViewBag.Price = priceList;

            return PartialView();
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
            ViewBag.LoginID = new SelectList(db.Logins, "LoginID", "UserName", monthlySpendings.LoginID);
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
