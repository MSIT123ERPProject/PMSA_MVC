namespace PMS_Inventory_huan.Migrations
{
    using Microsoft.AspNet.Identity;
    using PMS_Inventory_huan.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PMS_Inventory_huan.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PMS_Inventory_huan.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            var store = new ApplicationUserStore(context);
            var userManager = new ApplicationUserManager(store);
            var roleStore = new ApplicationRoleStore(context);
            var roleManager = new ApplicationRoleManager(roleStore);

            // ∑sºW Buyer ===================================================
            var userBuyercheck = userManager.FindByName("Buyer");

            if (userBuyercheck == null)
            {
                userBuyercheck = new ApplicationUser
                {
                    EmployeeID = "CE00002",
                    realName = "™ÙßQ∂≤",
                    UserName = "CE00002",
                    Email = "as123471@yahoo.com",
                    AccountStatus = "E",
                    CreateDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    SendLetterStatus = "Y",
                    SendLetterDate = DateTime.Now,
                    PasswordHash = userManager.PasswordHasher.HashPassword("P@ssw0rd"),
                    SecurityStamp = Guid.NewGuid().ToString("D")
                };
                context.Users.AddOrUpdate(x => x.Id, userBuyercheck);
                // ∑sºW®§?E==================================================
                //Create Role Buyer if it does not exist // ∑sºW Roles [Buyer]
                var role1 = roleManager.FindByName("Buyer");
                if (role1 == null)
                {
                    role1 = new ApplicationRole("Buyer", "çÃçwàı");
                    var roleresult = roleManager.Create(role1);
                }
                // Add user admin to Role Buyer if not already added // ±N user ≥]©w¨∞ [Buyer]
                var rolesForUser1 = userManager.GetRoles(userBuyercheck.Id);
                if (!rolesForUser1.Contains(role1.Name))
                {
                    var result = userManager.AddToRole(userBuyercheck.Id, role1.Name);
                }
            }
            // ∑sºW Manager ===================================================
            var userManagercheck = userManager.FindByName("Manager");

            if (userManagercheck == null)
            {
                userManagercheck = new ApplicationUser
                {
                    EmployeeID = "CE00015",
                    realName = "‰ÔãXâp",
                    UserName = "CE00015",
                    Email = "s19943588@yahoo.com",
                    AccountStatus = "E",
                    CreateDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    SendLetterStatus = "Y",
                    SendLetterDate = DateTime.Now,
                    PasswordHash = userManager.PasswordHasher.HashPassword("P@ssw0rd"),
                    SecurityStamp = Guid.NewGuid().ToString("D")
                };
                context.Users.AddOrUpdate(x => x.Id, userManagercheck);

                // ∑sºW®§?E==================================================
                //Create Role Manager if it does not exist // ∑sºW Roles [Manager]
                var role2 = roleManager.FindByName("Manager");
                if (role2 == null)
                {
                    role2 = new ApplicationRole("Manager", "çÃçwéÂä«");
                    var roleresult = roleManager.Create(role2);
                }
                // Add user admin to Role Admin if not already added // ±N user ≥]©w¨∞ [Admin]
                var rolesForUser2 = userManager.GetRoles(userManagercheck.Id);
                if (!rolesForUser2.Contains(role2.Name))
                {
                    var result = userManager.AddToRole(userManagercheck.Id, role2.Name);
                }
            }
            // ∑sºW Warehouse ===================================================
            var userWarehousecheck = userManager.FindByName("Warehouse");

            if (userWarehousecheck == null)
            {
                ApplicationUser user3 = new ApplicationUser
                {
                    EmployeeID = "CE00014",
                    realName = "?åN?",
                    UserName = "CE00014",
                    Email = "t2994645@yahoo.com",
                    AccountStatus = "E",
                    CreateDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    SendLetterStatus = "Y",
                    SendLetterDate = DateTime.Now,
                    PasswordHash = userManager.PasswordHasher.HashPassword("P@ssw0rd"),
                    SecurityStamp = Guid.NewGuid().ToString("D")
                };
                context.Users.AddOrUpdate(x => x.Id, user3);

                // ∑sºW®§?E==================================================
                //Create Role Admin if it does not exist // ∑sºW Roles [Admin]
                var role3 = roleManager.FindByName("Warehouse");
                if (role3 == null)
                {
                    role3 = new ApplicationRole("Warehouse", "ëqä«");
                    var roleresult = roleManager.Create(role3);
                }

                // Add user admin to Role Admin if not already added // ±N user ≥]©w¨∞ [Admin]
                var rolesForUser3 = userManager.GetRoles(user3.Id);
                if (!rolesForUser3.Contains(role3.Name))
                {
                    var result = userManager.AddToRole(user3.Id, role3.Name);
                }
            }

            // ∑sºW Admin===================================================
            var userAdmincheck = userManager.FindByName("Admin");

            if (userAdmincheck == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    EmployeeID = "T000000001",
                    realName = "â§?êu",
                    UserName = "Admin",
                    Email = "admin@example.com",
                    AccountStatus = "E",
                    CreateDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    SendLetterStatus = "Y",
                    SendLetterDate = DateTime.Now,
                    PasswordHash = userManager.PasswordHasher.HashPassword("P@ssw0rd"),
                    SecurityStamp = Guid.NewGuid().ToString("D")
                };
                context.Users.AddOrUpdate(x => x.Id, user);

                // ∑sºW®§?E==================================================
                //Create Role Admin if it does not exist // ∑sºW Roles [Admin]
                var role = roleManager.FindByName("Admin");
                if (role == null)
                {
                    role = new ApplicationRole("Admin", "ån?ä«?àı");
                    var roleresult = roleManager.Create(role);
                }

                // Add user admin to Role Admin if not already added // ±N user ≥]©w¨∞ [Admin]
                var rolesForUser = userManager.GetRoles(user.Id);
                if (!rolesForUser.Contains(role.Name))
                {
                    var result = userManager.AddToRole(user.Id, role.Name);
                }
            }
            // ∑sºW Supplier ===================================================
            var userSuplliercheck = userManager.FindByName("Supplier");

            if (userSuplliercheck == null)
            {
                ApplicationUser user4 = new ApplicationUser
                {
                    EmployeeID = "SE00001",
                    realName = "ó—ê≠óË",
                    UserName = "SE00001",
                    Email = "h1051235@gmail.com",
                    AccountStatus = "E",
                    CreateDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    SendLetterStatus = "Y",
                    SendLetterDate = DateTime.Now,
                    PasswordHash = userManager.PasswordHasher.HashPassword("P@ssw0rd"),
                    SecurityStamp = Guid.NewGuid().ToString("D")
                };
                context.Users.AddOrUpdate(x => x.Id, user4);

                // ∑sºW®§?E==================================================
                //Create Role Admin if it does not exist // ∑sºW Roles [Admin]
                var role4 = roleManager.FindByName("Supplier");
                if (role4 == null)
                {
                    role4 = new ApplicationRole("Supplier", "?ú‰è§");
                    var roleresult = roleManager.Create(role4);
                }

                // Add user admin to Role Admin if not already added // ±N user ≥]©w¨∞ [Admin]
                var rolesForUser4 = userManager.GetRoles(user4.Id);
                if (!rolesForUser4.Contains(role4.Name))
                {
                    var result = userManager.AddToRole(user4.Id, role4.Name);
                }
            }
        }
    }
}