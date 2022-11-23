using Kiinteistosovellus.Models;
using Kiinteistosovellus.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace Kiinteistosovellus.Controllers
{
    public class HomeController : BaseController
    {
        private KiinteistoDBEntities db = new KiinteistoDBEntities();

        public ActionResult Index()
        {
            //Pie chartti
            string typeList;
            string priceList;
            List<ForCategorySortChartClass> spendingList = new List<ForCategorySortChartClass>();

            var spendingListData = from sld in db.ForCategorySortChart select sld;

            foreach (ForCategorySortChart spending in spendingListData)
            {
                ForCategorySortChartClass OneRow = new ForCategorySortChartClass();
                OneRow.Category = spending.Category;
                OneRow.Price = (int)spending.Price;
                spendingList.Add(OneRow);
            }

            typeList = "'" + string.Join("','", spendingList.Select(n => n.Category).ToList()) + "'";
            priceList = string.Join(",", spendingList.Select(n => n.Price).ToList());

            ViewBag.Category = typeList;
            ViewBag.Price = priceList;


            //line chart


            //List<MonthlyAndOtherSpendingsByMonthClass> monthOthSpendList = new List<MonthlyAndOtherSpendingsByMonthClass>();
            int? thisYear = DateTime.Now.Year;
            var monthOthSpendData = from sld in db.MonthlyAndOtherSpendingsByMonth
                                        where sld.Vuosi == thisYear
                                        select sld;

            var yearObject = monthOthSpendData.FirstOrDefault();

            //foreach (MonthlyAndOtherSpendingsByMonth year in monthOthSpendData)
            //{
            //    MonthlyAndOtherSpendingsByMonthClass OneRow = new MonthlyAndOtherSpendingsByMonthClass();
            //    OneRow.Tammikuu = year.Tammikuu;
            //    OneRow.Helmikuu = year.Helmikuu;
            //    OneRow.Maaliskuu = year.Maaliskuu;
            //    OneRow.Huhtikuu = year.Huhtikuu;
            //    OneRow.Toukokuu = year.Toukokuu;
            //    OneRow.Kesäkuu = year.Kesäkuu;
            //    OneRow.Heinäkuu = year.Heinäkuu;
            //    OneRow.Elokuu = year.Elokuu;
            //    OneRow.Syyskuu = year.Syyskuu;
            //    OneRow.Lokakuu = year.Lokakuu;
            //    OneRow.Marraskuu = year.Marraskuu;
            //    OneRow.Joulukuu = year.Joulukuu;
            //    monthOthSpendList.Add(OneRow);
            //}

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

            ViewBag.Year = JsonConvert.SerializeObject(yearValues);


            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}