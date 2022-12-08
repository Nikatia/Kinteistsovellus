using Kiinteistosovellus.Models;
using Kiinteistosovellus.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Kiinteistosovellus.Controllers
{
    public class HomeController : BaseController
    {
        private KiinteistoDBEntities db = new KiinteistoDBEntities();

        public ActionResult IndexNotLogged()
        {
            return View();
        }

        public ActionResult Index()
        {
            ViewBag.Vuosi = new SelectList(db.MonthlyAndOtherSpendingsByMonth, "Vuosi", "Vuosi");
            ViewBag.VuosiLine = new SelectList(db.MonthlyAndOtherSpendingsByMonth, "Vuosi", "Vuosi");
            ViewBag.Years = GetYears();
            var thisYear = DateTime.Now.Year;
            decimal summa = 0;
            var monthOthSpendData = from sld in db.MonthlyAndOtherSpendingsByMonth
                                    where sld.Vuosi == thisYear
                                    select sld;
            var yearObject = monthOthSpendData.FirstOrDefault();
            summa = yearObject.Tammikuu + yearObject.Helmikuu + yearObject.Maaliskuu + yearObject.Huhtikuu + yearObject.Toukokuu + 
                    yearObject.Kesäkuu + yearObject.Heinäkuu + yearObject.Elokuu + yearObject.Syyskuu + yearObject.Lokakuu + yearObject.Marraskuu + yearObject.Joulukuu;
            ViewBag.ThisYearSumma = summa;

            return View();
        }

        public ActionResult PieChart(int? year)
        {
            string typeList;
            string priceList;
            List<ForCategorySortChartWithYearsClass> spendingList = new List<ForCategorySortChartWithYearsClass>();

            var spendingListData = from sld in db.ForCategorySortChartWithYears where sld.SpendingYear == year select sld;

            foreach (ForCategorySortChartWithYears spending in spendingListData)
            {
                if (spending.SpendingYear == year)
                {
                    ForCategorySortChartWithYearsClass OneRow = new ForCategorySortChartWithYearsClass();
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

        public ActionResult _LineChart(int? year)
        {
            int? thisYear = year;

            var monthOthSpendData = from sld in db.MonthlyAndOtherSpendingsByMonth
                                    where sld.Vuosi == thisYear
                                    select sld;

            var yearObject = monthOthSpendData.FirstOrDefault();

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

            string[] months = new string[12];
            months[0] = "Tammikuu";
            months[1] = "Helmikuu";
            months[2] = "Maaliskuu";
            months[3] = "Huthikuu";
            months[4] = "Toukokuu";
            months[5] = "Kesäkuu";
            months[6] = "Heinäkuu";
            months[7] = "Elokuu";
            months[8] = "Syyskuu";
            months[9] = "Lokakuu";
            months[10] = "Marraskuu";
            months[11] = "Joulukuu";

            ViewBag.Year = JsonConvert.SerializeObject(yearValues);
            ViewBag.Months = JsonConvert.SerializeObject(months);

            return PartialView("/Views/Home/_LineChart.cshtml");
        }

        public List<MonthlyAndOtherSpendingsByMonth> GetYears()
        {
            List<MonthlyAndOtherSpendingsByMonth> years = db.MonthlyAndOtherSpendingsByMonth.ToList();
            return years;
        }

    }
}