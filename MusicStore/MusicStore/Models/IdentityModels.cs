using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;
using System.Net;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

namespace MusicStore.Models
{
    // 可以通过向 ApplicationUser 类添加更多属性来为用户添加配置文件数据。若要了解详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=317594。
    public class ApplicationUser : IdentityUser
    {
        //  public int Age { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 在此处添加自定义用户声明
            return userIdentity;
        }
    }

    public class ApplicationDBInitialize : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var userManger = HttpContext.Current.GetOwinContext().Get<ApplicationUserManager>();

             var roleManger = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();

            var roleName = "admin";
            var userName = "admin@123.com";
            var userPsw = "Admin@123";

             var role = roleManger.FindByName(roleName);
            if (role == null)
            {
                role = new IdentityRole(roleName);
                roleManger.Create(role);
               }

            var user = userManger.FindByName(userName);
            if (user == null)
            {
                user = new ApplicationUser();
                user.Email = userName;
                user.UserName = userName;
                userManger.Create(user,userPsw);
            }
            var roles = userManger.GetRoles(user.Id);

            if (!roles.Contains(roleName))
            {
                userManger.AddToRole(user.Id, roleName);
            }

            base.Seed(context);

        }
    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new ApplicationDBInitialize());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}