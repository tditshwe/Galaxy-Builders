﻿using System;
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
                var manager = db.Employees.Where(e => e.IsManager == true && e.TeamId == emp.TeamId).First();

                ViewBag.Team = emp.Team.Description;
                ViewBag.Productivity = emp.Team.Productivity;
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

                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return RedirectToAction("Login", "Account");
        }

        // GET: Employees/Ranking/
        public ActionResult Ranking()
        {
            if (User.Identity.IsAuthenticated)
            {
                var emp = db.Employees.Find(Guid.Parse(User.Identity.GetUserId()));

                if (emp.IsManager)
                {
                    List<TeamRanking> ranking = new List<TeamRanking>();
                    ViewBag.Role = "Manager";

                    foreach (Team tm in db.Teams)
                    {
                        if (tm.Productivity > 0)
                        {
                            TeamRanking tr = new TeamRanking
                            {
                                Description = tm.Description,
                                Productivity = tm.Productivity,
                                BestEmployee = BestEmployee(tm)
                            };

                            ranking.Add(tr);
                        }
                    }

                    ranking = ranking.OrderByDescending(r => r.Productivity).ToList();
                    Rank(ranking);

                    return View(ranking);
                }

                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return RedirectToAction("Login", "Account");
        }

        // GET: Employees/Details/5
        public ActionResult Details(Guid? id)
        {
            if (User.Identity.IsAuthenticated)
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

            return RedirectToAction("Login", "Account");
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
            if (User.Identity.IsAuthenticated)
            {
                Guid userId = Guid.Parse(User.Identity.GetUserId());
                var emp = db.Employees.Find(userId);

                if (emp.IsManager)
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

                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return RedirectToAction("Login", "Account");
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
            if (User.Identity.IsAuthenticated)
            {
                Guid userId = Guid.Parse(User.Identity.GetUserId());
                var emp = db.Employees.Find(userId);

                if (emp.IsManager)
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

                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return RedirectToAction("Login", "Account");
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            if (User.Identity.IsAuthenticated)
            {
                Guid userId = Guid.Parse(User.Identity.GetUserId());
                var emp = db.Employees.Find(userId);

                if (emp.IsManager)
                {
                    Employee employee = db.Employees.Find(id);

                    employee.Tasks.ToList().ForEach(e => db.Tasks.Remove(e));
                    db.Employees.Remove(employee);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return RedirectToAction("Login", "Account");
        }

        // GET: Employees/AssignTask/{id}
        public ActionResult AssignTask(Guid? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                Guid userId = Guid.Parse(User.Identity.GetUserId());
                var emp = db.Employees.Find(userId);

                if (emp.IsManager)
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

                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return RedirectToAction("Login", "Account");
        }

        // POST: Employees/AssignTask/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignTask([Bind(Include = "Description, EmployeeId")] GTask task)
        {
            if (User.Identity.IsAuthenticated)
            {
                Guid userId = Guid.Parse(User.Identity.GetUserId());
                var emp = db.Employees.Find(userId);

                if (emp.IsManager)
                {
                    if (ModelState.IsValid)
                    {
                        task.DateAssigned = DateTime.Now;
                        task.IsDone = false;
                        
                        db.Tasks.Add(task);
                        db.SaveChanges();

                        Employee employee = db.Employees.Find(task.EmployeeId);
                        Productivity.TeamProductivity(employee.Team);
                        Productivity.CalculateProductivity(employee);
                        Productivity.TeamProductivity(employee.Team);
                        db.Entry(employee).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Edit", employee);
                    }

                    return View(task);
                }

                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return RedirectToAction("Login", "Account");
        }

        // GET: Tasks/RemoveTask/{id}
        public ActionResult RemoveTask(int? id)
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
            db.Tasks.Remove(gTask);
            db.SaveChanges();

            Employee employee = db.Employees.Find(gTask.EmployeeId);

            Productivity.CalculateProductivity(employee);
            Productivity.TeamProductivity(employee.Team);
            db.Entry(employee).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Edit", db.Employees.Find(gTask.EmployeeId)); ;
        }

        private Employee BestEmployee(Team team)
        {
            decimal max = 0;
            Employee emp = null;

            foreach (Employee e in team.Employees)
            {
                if (e.Productivity > max)
                {
                    max = e.Productivity;
                    emp = e;
                }
            }

            return emp;
        }

        private void Rank(List<TeamRanking> ranking)
        {
            for (int i = 0; i < ranking.Count(); i++)
            {
                ranking[i].Rank = i + 1;
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
