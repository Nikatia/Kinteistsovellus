using Kiinteistosovellus.Models;
using Kiinteistosovellus.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Kiinteistosovellus.Controllers
{
    public class HomeController : Controller
    {
        private KiinteistoDBEntities db = new KiinteistoDBEntities();

        public ActionResult Index()
        {
            ViewBag.Vuosi = new SelectList(db.MonthlyAndOtherSpendingsByMonth, "Vuosi", "Vuosi");
            ViewBag.VuosiLine = new SelectList(db.MonthlyAndOtherSpendingsByMonth, "Vuosi", "Vuosi");
            ViewBag.Years = GetYears();

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

        public List<MonthlyAndOtherSpendingsByMonth> GetYears()
        {
            List<MonthlyAndOtherSpendingsByMonth> years = db.MonthlyAndOtherSpendingsByMonth.ToList();
            return years;
        }

    }
}