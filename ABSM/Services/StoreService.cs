using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using ABSM.Models;
using System.Web.Mvc;

namespace ABSM.Services
{
    public class StoreService
    {
        private readonly ApplicationDbContext _db;

        public StoreService() : this(new ApplicationDbContext()) { }
        public StoreService(ApplicationDbContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Shop>> GetShopsAsync()
        {
            return await _db.Shops.OrderBy(c => c.Name).ToArrayAsync();
        }
        public async Task<IEnumerable<FileUpload>> GetFilesAsync()
        {
            return await _db.FileUploads.OrderBy(c => c.ID).ToArrayAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _db.Categories.OrderBy(c => c.Name).ToArrayAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesByIdAsync(int id)
        {
            return await _db.Categories.OrderBy(c => c.Name).Where(s=>s.ShopID==id).ToArrayAsync();
        }

        

        public async Task<IEnumerable<Product>> GetProductsForAsync(string category)
        {
            return await _db.Products.Include("Category")
                .Where(p => p.Category.Name == category).ToArrayAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _db.Products.Take(10).OrderByDescending(x=>x.Name).ToListAsync();
                
        }

        public async Task<IEnumerable<Product>> GetProductsByShop(int id)
        {
            return await _db.Products.Take(10).OrderByDescending(x => x.Name).Where(s => s.ShopID == id).ToListAsync();

        }
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _db.Products.Include("Category")
                .Where(p => p.ProductID == id).SingleOrDefaultAsync();
        }


        public IEnumerable<Product> GetNewProducs()
        {
            return _db.Products.Include("Shop").Include("Category").Take(10).OrderByDescending(x => x.Name).ToList();
                
        }


        public IList<SelectListItem> GetShops()
        {
            return _db.Shops.Select(S => new SelectListItem { Value = S.ShopID.ToString(), Text = S.Name }).ToList();
        }
    }
}