
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ABSM.Models
{
    public class DbPopulate
    {
        public async static Task AddDataAsync()
        {
            using (var db = new ApplicationDbContext())
            {

                var computers = new Category { Name = "Computers" };
                var printers = new Category { Name = "Printers" };
                var books = new Category { Name = "Books" };

                db.Categories.AddRange(new Category[] { computers, printers, books });

                db.Products.Add(new Models.Product
                {
                    Category = computers,
                    Name = "Really Fast Computer"
                });

                db.Products.Add(new Models.Product
                {
                    Category = computers,
                    Name = "Mainstream Computer"
                });

                db.Products.Add(new Models.Product
                {
                    Category = printers,
                    Name = "Fast Color Laser Printer"
                });

                db.Products.Add(new Models.Product
                {
                    Category = printers,
                    Name = "Normal Ink Jet Printer"
                });

                db.Products.Add(new Models.Product
                {
                    Category = printers,
                    Name = "Dot Matrix Printer"
                });

                db.Products.Add(new Models.Product
                {
                    Category = books,
                    Name = "Really Interesting Book"
                });

                db.Products.Add(new Models.Product
                {
                    Category = books,
                    Name = "Boring Book You Have to Read"
                });

                db.Products.Add(new Models.Product
                {
                    Category = books,
                    Name = "One of My Books"
                });

                await db.SaveChangesAsync();
            }
        }
    }
}