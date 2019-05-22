using System;
using System.Linq;
using GalaxyBuildersSystem.Models;

namespace GalaxyBuildersSystem
{
    public class Productivity
    {
        public static string GetCurrentUserName(string id)
        {
            GalaxyContext db = new GalaxyContext();
            
            var emp = db.Employees.Find(Guid.Parse(id));

            return string.Format("{0} {1}", emp.Name, emp.Lastname);
        }
    

        public static void CalculateProductivity(Employee em)
        {
            decimal done = 0;

            foreach (GTask t in em.Tasks)
            {
                if (t.IsDone)
                    done++;
            }

            if (em.Tasks.Count() > 0)
                em.Productivity = Math.Round(done / em.Tasks.Count(), 2, MidpointRounding.AwayFromZero);
            else
                em.Productivity = 0;
        }

        public static void TeamProductivity(Team team)
        {
            decimal toDo = 0;
            decimal done = 0;

            foreach (Employee e in team.Employees)
            {
                foreach (GTask t in e.Tasks)
                {
                    if (t.IsDone)
                        done++;
                }

                toDo += e.Tasks.Count();
            }

            if (toDo > 0)
                team.Productivity = Math.Round(done / toDo, 2, MidpointRounding.AwayFromZero);
            else
                team.Productivity = 0;
        }
    }
}