using System.Runtime.InteropServices;

namespace FormsApp.Models
{
    public class Repository
    {
        private static readonly List<Product> _products = new(); //kapalı bir List kuruyoruz
        private static readonly List<Category> _categoryries = new(); //kapalı bir List kuruyoruz

        static Repository()
        {
            _categoryries.Add(new Category {CategoryId=1,Name="Telefon"});
            _categoryries.Add(new Category {CategoryId=2,Name="Bilgisayar"});

            _products.Add(new Product{ProductId=1,Name="Apple Iphone 14",Price=44999,IsActive=true,Image="iphone_14.png", CategoryId=1});
            _products.Add(new Product{ProductId=2,Name="Samsung Galaxy S24 Mor",Price=42499,IsActive=true,Image="s24-Mor.jpg", CategoryId=1});
            _products.Add(new Product{ProductId=3,Name="Honor Magic 6 Pro",Price=60000,IsActive=false,Image="honor-magic6-pro.png", CategoryId=1});
            
            _products.Add(new Product{ProductId=4,Name="Asus Rog Strix Laptop",Price=60000,IsActive=true,Image="asus.png", CategoryId=2});
        }
        public static List<Product> Products //Kapalı sistemden veri çekmek için Get kullanıyoruz
        {
            get
            {
                return _products;
            }
        }

        public static void CreateProduct (Product entity){

            _products.Add(entity);
        }

        public static void EditProduct (Product updatedProduct)
        {
            var entity = _products.FirstOrDefault(p => p.ProductId == updatedProduct.ProductId);
            if(entity != null)
            {
                entity.Name=updatedProduct.Name;
                entity.Price=updatedProduct.Price;
                entity.Image=updatedProduct.Image;
                entity.CategoryId=updatedProduct.CategoryId;
                entity.IsActive=updatedProduct.IsActive;
            }
        }
        public static void DeleteProduct (Product deletedProduct)
        {
            var entity = _products.FirstOrDefault(p => p.ProductId == deletedProduct.ProductId);

            if(entity != null)
            {
                _products.Remove(entity);
            }
        }

        public static List<Category> Categories
        {
            get
            {
                return _categoryries;
            }
        }
        
    }
}