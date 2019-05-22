using System;
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
            if (User.Identity.IsAuthenticated)
            {
                Guid userId = Guid.Parse(User.Identity.GetUserId());
                var user = db.Employees.Find(userId);

                if (user.IsManager)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                ViewBag.Productivity = user.Productivity;
                var tasks = user.Tasks;
                return View(tasks.ToList());
            }

            return RedirectToAction("Login", "Account");
        }

        // GET: Tasks/MarkAsDone
        public ActionResult MarkAsDone(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var task = db.Tasks.Find(id);

            task.IsDone = true;
            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();

            Employee employee = db.Employees.Find(task.EmployeeId);
            Productivity.CalculateProductivity(employee);
            Productivity.TeamProductivity(employee.Team);
            db.Entry(employee).State = EntityState.Modified;
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
