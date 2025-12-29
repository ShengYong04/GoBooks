using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MVCBookStore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            MvcApplication.SeedRolesAndAdmin();
        }

        private static void SeedRolesAndAdmin()
        {
            var context = new Models.ApplicationDbContext();
            var roleManager = new Microsoft.AspNet.Identity.RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(
                new Microsoft.AspNet.Identity.EntityFramework.RoleStore<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(context));
            var userManager = new Microsoft.AspNet.Identity.UserManager<Models.ApplicationUser>(
                new Microsoft.AspNet.Identity.EntityFramework.UserStore<Models.ApplicationUser>(context));

            if (!roleManager.RoleExists("Admin"))
                roleManager.Create(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole("Admin"));

            if (!roleManager.RoleExists("User"))
                roleManager.Create(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole("User"));

            if (userManager.FindByName("admin") == null)
            {
                var adminUser = new Models.ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@bookstore.com",
                    FullName = "Admin"
                };
                string password = "Admin123!";
                var result = userManager.Create(adminUser, password);
                if (result.Succeeded)
                    userManager.AddToRole(adminUser.Id, "Admin");
            }
        }

    }
}
