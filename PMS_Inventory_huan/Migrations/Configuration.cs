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

            // 新增 Buyer ===================================================
            var userBuyercheck = userManager.FindByName("Buyer");

            if (userBuyercheck == null)
            {
                userBuyercheck = new ApplicationUser
                {
                    EmployeeID = "CE00002",
                    realName = "邱利雯",
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
                // 新增角色===================================================
                //Create Role Buyer if it does not exist // 新增 Roles [Buyer]
                var role1 = roleManager.FindByName("Buyer");
                if (role1 == null)
                {
                    role1 = new ApplicationRole("Buyer", "採購員");
                    var roleresult = roleManager.Create(role1);
                }
                // Add user admin to Role Buyer if not already added // 將 user 設定為 [Buyer]
                var rolesForUser1 = userManager.GetRoles(userBuyercheck.Id);
                if (!rolesForUser1.Contains(role1.Name))
                {
                    var result = userManager.AddToRole(userBuyercheck.Id, role1.Name);
                }
            }
            // 新增 Manager ===================================================
            var userManagercheck = userManager.FindByName("Manager");

            if (userManagercheck == null)
            {
                userManagercheck = new ApplicationUser
                {
                    EmployeeID = "CE00015",
                    realName = "蔡宜英",
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

                // 新增角色===================================================
                //Create Role Manager if it does not exist // 新增 Roles [Manager]
                var role2 = roleManager.FindByName("Manager");
                if (role2 == null)
                {
                    role2 = new ApplicationRole("Manager", "採購主管");
                    var roleresult = roleManager.Create(role2);
                }
                // Add user admin to Role Admin if not already added // 將 user 設定為 [Admin]
                var rolesForUser2 = userManager.GetRoles(userManagercheck.Id);
                if (!rolesForUser2.Contains(role2.Name))
                {
                    var result = userManager.AddToRole(userManagercheck.Id, role2.Name);
                }
            }
            // 新增 Warehouse ===================================================
            var userWarehousecheck = userManager.FindByName("Warehouse");

            if (userWarehousecheck == null)
            {
                ApplicationUser user3 = new ApplicationUser
                {
                    EmployeeID = "CE00014",
                    realName = "李君豪",
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

                // 新增角色===================================================
                //Create Role Admin if it does not exist // 新增 Roles [Admin]
                var role3 = roleManager.FindByName("Warehouse");
                if (role3 == null)
                {
                    role3 = new ApplicationRole("Warehouse", "倉管");
                    var roleresult = roleManager.Create(role3);
                }

                // Add user admin to Role Admin if not already added // 將 user 設定為 [Admin]
                var rolesForUser3 = userManager.GetRoles(user3.Id);
                if (!rolesForUser3.Contains(role3.Name))
                {
                    var result = userManager.AddToRole(user3.Id, role3.Name);
                }
            }

            // 新增 Admin===================================================
            var userAdmincheck = userManager.FindByName("Admin");

            if (userAdmincheck == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    EmployeeID = "T000000001",
                    realName = "王資訊",
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

                // 新增角色===================================================
                //Create Role Admin if it does not exist // 新增 Roles [Admin]
                var role = roleManager.FindByName("Admin");
                if (role == null)
                {
                    role = new ApplicationRole("Admin", "系統管理員");
                    var roleresult = roleManager.Create(role);
                }

                // Add user admin to Role Admin if not already added // 將 user 設定為 [Admin]
                var rolesForUser = userManager.GetRoles(user.Id);
                if (!rolesForUser.Contains(role.Name))
                {
                    var result = userManager.AddToRole(user.Id, role.Name);
                }
            }
            // 新增 Supplier ===================================================
            var userSuplliercheck = userManager.FindByName("Supplier");

            if (userSuplliercheck == null)
            {
                ApplicationUser user4 = new ApplicationUser
                {
                    EmployeeID = "SE00001",
                    realName = "林政苓",
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

                // 新增角色===================================================
                //Create Role Admin if it does not exist // 新增 Roles [Admin]
                var role4 = roleManager.FindByName("Supplier");
                if (role4 == null)
                {
                    role4 = new ApplicationRole("Supplier", "供應商");
                    var roleresult = roleManager.Create(role4);
                }

                // Add user admin to Role Admin if not already added // 將 user 設定為 [Admin]
                var rolesForUser4 = userManager.GetRoles(user4.Id);
                if (!rolesForUser4.Contains(role4.Name))
                {
                    var result = userManager.AddToRole(user4.Id, role4.Name);
                }
            }
        }
    }
}