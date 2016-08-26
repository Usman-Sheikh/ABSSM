using ABSM.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ABSM.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {

        private ApplicationUserManager _userManager;

        public RoleController()
        {
        }

        public RoleController(ApplicationUserManager userManager)
        {
            UserManager = userManager;

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
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            ViewBag.Roles = db.Roles.OrderBy(r => r.Name).ToList().Select
             (ro => new SelectListItem { Value = ro.Name.ToString(), Text = ro.Name }).ToList();

            ViewBag.Users = db.Users.OrderBy(u => u.UserName).ToList().Select(uu =>
            new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();


            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RoleAddToUser(string UserName, string RoleName)
        {
            //var context = new Models.ApplicationDbContext();

            if (db == null)
            {
                throw new ArgumentNullException("context", "Context must not be null.");
            }

            var user = db.Users.Where
                (u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            //var userStore = new UserStore<ApplicationUser>(context);
            //var userManager = new UserManager<ApplicationUser>(userStore);
            await UserManager.AddToRoleAsync(user.Id, RoleName);


            ViewBag.Message = "Role created successfully !";

            // Repopulate Dropdown Lists
            var rolelist = db.Roles.OrderBy(r => r.Name).ToList().Select
                (rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = rolelist;
            var userlist = db.Users.OrderBy(u => u.UserName).ToList().Select(uu =>
            new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
            ViewBag.Users = userlist;

            return View("Index");
        }


        //Getting a List of Roles for a User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetRoles(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                var user = db.Users.Where(u => u.UserName.Equals
                (UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                IList<string> role = await UserManager.GetRolesAsync(user.Id);
                ViewBag.RolesForThisUser = role;


                //var roles = db.Roles.Select(rr => rr.Name).Where(ro=>ro..Where(se=>se.UserId==user.Id) ).FirstOrDefault();
                //ViewBag.RolesForThisUser = db.Roles.ToList().Select(rr => rr.Users.Where(r => r.UserId == user.Id)).FirstOrDefault();

                //var userStore = new UserStore<ApplicationUser>(context);
                //var userManager = new UserManager<ApplicationUser>(userStore);

                //ViewBag.RolesForThisUser = roles;


                // Repopulate Dropdown Lists
                var rolelist = db.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.Roles = rolelist;
                var userlist = db.Users.OrderBy(u => u.UserName).ToList().Select(uu =>
                new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
                ViewBag.Users = userlist;
                ViewBag.Message = "Roles retrieved successfully !";
            }

            return View("Index");
        }

        //Deleting a User from A Role
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteRoleForUser(string UserName, string RoleName)
        {
            //var account = new AccountController();
            //var context = new Models.ApplicationDbContext();
            var user = db.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            //var userStore = new UserStore<ApplicationUser>(context);
            //var userManager = new UserManager<ApplicationUser>(userStore);


            if (await UserManager.IsInRoleAsync(user.Id, RoleName))
            {
                await UserManager.RemoveFromRoleAsync(user.Id, RoleName);
                ViewBag.Message = "Role removed from this user successfully !";
            }
            else
            {
                ViewBag.Message = "This user doesn't belong to selected role.";
            }

            // Repopulate Dropdown Lists
            var rolelist = db.Roles.OrderBy(r => r.Name).ToList().Select
                (rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = rolelist;
            var userlist = db.Users.OrderBy(u => u.UserName).ToList().Select(uu =>
            new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
            ViewBag.Users = userlist;

            return View("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Create(FormCollection model)
        {
            try
            {
                db.Roles.Add(new IdentityRole()
                {
                    Name = model["RoleName"]
                });
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                return View();
            }
        }


        //[HttpPost]
        public async Task<ActionResult> Delete(string roleName)
        {
            var role = db.Roles.Where(r => r.Name.Equals
            (roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            db.Roles.Remove(role);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public ActionResult Edit(string roleName)
        {
            var thisRole = db.Roles.Where(r => r.Name.Equals
            (roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            return View(thisRole);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(IdentityRole role)
        {
            try
            {
                db.Entry(role).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}