﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GalaxyBuildersSystem;
using GalaxyBuildersSystem.Models;
using Microsoft.AspNet.Identity;

namespace GalaxyBuildersSystem.Controllers
{
    public class TasksController : Controller
    {
        private GalaxyContext db = new GalaxyContext();

        // GET: Tasks
        public ActionResult Index()
        {
            Guid userId = Guid.Parse(User.Identity.GetUserId());
            var tasks = db.Employees.Find(userId).Tasks;
            return View(tasks.ToList());
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GTask gTask = db.Tasks.Find(id);
            if (gTask == null)
            {
                return HttpNotFound();
            }
            return View(gTask);
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "Name");
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarkAsDone(IEnumerable<GTask> tasks)
        {
            if (ModelState.IsValid)
            {
                foreach (GTask tsk in tasks)
                {
                    db.Entry(tsk).State = EntityState.Modified;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tasks);
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GTask gTask = db.Tasks.Find(id);
            if (gTask == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "Name", gTask.EmployeeId);
            return View(gTask);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,DateAssigned,IsDone,EmployeeId")] GTask gTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gTask).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "Name", gTask.EmployeeId);
            return View(gTask);
        }

        // GET: Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GTask gTask = db.Tasks.Find(id);
            if (gTask == null)
            {
                return HttpNotFound();
            }
            return View(gTask);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GTask gTask = db.Tasks.Find(id);
            db.Tasks.Remove(gTask);
            db.SaveChanges();
            return RedirectToAction("Index");
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