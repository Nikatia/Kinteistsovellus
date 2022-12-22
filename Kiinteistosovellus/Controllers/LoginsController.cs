using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Kiinteistosovellus.Models;

namespace Kiinteistosovellus.Controllers
{
    public class LoginsController : BaseController
    {
        private KiinteistoDBEntities db = new KiinteistoDBEntities();

        // GET: Logins
        public ActionResult Index()
        {
            if (Session["Role"].ToString() == "Admin")
            {
                return View(db.Logins.ToList());
            }
            else { return RedirectToAction("Index", "Home"); }
        }

        // GET: Logins/Create
        public ActionResult Create()
        {
            if (Session["Role"].ToString() == "Admin")
            {
                return View();
            }
            else { return null; }
        }

        // POST: Logins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LoginID,UserName,UserPassword,RoleID")] Logins logins)
        {
            if (Session["Role"].ToString() == "Admin")
            {
                var userExists = from l in db.Logins
                                 where logins.UserName.ToString() == l.UserName.ToString()
                                 select l;
                int count = userExists.Count();
                ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "Role", logins.RoleID);
                if (ModelState.IsValid)
                {
                    if (count > 0)
                    {
                        ViewBag.Error = "Käyttäjä on jo olemassa!";
                        return PartialView("/Views/Logins/_CreateModalLogins.cshtml", logins);
                    }
                    else
                    {
                        db.Logins.Add(logins);
                        db.SaveChanges();
                        return null;
                    }
                    
                }
                ViewBag.Error = "";
                return PartialView("/Views/Logins/_CreateModalLogins.cshtml",logins);
            }
            else { return null; }
        }

        public ActionResult _CreateModalLogins()
        {
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "Role");
            return PartialView();
        }
        // GET: Logins/Edit/5
        public ActionResult Edit(int? id)
        {

            if (Session["Role"].ToString() == "Admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Logins logins = db.Logins.Find(id);
                if (logins == null)
                {
                    return HttpNotFound();
                }
                return View(logins);
            }
            else
            {
                return null;
            }
        }

        public ActionResult _EditModalLogins(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Logins logins = db.Logins.Find(id);
            if (logins == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "Role", logins.RoleID);
            return PartialView(logins);
        }

        // POST: Logins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LoginID,UserName,UserPassword,RoleID")] Logins logins)
        {

            if (Session["Role"].ToString() == "Admin")
            {
                if (ModelState.IsValid)
                {
                    db.Entry(logins).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "Role", logins.RoleID);
                return View(logins);
            }
            else
            {
                return null;
            }
        }

        public ActionResult _DeleteModalLogins(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Logins logins = db.Logins.Find(id);
            if (logins == null)
            {
                return HttpNotFound();
            }
            return PartialView(logins);
        }

        [HttpPost, ActionName("_DeleteModalLogins")]
        [ValidateAntiForgeryToken]
        public ActionResult _DeleteModalLoginsConfirmed(int id)
        {

            if (Session["Role"].ToString() == "Admin")
            {
                Logins logins = db.Logins.Find(id);
                db.Logins.Remove(logins);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return null;
            }   
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
