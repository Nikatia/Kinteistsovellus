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

        public List<MonthlySpendingTypes> GetTypes()
        {
            if (Session["UserName"] != null)
            {
                List<MonthlySpendingTypes> types = db.MonthlySpendingTypes.ToList();
                return types;
            }
            else { return null; }
        }

        [HttpGet]
        public JsonResult FetchTypes()
        {
            if (Session["UserName"] != null)
            {
                var data = GetTypes().Select(c => new { Value = c.SpendingTypeID, Text = c.TypeName });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        // GET: MonthlySpendings
        public ActionResult Index()
        {
            if (Session["UserName"] != null)
            {
                ViewBag.Error = 0;
                var monthlySpendings = db.MonthlySpendings.Include(m => m.Contractors).Include(m => m.MonthlySpendingTypes);
                var distinctYearList = db.MonthlyTypeSpendingsByMonth.DistinctBy(x => x.Vuosi).ToList();
                ViewBag.Vuosi = new SelectList(distinctYearList, "Vuosi", "Vuosi");

                var kulutyypit = db.MonthlySpendings.ToList();
                List<string> monthTypeArray = new List<string>();
                for (int i = 0; i < kulutyypit.Count; i++)
                {
                    if (!monthTypeArray.Contains(kulutyypit[i].MonthlySpendingTypes.TypeName.ToString()))
                    {
                        monthTypeArray.Add(kulutyypit[i].MonthlySpendingTypes.TypeName.ToString());
                    }
                }
                ViewBag.Kulutyypit = monthTypeArray;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home"); ;
            }
        }

        public PartialViewResult _IndexTable(string alkuPvm, string loppuPvm, string kulutyypit, string columnNumber, string ascOrDesc, string showOrHide)
        {
            DateTime dtAlkuPvmTest;
            DateTime dtLoppuPvmTest;

            DateTime? dtAlkuPvm = null;
            DateTime? dtLoppuPvm = null;

            if (DateTime.TryParse(alkuPvm, out dtAlkuPvmTest))
            {
                dtAlkuPvmTest = DateTime.Parse(alkuPvm);
                dtAlkuPvm = dtAlkuPvmTest;
            }

            if (DateTime.TryParse(loppuPvm, out dtLoppuPvmTest))
            {
                dtLoppuPvmTest = DateTime.Parse(loppuPvm);
                dtLoppuPvm = dtLoppuPvmTest;
            }

            ViewBag.Disabled = showOrHide;
            var tempIQueryable = from ms in db.MonthlySpendings
                                 select ms;
            IQueryable<MonthlySpendings> monthlySpendings = null;
            //var monthlySpendings = from ms in db.MonthlySpendings
            //                       where (ms.MonthlySpendingID == 1)
            //                       select ms;
            string strKulutyypit = kulutyypit;


            if (strKulutyypit != "")
            {
                var kulutyyppiArray = strKulutyypit.Split('#');
                HashSet<string> strset = new HashSet<string>(kulutyyppiArray);
                monthlySpendings = tempIQueryable.Where(ms => strset.Contains(ms.MonthlySpendingTypes.TypeName));
            }
            else
            {
                monthlySpendings = tempIQueryable;
            }

            string intColumnNumber = columnNumber;

            //var monthlySpendings = from ms in db.MonthlySpendings
            //                       select ms;

            if (dtAlkuPvm != null && dtLoppuPvm != null)
            {
                monthlySpendings = monthlySpendings.Where(ms => ((ms.DateBegin >= dtAlkuPvm && ms.DateEnd == null) || (ms.DateEnd >= dtAlkuPvm && ms.DateEnd != null)) && ms.DateBegin <= dtLoppuPvm);
            }
            else if (dtAlkuPvm != null && dtLoppuPvm == null)
            {
                monthlySpendings = monthlySpendings.Where(ms => (ms.DateBegin >= dtAlkuPvm && ms.DateEnd == null) || (ms.DateEnd >= dtAlkuPvm && ms.DateEnd != null));
            }
            else if (dtAlkuPvm == null && dtLoppuPvm != null)
            {
                monthlySpendings = monthlySpendings.Where(ms => ms.DateBegin <= dtLoppuPvm);
            }


            switch (columnNumber)
            {
                case "0":
                    if (ascOrDesc == "desc")
                    {
                        monthlySpendings = monthlySpendings.OrderByDescending(ms => ms.DateBegin);
                    }
                    else
                    {
                        monthlySpendings = monthlySpendings.OrderBy(ms => ms.DateBegin);
                    }
                    break;
                case "1":
                    if (ascOrDesc == "desc")
                    {
                        monthlySpendings = monthlySpendings.OrderByDescending(ms => ms.DateEnd ?? ms.DateBegin);
                    }
                    else
                    {
                        monthlySpendings = monthlySpendings.OrderBy(ms => ms.DateEnd ?? ms.DateBegin);
                    }
                    break;
                case "2":
                    if (ascOrDesc == "desc")
                    {
                        monthlySpendings = monthlySpendings.OrderByDescending(ms => ms.MonthlySpendingTypes.TypeName);
                    }
                    else
                    {
                        monthlySpendings = monthlySpendings.OrderBy(ms => ms.MonthlySpendingTypes.TypeName);
                    }
                    break;
                case "3":
                    if (ascOrDesc == "desc")
                    {
                        monthlySpendings = monthlySpendings.OrderByDescending(ms => ms.AmountOfUnits);
                    }
                    else
                    {
                        monthlySpendings = monthlySpendings.OrderBy(ms => ms.AmountOfUnits);
                    }
                    break;
                case "4":
                    if (ascOrDesc == "desc")
                    {
                        monthlySpendings = monthlySpendings.OrderByDescending(ms => ms.MonthlySpendingTypes.Unit);
                    }
                    else
                    {
                        monthlySpendings = monthlySpendings.OrderBy(ms => ms.MonthlySpendingTypes.Unit);
                    }
                    break;
                case "5":
                    if (ascOrDesc == "desc")
                    {
                        monthlySpendings = monthlySpendings.OrderByDescending(ms => ms.PricePerUnit);
                    }
                    else
                    {
                        monthlySpendings = monthlySpendings.OrderBy(ms => ms.PricePerUnit);
                    }
                    break;
                case "6":
                    if (ascOrDesc == "desc")
                    {
                        monthlySpendings = monthlySpendings.OrderByDescending(ms => ms.TransferPayment);
                    }
                    else
                    {
                        monthlySpendings = monthlySpendings.OrderBy(ms => ms.TransferPayment);
                    }
                    break;
                case "7":
                    if (ascOrDesc == "desc")
                    {
                        monthlySpendings = monthlySpendings.OrderByDescending(ms => ms.FullPrice);
                    }
                    else
                    {
                        monthlySpendings = monthlySpendings.OrderBy(ms => ms.FullPrice);
                    }
                    break;
                case "8":
                    if (ascOrDesc == "desc")
                    {
                        monthlySpendings = monthlySpendings.OrderByDescending(ms => ms.Contractors.Name);
                    }
                    else
                    {
                        monthlySpendings = monthlySpendings.OrderBy(ms => ms.Contractors.Name);
                    }
                    break;
                case "9":
                    if (ascOrDesc == "desc")
                    {
                        monthlySpendings = monthlySpendings.OrderByDescending(ms => ms.ImageUrl);
                    }
                    else
                    {
                        monthlySpendings = monthlySpendings.OrderBy(ms => ms.ImageUrl);
                    }
                    break;
                default:
                    monthlySpendings = monthlySpendings.OrderByDescending(ms => ms.DateBegin);
                    break;
            }
            return PartialView("/Views/MonthlySpendings/_IndexTable.cshtml", monthlySpendings);
        }

        public ActionResult Chart(int year)
        {
            if (Session["UserName"] != null)
            {
                decimal tammikuu = 0, helmikuu = 0, maaliskuu = 0, huhtikuu = 0, toukokuu = 0, kesäkuu = 0, heinäkuu = 0, elokuu = 0, syyskuu = 0, lokakuu = 0, marraskuu = 0, joulukuu = 0;
                var monthSpendData = from sld in db.MonthlyTypeSpendingsByMonth
                                     where sld.Vuosi == year
                                     select sld;

                foreach (var m in monthSpendData)
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
                ViewBag.ThisYear = year;
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
            else { return null; }
        }

        public ActionResult _CreateModal()
        {
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name");
            ViewBag.SpendingTypeID = new SelectList(db.MonthlySpendingTypes, "SpendingTypeID", "TypeName");
            return PartialView("/Views/MonthlySpendings/_CreateModal.cshtml");
        }
        // POST: MonthlySpendings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _CreateModal([Bind(Include = "MonthlySpendingID,DateBegin,DateEnd,SpendingTypeID,AmountOfUnits,PricePerUnit,TransferPayment,FullPrice,ContractorID,ImageUrl")] MonthlySpendings monthlySpendings, HttpPostedFileBase kuvaim)
        {

            if (Session["UserName"] != null)
            {


                ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", monthlySpendings.ContractorID);

                ViewBag.SpendingTypeID = new SelectList(db.MonthlySpendingTypes, "SpendingTypeID", "TypeName", monthlySpendings.SpendingTypeID);
                if (ModelState.IsValid)
                {
                    if (monthlySpendings.ImageUrl != null)
                    {
                        ImageService imageService = new ImageService();

                        await imageService.UploadImageAsync(kuvaim);
                        ViewBag.Error = 0;
                        db.MonthlySpendings.Add(monthlySpendings);
                        db.SaveChanges();
                        return null;
                    }

                    ViewBag.Error = 0;
                    db.MonthlySpendings.Add(monthlySpendings);
                    db.SaveChanges();
                    return null;


                }
                ViewBag.Error = 1;
                return PartialView("/Views/MonthlySpendings/_CreateModal.cshtml", monthlySpendings);
            }
            else { return null; }
        }

        public PartialViewResult _ModalCreateMonthSpendingType()//vain sitä varten, että modaalin avautuessa ajax-pyynnöllä luodaan partial view
        {
            if (Session["UserName"] != null)
            {
                ViewBag.SuccessMsg = "";
                return PartialView("/Views/MonthlySpendings/_PartialViewMonthSpendType.cshtml");
            }
            else { return null; }
        }

        // POST: OtherSpendings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult _ModalCreateMonthSpendingType([Bind(Include = "SpendingTypeID,TypeName,Unit")] MonthlySpendingTypes monthSpendingType)
        {
            if (Session["UserName"] != null)
            {
                ViewBag.SuccessMsg = "";

                var typeExists = from l in db.MonthlySpendingTypes
                                 where monthSpendingType.TypeName.ToString() == l.TypeName.ToString()
                                 select l;
                int count = typeExists.Count();

                if (ModelState.IsValid)
                {
                    if (count > 0)
                    {
                        ViewBag.Error = "Kulutustyyppi on jo olemassa!";
                        return PartialView("/Views/MonthlySpendings/_PartialViewMonthSpendType.cshtml", monthSpendingType);
                    }
                    else
                    {
                        db.MonthlySpendingTypes.Add(monthSpendingType);
                        db.SaveChanges();
                        ViewBag.SuccessMsg = "successfully added";
                        return PartialView("/Views/MonthlySpendings/_PartialViewMonthSpendType.cshtml");
                    }
                }
                return PartialView("/Views/MonthlySpendings/_PartialViewMonthSpendType.cshtml", monthSpendingType);
            }
            else
            {
                return null;
            }
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
            ViewBag.SpendingTypeID = new SelectList(db.MonthlySpendingTypes, "SpendingTypeID", "TypeName", monthlySpendings.SpendingTypeID);
            return PartialView("_EditModal", monthlySpendings);
        }

        // POST: MonthlySpendings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _EditModal([Bind(Include = "MonthlySpendingID,DateBegin,DateEnd,SpendingTypeID,AmountOfUnits,PricePerUnit,TransferPayment,FullPrice,ContractorID,ImageUrl")] MonthlySpendings monthlySpendings, HttpPostedFileBase kuvaim)
        {
            if (Session["UserName"] != null)
            {
                var original = db.MonthlySpendings.AsNoTracking().FirstOrDefault(m => m.MonthlySpendingID == monthlySpendings.MonthlySpendingID); //Alkuperäiset arvot
                var old = original.ImageUrl;
                var anew = monthlySpendings.ImageUrl;//Uudet arvot

                bool equal()
                {
                    if (old != null && anew != null)
                    {
                        var modified = db.MonthlySpendings.AsNoTracking().FirstOrDefault(m => m.MonthlySpendingID == monthlySpendings.MonthlySpendingID);
                        var result = modified.ImageUrl.Equals(monthlySpendings.ImageUrl);

                        return result;
                    }
                    return false;
                }

                if (ModelState.IsValid)
                {
                    if (equal() || (old == null && anew == null) || anew == null)
                    {

                        db.Entry(monthlySpendings).State = EntityState.Modified;
                        db.SaveChanges();
                        db.Dispose();
                        return null;
                    }
                    ImageService imageService = new ImageService();
                    await imageService.UploadImageAsync(kuvaim);
                    db.Entry(monthlySpendings).State = EntityState.Modified;
                    db.SaveChanges();
                    db.Dispose();
                    return null;

                }
                ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Name", monthlySpendings.ContractorID);
                ViewBag.SpendingTypeID = new SelectList(db.MonthlySpendingTypes, "SpendingTypeID", "TypeName", monthlySpendings.SpendingTypeID);
                return PartialView("_EditModal", monthlySpendings);
            }
            else
            {
                return null;
            }
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
            return PartialView("_DeleteModal", monthlySpendings);
        }

        // POST: MonthlySpendings/Delete/5
        [HttpPost, ActionName("_DeleteModal")]
        [ValidateAntiForgeryToken]
        public ActionResult _DeleteModalConfirmed(int id)
        {
            if (Session["UserName"] != null)
            {
                MonthlySpendings monthlySpendings = db.MonthlySpendings.Find(id);
                db.MonthlySpendings.Remove(monthlySpendings);
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
                var itemlist = db.MonthlySpendingTypes.ToList();
                var itemList = itemlist.Select(item => new SelectListItem { Text = item.TypeName, Value = Convert.ToString(item.SpendingTypeID) }).ToList();
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
