using System.Collections.Generic; // Để dùng ICollection
using BakeryShopAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
namespace BakeryShopAPI.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } // Tên loại bánh (VD: Bánh kem, Bánh mì)
        public string Description { get; set; }

        // Mối quan hệ: Một danh mục có nhiều bánh
        public ICollection<Product> Products { get; set; }
    }
}