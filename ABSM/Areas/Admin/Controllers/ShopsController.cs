using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ABSM.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;
using System.Web.Hosting;

namespace ABSM.Areas.Admin.Controllers
{
   [Authorize(Roles ="Admin")]
    public class ShopsController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ShopsController()
        {

        }

        public ShopsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    var roles = ApplicationRoleManager.Create(HttpContext.GetOwinContext());

                    if (!await roles.RoleExistsAsync("Shop"))
                    {
                        await roles.CreateAsync(new IdentityRole { Name = "Shop" });
                    }
                    await UserManager.AddToRoleAsync(user.Id, "Shop");

                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Dashboard", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    return RedirectToAction("Index", "Shops");
                }
                AddErrors(result);
            }

            return View(model);
        }

        // GET: Admin/Shops
        public ActionResult Index()
        {
            return View(db.Shops.ToList());
        }

        // GET: Admin/Shops/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shop shop = db.Shops.Find(id);
            if (shop == null)
            {
                return HttpNotFound();
            }
            return View(shop);
        }

        // GET: Admin/Shops/Create
        public ActionResult Create()
        {
            var shopkeepers = db.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains("c3ecfd61-1f5f-495f-90a7-85fc15955264"))
                       .OrderBy(u => u.UserName).ToList();

            var UsersList= new List<ApplicationUser>();
            foreach (var item in shopkeepers)
            {
              var user = db.Shops.Select(x => x.UserName).Contains(item.UserName).ToString();  
                if (user=="False")
                {
                    UsersList.Add(item);
                }
            }

            ViewBag.UserName = UsersList.Select(uu =>
            new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
            return View();
        }

        // POST: Admin/Shops/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Shop shop, HttpPostedFileBase doc)
        {
            string path;
            if ( doc != null )
            {

                var filename = Path.GetFileName(doc.FileName);
                var extension = Path.GetExtension(filename).ToLower();
                if (extension == ".jpg" || extension == ".png")
                {
                    path = HostingEnvironment.MapPath(Path.Combine("~/Content/Images/", filename));
                    doc.SaveAs(path);
                    shop.ImageUrl = "~/Content/Images/" + filename;
                }
                else
                {
                    ModelState.AddModelError("", "Document size must be less then 5MB");
                    return View(shop);
                }
                if (ModelState.IsValid)
                {

                    db.Shops.Add(shop);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            var shopkeepers = db.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains("f0d11509-27fc-48cf-9dbd-7983c4c828ed"))
                       .OrderBy(u => u.UserName).ToList();

            var UsersList = new List<ApplicationUser>();
            foreach (var item in shopkeepers)
            {
                var user = db.Shops.Select(x => x.UserName).Contains(item.UserName).ToString();
                if (user == "False")
                {
                    UsersList.Add(item);
                }
            }

            ViewBag.UserName = UsersList.Select(uu =>
            new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
            return View(shop);
        }

        // GET: Admin/Shops/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shop shop = db.Shops.Find(id);
            if (shop == null)
            {
                return HttpNotFound();
            }

            var shopkeepers = db.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains("f0d11509-27fc-48cf-9dbd-7983c4c828ed"))
                       .OrderBy(u => u.UserName).ToList();

            var UsersList = new List<ApplicationUser>();
            foreach (var item in shopkeepers)
            {
                var user = db.Shops.Select(x => x.UserName).Contains(item.UserName).ToString();
                if (user == "False")
                {
                    UsersList.Add(item);
                }
            }

            ViewBag.UserName = UsersList.Select(uu =>
            new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
            return View(shop);


        }

        // POST: Admin/Shops/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Shop shop, string ImageValue, HttpPostedFileBase doc)
        {
            string path;
            if (doc != null && ModelState.IsValid)
            {

                var filename = Path.GetFileName(doc.FileName);
                var extension = Path.GetExtension(filename).ToLower();
                if (extension == ".jpg" || extension == ".png")
                {
                    path = HostingEnvironment.MapPath(Path.Combine("~/Content/Images/", filename));
                    doc.SaveAs(path);
                    shop.ImageUrl = "~/Content/Images/" + filename;
                    db.Entry(shop).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Only Image type file allowed");
                    return View(shop);
                }
              
            }
            else if (ModelState.IsValid)
            {
                shop.ImageUrl = ImageValue;
                db.Entry(shop).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           

           
            return View(shop);
        }

        // GET: Admin/Shops/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shop shop = db.Shops.Find(id);
            if (shop == null)
            {
                return HttpNotFound();
            }
            return View(shop);
        }

        // POST: Admin/Shops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Shop shop = db.Shops.Find(id);
            var products = db.Products.Where(c => c.ShopID == id).ToList();
            db.Products.RemoveRange(products);
            var categories = db.Categories.Where(c => c.ShopID == id).ToList();
            db.Categories.RemoveRange(categories);
            db.Shops.Remove(shop);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public ActionResult Complains()
        {
            var complains = db.Complains.OrderByDescending(o => o.ComplainID).ToList();
            return View(complains);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
                db.Dispose();
            }

            base.Dispose(disposing);
        }



       
    }
}
