using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ABSM.Services;
using ABSM.ViewModels;

namespace ABSM.Controllers
{
    public class StoreController : Controller
    {
        private readonly StoreService _store;
        public StoreController() : this(new StoreService()) { }
        public StoreController(StoreService service)
        {
            _store = service;
        }
        // GET: Store
        public async Task<ActionResult> Index()
        {
            
            var shops = await _store.GetShopsAsync();
            var files = await _store.GetFilesAsync();
            var ShopVm = new ShopViewModel
            {
                Shops= shops,
                FileUploads=files
            };
            return View(ShopVm);


          

        }


        public ActionResult Shops()
        {
            ViewBag.Names = _store.GetShops();
            return PartialView("_Shops");
        }


        public async Task<ActionResult> Browse(int id, string name)
        {
            var categories = await _store.GetCategoriesByIdAsync(id);
            if (name == null && !Request.IsAjaxRequest())
            {
                var products = await _store.GetProductsByShop(id);
                var categoryProductVM = new CategoryProductViewModel
                {
                    categories = categories,
                    products = products

                };
                //return View(categoryProductVM);
                return View(categoryProductVM);
            }
            else
            {
              
                    var products = await _store.GetProductsForAsync(name);
                    var categoryProductVM = new CategoryProductViewModel
                    {
                        categories = categories,
                        products = products

                    };
                return PartialView("_Products", categoryProductVM);

            }
        }

   

        public async Task<ActionResult> Details(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();                                
            }

            var product = await _store.GetProductByIdAsync(id.Value);

            if (product == null)
            {
                return HttpNotFound();                
            }

            return View(product);
        }


        public ActionResult NewArrivals()
        {
            var products= _store.GetNewProducs();

            return PartialView("_NewArrivals", products);
        }

    }
}