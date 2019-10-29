﻿using PMS_Inventory_huan.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace PMS_Inventory_huan.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersAdminController : BaseController
    {
        public UsersAdminController()
        {
        }

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public UsersAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        //
        // GET: /Users/
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View(await UserManager.Users.ToListAsync());
        }

        //
        // GET: /Users/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            if (id > 0)
            {
                // Process normally:
                var user = await UserManager.FindByIdAsync(id);
                ViewBag.RoleNames = await UserManager.GetRolesAsync(user.Id);
                return View(user);
            }
            // Return Error:
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        //
        // GET: /Users/Create
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            //Get the list of Roles
            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            return View();
        }

        public string generateFirstPwd()
        {
            //salt: 隨機位元組=>演算法=>計算後給值
            RNGCryptoServiceProvider rngbyte = new RNGCryptoServiceProvider();
            byte[] bytesalt = new byte[8];
            rngbyte.GetBytes(bytesalt);
            string salty = Convert.ToBase64String(bytesalt);
            return salty;
        }

        //
        // POST: /Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                ApplicationDbContext context = new ApplicationDbContext();
                ApplicationRoleStore roleStore = new ApplicationRoleStore(context);
                ApplicationRoleManager roleManager = new ApplicationRoleManager(roleStore);
                //新增 user 資料
                var user = new ApplicationUser
                {
                    EmployeeID = model.EmployeeID,
                    UserName = model.EmployeeID,
                    realName = model.realName,
                    Email = model.Email,
                    AccountStatus = "R",
                    CreateDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    SendLetterStatus = "Y",
                    SendLetterDate = DateTime.Now,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                };
                // TODO
                // 密碼寫死先統一 P@ssw0rd
                // string pwd = generateFirstPwd();
                string pwd = "P@ssw0rd";
                var result = await UserManager.CreateAsync(user, pwd);
                ViewBag.RoleId = new SelectList(await roleManager.Roles.ToListAsync(), "Name", "Name");
                // 如果成功新增 進行驗證
                if (result.Succeeded)
                {
                    // 新增成功才有 userID(流水號)
                    // 如果選擇不為空
                    if (selectedRoles != null)
                    {
                        // 將 user 加入角色池  //Add User to the selected Roles
                        var resultSelect = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
                        // 加入失敗的話 讓他可以選 RoleId
                        if (!resultSelect.Succeeded)
                        {
                            ModelState.AddModelError("", resultSelect.Errors.First());
                            ViewBag.RoleId = new SelectList(await roleManager.Roles.ToListAsync(), "Name", "Name");
                            return View();
                        }
                    }
                    //?????
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // TODO 帳戶確認及密碼重設
                    // 如需如何進行帳戶確認及密碼重設的詳細資訊，請前往 https://go.microsoft.com/fwlink/?LinkID=320771
                    // 傳送包含此連結的電子郵件
                    var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("PMS_Inventory_huan");
                    UserManager.UserTokenProvider = new Microsoft.AspNet.Identity.Owin.DataProtectorTokenProvider<ApplicationUser, int>(provider.Create("TokenName"));
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "確認您的帳戶", $"<h1>請按一下此連結確認您的帳戶</h1> <a href='{callbackUrl}'> 這裏 {pwd}</a>");

                    ViewBag.Link = callbackUrl;
                    return View("DisplayEmail");
                }
            }
            // 如果執行到這裡，發生某項失敗，則重新顯示表單
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        //
        // GET: /Users/Edit/1
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            if (id > 0)
            {
                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                var userRoles = await UserManager.GetRolesAsync(user.Id);
                return View(new EditUserViewModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
                    {
                        Selected = userRoles.Contains(x.Name),
                        Text = x.Name,
                        Value = x.Name
                    })
                });
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Email,Id")] EditUserViewModel editUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                user.UserName = editUser.Email;
                user.Email = editUser.Email;

                var userRoles = await UserManager.GetRolesAsync(user.Id);

                selectedRole = selectedRole ?? new string[] { };

                var result = await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }

        //
        // GET: /Users/Delete/5
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            if (id > 0)
            {
                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        //
        // POST: /Users/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (ModelState.IsValid)
            {
                if (id > 0)
                {
                    var user = await UserManager.FindByIdAsync(id);
                    if (user == null)
                    {
                        return HttpNotFound();
                    }
                    var result = await UserManager.DeleteAsync(user);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", result.Errors.First());
                        return View();
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            return View();
        }
    }
}