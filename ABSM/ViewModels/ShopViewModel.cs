using ABSM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABSM.ViewModels
{
    public class ShopViewModel
    {
        public IEnumerable<Shop> Shops { get; set; }
        public IEnumerable<FileUpload> FileUploads { get; set; }
    }
}