using Kiinteistosovellus.Models;
using Kiinteistosovellus.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kiinteistosovellus.Controllers
{
    public class HomeController : Controller
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