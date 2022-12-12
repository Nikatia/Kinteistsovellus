using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kiinteistosovellus.Models;

namespace Kiinteistosovellus.Controllers
{
    public class LoginsController : BaseController
    {
        private KiinteistoDBEntities db = new KiinteistoDBEntities();

        // GET: Logins
        public ActionResult Index()
        {
            if (Session["UserName"] != null)
            {
                return View(db.Logins.ToList());
            }
            else { return null; }
            }

        // GET: Logins/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UserName"] != null)
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
            else { return null; }
        }

        // GET: Logins/Create
        public ActionResult Create()
        {
            if (Session["UserName"] != null)
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
        public ActionResult Create([Bind(Include = "LoginID,UserName,UserPassword")] Logins logins)
        {
            if (Session["UserName"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Logins.Add(logins);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(logins);
            }
            else { return null; }
        }

        public ActionResult _CreateModalLogins()
        {
            if (Session["UserName"] != null)
            {
                return PartialView();
            }
            else { return null; }
            }
        // GET: Logins/Edit/5
        public ActionResult Edit(int? id)
        {

            if (Session["UserName"] != null)
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

            if (Session["UserName"] != null)
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
            else
            {
                return null;
            }
            }

        // POST: Logins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LoginID,UserName,UserPassword")] Logins logins)
        {

            if (Session["UserName"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(logins).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(logins);
            }
            else
            {
                return null;
            }
            }

        // GET: Logins/Delete/5
        public ActionResult Delete(int? id)
        {

            if (Session["UserName"] != null)
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

        // POST: Logins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            if (Session["UserName"] != null)
            {
                Logins logins = db.Logins.Find(id);
                db.Logins.Remove(logins);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else { return null; }
            }
        public ActionResult _DeleteModalLogins(int? id)
        {

            if (Session["UserName"] != null)
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
            else { return null; }
            }
        [HttpPost, ActionName("_DeleteModalLogins")]
        [ValidateAntiForgeryToken]
        public ActionResult _DeleteModalLoginsConfirmed(int id)
        {

            if (Session["UserName"] != null)
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
