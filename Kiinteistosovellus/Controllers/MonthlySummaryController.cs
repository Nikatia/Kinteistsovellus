using Kiinteistosovellus.Models;
using Kiinteistosovellus.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kiinteistosovellus.Controllers
{
    public class MonthlySummaryController : Controller
    {
        private KiinteistoDBEntities db = new KiinteistoDBEntities();

        public List<MonthlySummary> GetSummary()
        {
            if (Session["UserName"] != null)
            {
                List<MonthlySummary> summary = db.MonthlySummary.ToList();
                return summary;
            }
            else { return null; }
            
        }

        // GET: MonthlySummary
        public ActionResult Index()
        {
            if (Session["UserName"] != null)
            {
                ViewBag.Summary = GetSummary();
                return View(db.SpendingMonths.ToList());
            }
            else { return RedirectToAction("Index", "Home"); }
            
        }

        public ActionResult _PieChart(int? id)
        {
            if (Session["UserName"] != null)
            {
                string typeList;
                string priceList;
                List<ForMonthSummary> spendingList = new List<ForMonthSummary>();
                var monthList = from sm in db.SpendingMonths where sm.Rivi == id select sm;
                var categoryList = from ms in db.MonthlySummary select ms;

                foreach (SpendingMonths month in monthList)
                {
                    foreach (MonthlySummary category in categoryList)
                    {
                        if (month.MonthOfSpending == category.MonthOfSpending && month.YearOfSpending == category.YearOfSpending)
                        {
                            ForMonthSummary OneRow = new ForMonthSummary();
                            OneRow.TypeName = category.TypeName;
                            OneRow.Kokonaishinta = ((int)category.Kokonaishinta * 100 / (int)month.Summa);
                            spendingList.Add(OneRow);
                        }
                    }
                }
                typeList = "'" + string.Join("','", spendingList.Select(n => n.TypeName).ToList()) + "'";
                priceList = string.Join(",", spendingList.Select(n => n.Kokonaishinta).ToList());

                ViewBag.Category = typeList;
                ViewBag.Price = priceList;

                return PartialView();
            }
            else { return null; }
            
        }
    }
}