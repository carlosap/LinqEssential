using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace LinqEssential
{
    class Program
    {
        static void Main(string[] args)
        {
            Product product = new Product();
            ProductCategory categories = new ProductCategory();

            //------------Get Product--------------
            var products = from items in product.GetProducts()
                           select items;

            foreach (var item in products)
                Debug.WriteLine($"ProductID: {item.ProductId} Product Name: {item.Name} Product List Price: {item.ListPrice}");

            //------------Using Select Extention--------------
            var products2 = product.GetProducts().Select(x => x);

            foreach (var item in products2)
                Debug.WriteLine($"ProductID: {item.ProductId} Product Name: {item.Name} Product List Price: {item.ListPrice}");


            //------------Select Single Field--------------
            var products3 = from items in product.GetProducts()
                            select items.ListPrice;

            foreach (var item in products3)
                Debug.WriteLine($"Product Price: {item}");


            //------------Anonymous Types Projections--------------
            var products4 = from items in product.GetProducts()
                            select new
                            {
                                items.Name,
                                items.ListPrice
                            };

            foreach (var item in products4)
                Debug.WriteLine($"Product Name: {item.Name} Prize: {item.ListPrice}");

            //------------Anonymous Types Projections Changing Default--------------
            var products5 = from items in product.GetProducts()
                            select new
                            {
                               Name = items.Name,
                               Price = items.ListPrice
                            };

            foreach (var item in products5)
                Debug.WriteLine($"Product Name: {item.Name} Prize: {item.Price}");


            //------------Custom Types Projections--------------
            var products6 = from items in product.GetProducts()
                           select
                                new ProductInfo
                                {
                                    Name = items.Name,
                                    ProductId = items.ProductId
                                };

            foreach (var item in products6)
                Debug.WriteLine($"Product Info Name: {item.Name} ID: {item.ProductId}");

            //------------Filtering Data--------------
            var products7 = from items in product.GetProducts()
                            where items.ListPrice > 8.0m &&
                                  items.Name.StartsWith("wi")
                            select items;

            //------------Filtering With Method--------------
            var products8 = from items in product.GetProducts()
                            where Recent(items.SaleStartDate)
                            select items;

            //usin 'let' for better readiability
            var products9 = from items in product.GetProducts()
                            let dayTotals = DayDiffInDays(items.SaleStartDate)
                            where dayTotals > 5 && dayTotals <= 10
                            select items;

            //------------Sorting Query Rsults--------------
            var products10 = from items in product.GetProducts()
                             orderby items.ListPrice, items.Name ascending
                             select items;

            //------------Group Query Rsults--------------
            var products11 = from items in product.GetProducts()
                             group items by items.ProductCategoryID
                             into ProductCategories
                             select
                                new
                                {
                                    CategoryId = ProductCategories.Key,
                                    Products = ProductCategories
                                };

            foreach (var item in products11)
            {
                Debug.WriteLine($"Product ID Contains Items: {item.CategoryId}");
                Debug.WriteLine($"----------------------------------------------");
                foreach (var productItem in item.Products)
                    Debug.WriteLine($"Product Name: {productItem.Name} Price: {productItem.ListPrice}");

            }
            //------------Group Multiple-------------------------
            var products12 = from items in product.GetProducts()
                             group items by
                             new
                             {
                                 items.ProductCategoryID,
                                 items.Name
                             }
                             into ProductCategories
                             select
                                new
                                {
                                    CategoryId = ProductCategories.Key.ProductCategoryID,
                                    Name = ProductCategories.Key.Name,
                                    Products = ProductCategories
                                };

            foreach (var item in products12)
            {
                Debug.WriteLine("");
                Debug.WriteLine($"Category ID: {item.CategoryId} Name: {item.Name}");
                Debug.WriteLine($"----------------------------------------------");
                foreach (var productItem in item.Products)
                    Debug.WriteLine($"ID: {productItem.ProductId} Product Name: {productItem.Name} Price: {productItem.ListPrice}");

            }

            //------------Join Objects-------------------------
            var products13 = from items in product.GetProducts()
                             join category in categories.GetProductCategories()
                             on items.ProductCategoryID equals
                                category.ProductCateogryId
                             select
                                new
                                {
                                    Category = category.Name,
                                    ProductName = items.Name
                                };

            foreach (var item in products13)
                Debug.WriteLine($"Product Name: {item.ProductName} Category: {item.Category}");




        }
        //recent 10 days
        private static bool Recent(DateTime datetime)
        {
            TimeSpan dateDiff = DateTime.Now - datetime;
            return Math.Abs(dateDiff.Days) <= 10;
        }
        private static int DayDiffInDays(DateTime datetime)
        {
            TimeSpan dateDiff = DateTime.Now - datetime;
            return Math.Abs(dateDiff.Days);
        }
    }
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal ListPrice { get; set; }
        public DateTime SaleStartDate { get; set; }
        public int ProductCategoryID { get; set; }
        public List<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product()
                {
                    Name = "wadget",
                    ProductId = 1,
                    ListPrice = 7.5m,
                    ProductCategoryID = 2,
                    SaleStartDate = new DateTime(2009,1,1)
                },
                new Product()
                {
                    Name = "widget",
                    ProductId = 2,
                    ListPrice = 13.29m,
                    ProductCategoryID = 2,
                    SaleStartDate = new DateTime(2009,1,1)
                },
                new Product()
                {
                    Name = "wodget",
                    ProductId = 3,
                    ListPrice = 10.23m,
                    ProductCategoryID = 4,
                    SaleStartDate = new DateTime(2009,1,1)
                },
                                new Product()
                {
                    Name = "wodget",
                    ProductId = 4,
                    ListPrice = 112.23m,
                    ProductCategoryID = 4,
                    SaleStartDate = new DateTime(2009,1,1)
                },
            };
        }
    }
    public class ProductCategory
    {
        public int ProductCateogryId { get; set; }
        public string Name { get; set; }
        public List<ProductCategory> GetProductCategories()
        {
            return new List<ProductCategory>
            {
                new ProductCategory()
                {
                    Name = "Bikes",
                    ProductCateogryId = 1
                },
                new ProductCategory()
                {
                    Name = "components",
                    ProductCateogryId = 2
                },
                new ProductCategory()
                {
                    Name = "Accessories",
                    ProductCateogryId = 4
                }
            };
        }
    }
    public class ProductInfo
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
    }
}

