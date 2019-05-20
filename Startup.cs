using System;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using GalaxyBuildersSystem.Models;

[assembly: OwinStartupAttribute(typeof(GalaxyBuildersSystem.Startup))]
namespace GalaxyBuildersSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesandUsers();
        }

        // In this method we will create default User roles and Manager user for logins
        private void CreateRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            GalaxyContext glxContext = new GalaxyContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // Creating Manager role 
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);

                //Here we create a Manager accounts who will manage teams				
                for (int i = 1; i < 5; i++)
                {
                    var user = new ApplicationUser();
                    user.UserName = string.Format("manager{0}@domain.com", i);
                    user.Email = string.Format("manager{0}@domain.com", i);
                    string userPWD = "P@ssw0rd";
                    var createUser = UserManager.Create(user, userPWD);

                    //Add User to Role Manager
                    if (createUser.Succeeded)
                    {
                        UserManager.AddToRole(user.Id, "Manager");

                        Employee emp = new Employee
                        {
                            Id = Guid.Parse(user.Id),
                            Name = "Manager" + i,
                            Productivity = 0,
                            IsManager = true,
                            Lastname = "Lastname",
                            TeamId = i
                        };

                        glxContext.Employees.Add(emp);                      
                    }
                }

            }

            // creating Creating Employee role 
            if (!roleManager.RoleExists("Employee"))
            {
                var role = new IdentityRole();
                role.Name = "Employee";
                roleManager.Create(role);
            }

            //Here we create Employee accounts				
            for (int i = 1; i < 3; i++)
            {
                var user = new ApplicationUser();
                user.UserName = string.Format("employee{0}@domain.com", i);
                user.Email = string.Format("employee{0}@domain.com", i);
                string userPWD = "P@ssw0rd";
                var createUser = UserManager.Create(user, userPWD);

                //Add User to Role Manager
                if (createUser.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Employee");

                    Employee emp = new Employee
                    {
                        Id = Guid.Parse(user.Id),
                        Name = "Employee" + i,
                        Lastname = "Lastname",
                        Productivity = 0,
                        IsManager = false,
                        TeamId = 1
                    };

                    glxContext.Employees.Add(emp);
                }
            }

            glxContext.SaveChanges();
        }
    }
}
