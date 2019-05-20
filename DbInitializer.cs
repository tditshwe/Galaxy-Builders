using System.Collections.Generic;
using GalaxyBuildersSystem.Models;

namespace GalaxyBuildersSystem
{
    public class DbInitializer : System.Data.Entity.CreateDatabaseIfNotExists<GalaxyContext>
    {
        protected override void Seed(GalaxyContext context)
        {
            var teams = new List<Team>
            {
                new Team { Id = 1, Description = "Development Team" },
                new Team { Id = 2, Description = "Marketing Team" },
                new Team { Id = 3, Description = "Graphic Design Team" },
                new Team { Id = 4, Description = "Hosting and Support Team" }
            };

            teams.ForEach(s => context.Teams.Add(s));
            context.SaveChanges();
        }
    }
}