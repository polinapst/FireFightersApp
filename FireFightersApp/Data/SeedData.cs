using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FireFightersApp.Authorization;

namespace FireFightersApp.Data
{
    public class SeedData
    {
        public static async Task Initialize(
            IServiceProvider serviceProvider,
            string password = "Test@1234")
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                //dispatcher
                var dispatcher = await EnsureUser(serviceProvider, "dispatcher@demo.com", password);
                await EnsureRole(serviceProvider, dispatcher, Constants.CallDispatcherRole);

                //administrator
                var admin = await EnsureUser(serviceProvider, "admin@demo.com", password);
                await EnsureRole(serviceProvider, dispatcher, Constants.CallAdminRole);
            }
        }

        private static async Task<string> EnsureUser(
            IServiceProvider serviceProvider,
            string userName, string initPw)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(userName);

            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = userName,
                    Email = userName,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, initPw);
            }

            if (user == null)
            {
                throw new Exception("User did not get created. Problem with password");
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(
            IServiceProvider serviceProvider,
            string uid, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            IdentityResult ir;

            if (await roleManager.RoleExistsAsync(role) == false)
            {
                ir = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("User not existing");
            }

            ir = await userManager.AddToRoleAsync(user, role);

            return ir;
        }
    }
}
