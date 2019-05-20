using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using GalaxyBuildersSystem.Models;

namespace GalaxyBuildersSystem.Controllers
{
    public class EmployeesController : Controller
    {
        private GalaxyContext db = new GalaxyContext();

        // GET: Employees/
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                Guid userId = Guid.Parse(User.Identity.GetUserId());
                var emp = db.Employees.Find(userId);
                var employees = db.Employees.Include(e => e.Team).Where(e => e.TeamId == emp.TeamId && e.IsManager == false);
                var manager = db.Employees.Where(e => e.IsManager == true).First();

                ViewBag.Team = emp.Team.Description;
                ViewBag.Manager = string.Format("{0} {1}", manager.Name, manager.Lastname);
                ViewBag.Role = emp.IsManager ? "Manager" : "Employee";
                return View(employees.ToList());
            }

            return RedirectToAction("Login", "Account");
        }

        // GET: Teams/
        public ActionResult Teams()
        {
            if (User.Identity.IsAuthenticated)
            {
                Guid userId = Guid.Parse(User.Identity.GetUserId());
                var emp = db.Employees.Find(userId);

                if (emp.IsManager)
                {
                    ViewBag.Role = "Manager";
                    return View(db.Teams.ToList());
                }
            }

            return View("Unauthorised");
        }

            // GET: Employees/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.TeamId = new SelectList(db.Teams, "Id", "Description");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Lastname,Productivity,TeamId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.Id = Guid.NewGuid();
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TeamId = new SelectList(db.Teams, "Id", "Description", employee.TeamId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.TeamId = new SelectList(db.Teams, "Id", "Description", employee.TeamId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Lastname,Productivity,TeamId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TeamId = new SelectList(db.Teams, "Id", "Description", employee.TeamId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Employees/AssignTask/{id}
        public ActionResult AssignTask(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Employee employee = db.Employees.Find(id);

            if (employee == null)
            {
                return HttpNotFound();
            }

            GTask task = new GTask
            {
                EmployeeId = employee.Id
            };

            return View(task);
        }

        // POST: Employees/AssignTask/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignTask([Bind(Include = "Description, EmployeeId")] GTask task)
        {
            if (ModelState.IsValid)
            {
                //employee.Id = Guid.NewGuid();
                task.DateAssigned = DateTime.Now;
                task.IsDone = false;
                Employee employee = db.Employees.Find(task.EmployeeId);

                db.Tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("Edit", employee);
            }

            return View(task);
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
