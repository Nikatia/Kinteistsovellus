using Kiinteistosovellus.Models;
using System;
using System.Collections.Generic;
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
            List<MonthlySummary> summary = db.MonthlySummary.ToList();
            return summary;
        }

        // GET: MonthlySummary
        public ActionResult Index()
        {
            ViewBag.Summary = GetSummary();
            return View(db.SpendingMonths.ToList());
        }
    }
}