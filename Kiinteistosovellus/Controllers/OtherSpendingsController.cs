using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kiinteistosovellus.Models;
using Kiinteistosovellus.ViewModels;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace Kiinteistosovellus.Controllers
{
    public class OtherSpendingsController : BaseController
    {
        private KiinteistoDBEntities db = new KiinteistoDBEntities();

        public List<OtherSpendingTypes> GetTypes()
        {
            if (Session["UserName"] != null)
            {
                List<OtherSpendingTypes> types = db.OtherSpendingTypes.ToList();
                return types;
            }
            else { return null; }
        }

        [HttpGet]
        public JsonResult FetchTypes()
        {
            if (Session["UserName"] != null)
            {
                var data = GetTypes().Select(c => new { Value = c.OtherSpendingTypeId, Text = c.TypeName });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        // GET: OtherSpendings
        public ActionResult Index()
        {
            if (Session["UserName"] != null)
            {
                var otherSpendings = db.OtherSpendings.Include(o => o.Contractors).Include(o => o.OtherSpendingTypes);
                var distinctYearList = db.OtherTypeSpendingsByMonth.DistinctBy(x => x.Vuosi).ToList();
                ViewBag.Vuosi = new SelectList(distinctYearList, "Vuosi", "Vuosi");
                return View(otherSpendings.ToList());
            }
            else { return RedirectToAction("Index", "Home"); }
        }

        public ActionResult Chart(int year)
        {
            if (Session["UserName"] != null)
            {
                decimal tammikuu = 0, helmikuu = 0, maaliskuu = 0, huhtikuu = 0, toukokuu = 0, kesäkuu = 0, heinäkuu = 0, elokuu = 0, syyskuu = 0, lokakuu = 0, marraskuu = 0, joulukuu = 0;
                var othSpendData = from sld in db.OtherTypeSpendingsByMonth
                                   where sld.Vuosi == year
                                   select sld;

                foreach (var m in othSpendData)
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
            else { return null; }
        }

        public ActionResult BarChart(int year)
        {
            if (Session["UserName"] != null)
            {
                string typeList;
                string priceList;
                List<ForOthersCategorySortChartClass> spendingList = new List<ForOthersCategorySortChartClass>();

                var spendingListData = from sld in db.ForOthersCategorySortChart where sld.SpendingYear == year select sld;

                foreach (ForOthersCategorySortChart spending in spendingListData)
                {
                    if (spending.SpendingYear == year)
                    {
                        ForOthersCategorySortChartClass OneRow = new ForOthersCategorySortChartClass();
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
            else { return null; }
        }


        // ----------------------------------------------- CREATE PART -----------------------------------------------

        public ActionResult _ModalCreate()
        {
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name");
            ViewBag.OtherSpendingTypeID = new SelectList(db.OtherSpendingTypes, "OtherSpendingTypeId", "TypeName");

            return PartialView("/Views/OtherSpendings/_ModalCreate.cshtml");
        }

        // POST: OtherSpendings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult _ModalCreate([Bind(Include = "OtherSpendingsID,DateBegin,DateEnd,Description,OtherSpendingTypeID,ContractorID,Price")] OtherSpendings otherSpendings)
        {
            if (Session["UserName"] != null)
            {
                if (ModelState.IsValid)
                {
                    Console.WriteLine("IsValid");
                    db.OtherSpendings.Add(otherSpendings);
                    db.SaveChanges();
                    return null;
                }

                ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", otherSpendings.ContractorID);
                ViewBag.OtherSpendingTypeID = new SelectList(db.OtherSpendingTypes, "OtherSpendingTypeId", "TypeName", otherSpendings.OtherSpendingTypeID);
                return PartialView("/Views/OtherSpendings/_ModalCreate.cshtml", otherSpendings);
            }
            else { return null; }
        }

        public PartialViewResult _ModalCreateOthSpendingType()//vain sitä varten, että modaalin avautuessa ajax-pyynnöllä luodaan partial view
        {
            if (Session["UserName"] != null)
            {
                ViewBag.SuccessMsg = "";
                return PartialView("/Views/OtherSpendings/_PartialOthSpendType.cshtml");
            }
            else { return null; }
        }

        // POST: OtherSpendings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult _ModalCreateOthSpendingType([Bind(Include = "OtherSpendingTypeId,TypeName")] OtherSpendingTypes otherSpendingType)
        {
            if (Session["UserName"] != null)
            {
                ViewBag.SuccessMsg = "";

                if (ModelState.IsValid)//On aina true jostain syystä PITÄÄ KORJATA!!!
                {
                    Console.WriteLine("IsValid");
                    db.OtherSpendingTypes.Add(otherSpendingType);
                    db.SaveChanges();
                    ViewBag.SuccessMsg = "successfully added";
                    return PartialView("/Views/OtherSpendings/_PartialOthSpendType.cshtml"); //Tässä pitää palauttaa näkymä!!!
                }

                return PartialView("/Views/OtherSpendings/_PartialOthSpendType.cshtml", otherSpendingType);
            }
            else
            {
                return null;
            }
        }

        // ----------------------------------------------- EDIT PART -----------------------------------------------

        public ActionResult _ModalEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OtherSpendings otherSpendings = db.OtherSpendings.Find(id);
            if (otherSpendings == null)
            {
                return HttpNotFound();
            }

            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", otherSpendings.ContractorID);
            ViewBag.OtherSpendingTypeID = new SelectList(db.OtherSpendingTypes, "OtherSpendingTypeId", "TypeName", otherSpendings.OtherSpendingTypeID);
            ViewBag.OtherSpendingID = otherSpendings.OtherSpendingsID;

            return PartialView("_ModalEdit", otherSpendings);
        }

        // POST: OtherSpendings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult _ModalEdit([Bind(Include = "OtherSpendingsID,DateBegin,DateEnd,Description,OtherSpendingTypeID,ContractorID,Price")] OtherSpendings otherSpendings)
        {
            if (Session["UserName"] != null)
            {
                if (ModelState.IsValid)
                {

                    db.Entry(otherSpendings).State = EntityState.Modified;
                    db.SaveChanges();
                    return null;
                }
                ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", otherSpendings.ContractorID);
                ViewBag.OtherSpendingTypeID = new SelectList(db.OtherSpendingTypes, "OtherSpendingTypeId", "TypeName", otherSpendings.OtherSpendingTypeID);
                ViewBag.OtherSpendingID = otherSpendings.OtherSpendingsID;

                return PartialView("/Views/OtherSpendings/_ModalEdit.cshtml", otherSpendings);
            }
            else { return null; }
        }



        // ----------------------------------------------- DELETE PART -----------------------------------------------
        //GET: OtherSpendings/Delete/5
        public ActionResult _ModalDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OtherSpendings otherSpendings = db.OtherSpendings.Find(id);
            if (otherSpendings == null)
            {
                return HttpNotFound();
            }

            return PartialView("_ModalDelete", otherSpendings);
        }

        // POST: OtherSpendings/Delete/5
        [HttpPost, ActionName("_ModalDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult _ModalDeleteConfirmed(int id)
        {
            if (Session["UserName"] != null)
            {
                OtherSpendings otherSpendings = db.OtherSpendings.Find(id);
                db.OtherSpendings.Remove(otherSpendings);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else { return null; }
        }


        [HttpPost]
        public JsonResult GetList()
        {
            if (Session["UserName"] != null)
            {
                var itemlist = db.OtherSpendingTypes.ToList();
                var itemList = itemlist.Select(item => new SelectListItem { Text = item.TypeName, Value = Convert.ToString(item.OtherSpendingTypeId) }).ToList();
                return Json(new SelectList(itemList, "Value", "Text"));
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
