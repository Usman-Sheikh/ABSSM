using ABSM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ABSM.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();
        public DashboardController()
        {
        }

        public DashboardController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: Admin/Dashboard
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult CheckRole()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index","Shops",new { area="Admin"});

            }
            else if(User.IsInRole("Shop"))
            {
                var shop = db.Shops.Where(s => s.UserName == User.Identity.Name).SingleOrDefault();
                if (shop != null)
                {
                    Session["Shop"] = shop.ShopID;
                    return RedirectToAction("ListProducts", "Owner", new { area = "Admin" });
                }
                else
                {
                    TempData["Message"] = "Shop not alloted, Please Contact Administrator";
                    return RedirectToAction("Login");
                }
               
            }

            else
            {
                TempData["Message"] = "You are not authorized";
                return RedirectToAction("Login");
            }
           
        }
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (TempData["Message"] == null)
            {
                TempData["Message"] = "";
            }
            
            ViewBag.Message = TempData["Message"].ToString();
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    
                 return RedirectToAction("CheckRole");
                  
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Dashboard",new { area="Admin"});
        }



        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        #endregion
    }
}