using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PMS_Inventory_huan.Models
{
    // 您可將更多屬性新增至 ApplicationUser 類別，藉此為使用者新增設定檔資料，如需深入了解，請瀏覽 https://go.microsoft.com/fwlink/?LinkID=317594。
    public class ApplicationUser : IdentityUser<int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IUser<int>
    {
        private DateTime _CreateDate;
        private DateTime _ModifiedDate;
        private DateTime _SendLetterDate;

        [StringLength(10)]
        public string EmployeeID { get; set; }

        [StringLength(30)]
        public string realName { get; set; }

        public override int Id { get => base.Id; set => base.Id = value; }

        [Required]
        [StringLength(10)]
        public override string UserName { get; set; }

        [StringLength(30)]
        public string Mobile { get; set; }

        [StringLength(30)]
        public string Tel { get; set; }

        [StringLength(1)]
        public string AccountStatus { get; set; }

        public DateTime CreateDate
        {
            get
            {
                return _CreateDate;
            }
            set
            {
                _CreateDate = DateTime.Now;
            }
        }

        public DateTime ModifiedDate
        {
            get
            {
                return _ModifiedDate;
            }
            set
            {
                _ModifiedDate = DateTime.Now;
            }
        }

        [StringLength(1)]
        public string SendLetterStatus { get; set; }

        public DateTime SendLetterDate
        {
            get
            {
                return _SendLetterDate;
            }
            set
            {
                _SendLetterDate = DateTime.Now;
            }
        }

        public override string PasswordHash { get; set; }
        public override string SecurityStamp { get => base.SecurityStamp; set => base.SecurityStamp = value; }

        //===================================================================================
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // 注意 authenticationType 必須符合 CookieAuthenticationOptions.AuthenticationType 中定義的項目
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 在這裡新增自訂使用者宣告
            return userIdentity;
        }
    }

    public class ApplicationUserLogin : IdentityUserLogin<int> { }

    public class ApplicationUserClaim : IdentityUserClaim<int> { }

    public class ApplicationUserRole : IdentityUserRole<int> { }

    public class ApplicationRole : IdentityRole<int, ApplicationUserRole>, IRole<int>
    {
        public string Description { get; set; }

        public ApplicationRole()
        {
        }

        public ApplicationRole(string name)
            : this()
        {
            this.Name = name;
        }

        public ApplicationRole(string name, string description)
            : this(name)
        {
            this.Description = description;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int,
    ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        private string v;

        public ApplicationDbContext() : base("DefaultConnection")
        {
        }

        static ApplicationDbContext()
        {
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<PMS_Inventory_huan.Models.ApplicationUserRole> ApplicationUserRoles { get; set; }
        //請使用 DbSet<Users> 呼叫 ApplicationUser 實體，而非以下此句
        //public System.Data.Entity.DbSet<PMS_Inventory_huan.Models.ApplicationUser> ApplicationUsers { get; set; }
    }

    //====================================================
    public class ApplicationUserStore :
        UserStore<ApplicationUser, ApplicationRole, int,
        ApplicationUserLogin, ApplicationUserRole,
        ApplicationUserClaim>, IUserStore<ApplicationUser, int>,
        IDisposable
    {
        public ApplicationUserStore() : this(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }

        public ApplicationUserStore(DbContext context)
            : base(context)
        {
        }
    }

    public class ApplicationRoleStore
        : RoleStore<ApplicationRole, int, ApplicationUserRole>,
        IQueryableRoleStore<ApplicationRole, int>,
        IRoleStore<ApplicationRole, int>, IDisposable
    {
        public ApplicationRoleStore()
            : base(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }

        public ApplicationRoleStore(DbContext context)
            : base(context)
        {
        }
    }
}