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
                new Team { Id = 1, Description = "Default Team" }
            };

            teams.ForEach(s => context.Teams.Add(s));
            context.SaveChanges();
        }
    }
}