using BLL.Abstract;
using BLL.Model;
using ConsoleAppEntityFW.Abstract;
using ConsoleAppEntityFW.Concrete;
using ConsoleAppEntityFW.Entitys;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfApp1;
using System.Data.Entity;

namespace BLL.Provider
{
    public class ProductProvider : IProductProvider
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductImageRepository _productImageRepository;

        public ProductProvider()
        {
            EfContext context = new EfContext();
            _productRepository = new ProductRepository(context);
            _productImageRepository = new ProductImageRepository(context);
        }

        public Product AddProduct(ProductAddViewModel productAdd)
        {
            //MessageBox.Show(Environment.CurrentDirectory);
            Product product = new Product
            {
                Name = productAdd.Name,
                CategoryId = productAdd.CategoryId,
                Price = productAdd.Price,
                Quantity = productAdd.Quantity,
                DateCreate = DateTime.Now
            };
            _productRepository.Add(product);
            _productRepository.SaveChanges();
            if (productAdd.Images.Count > 0)
            {
                List<string> imgTemp = new List<string>();
                foreach (var p in productAdd.Images)
                {
                    string fSaveName = Guid.NewGuid().ToString() + ".jpg";
                    string fImages = Environment.CurrentDirectory + ConfigurationManager.AppSettings["ImageStore"].ToString();
                    string fNameOriginal = "o_" + fSaveName;
                    string fNameSmall = "s_" + fSaveName;
                    // Will not overwrite if the destination file already exists.
                    try
                    {
                        File.Copy(p.SourceOriginal, Path.Combine(fImages, fNameOriginal));
                        imgTemp.Add(fNameOriginal);

                        ProductImage image = new ProductImage // сохраняем в таблицу ProductImages  
                        {
                            Name = fSaveName, // сохраняем имя без приставки
                            ProductId = product.Id
                        };
                        _productImageRepository.Add(image);
                        // Catch exception if the file was already copied.

                        using (FileStream stream = new FileStream(Path.Combine(fImages, fNameSmall), FileMode.Create))
                        {
                            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                            //TextBlock myTextBlock = new TextBlock();
                            //myTextBlock.Text = "Codec Author is: " + encoder.CodecInfo.Author.ToString();
                            encoder.Frames.Add(p.ImagePhoto);
                            //MessageBox.Show(myPalette.Colors.Count.ToString());
                            encoder.Save(stream);
                        }
                    }
                    catch (IOException copyError)
                    {
                        MessageBox.Show(copyError.Message);
                        foreach (var item in imgTemp)
                        {
                            File.Delete(Path.Combine(fImages, item));
                        }
                        return null;
                    }
                }
                _productImageRepository.SaveChanges();
            }
            return product;
        }

        //public IList<ProductItemViewModel> GetAllProducts()
        //{
        //    var model = _productRepository.GetAll()
        //       .Include(c => c.Categories)
        //       .Select(p =>
        //       new ProductItemViewModel
        //       {
        //           Id = p.Id,
        //           Name = p.Name,
        //           Category = p.Categories.Name,
        //           Quantity = p.Quantity,
        //           ProductImages = p.ProductImages.Select(i => i.Name).ToList()
        //       }).ToList();
        //    return model;
        //}

        public IList<ProductItemViewModel> GetAllProducts()
        {
            var model = _productRepository.GetAll()
               .Include(c => c.Categories)
               .Select(p =>
               new ProductItemViewModel
               {
                   Id = p.Id,
                   Name = p.Name,
                   Category = p.Categories.Name,
                   Quantity = p.Quantity,
                   ProductImages = p.ProductImages.Select(i =>
                                                    new ProductImageViewModel
                                                    {
                                                        Id = i.Id,
                                                        Name = i.Name
                                                    }).ToList()
               }).ToList();
            return model;
        }

        //public IQueryable<Product> GetAllProductsByName(string name)
        //{
        //    var allProducts = _productRepository.GetAll().Where(n => n.Name.StartsWith(name));
        //    return allProducts;
        //}

        //public IList<ProductItemViewModel> FindProducts(string name)
        //{
        //    var model = _productRepository.GetAll()
        //        .Include(c => c.Categories)
        //        .Where(p => p.Name.StartsWith(name))
        //        .Select(p =>
        //        new ProductItemViewModel
        //        {
        //            Id = p.Id,
        //            Name = p.Name,
        //            Category = p.Categories.Name,
        //            Quantity = p.Quantity,
        //            ProductImages = p.ProductImages.Select(i => i.Name).ToList()
        //        }).ToList();
        //    return model;
        //}

        public IList<ProductItemViewModel> FindProducts(string name, int quantity=2)
        {
            var model = _productRepository.GetAll()
                .Include(c => c.Categories)
                .Where(p => p.Name.StartsWith(name))
                .Select(p =>
                new ProductItemViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = p.Categories.Name,
                    Quantity = p.Quantity,
                    Price = p.Price,
                    ProductImages = p.ProductImages.Select(i =>
                   new ProductImageViewModel
                                                  {
                                                        Id = i.Id,
                                                        Name = i.Name
                                                    }).ToList()
                }).ToList();
            return model;
        }
        public int CountFindProducts(string name)
        {
            var countProduct = _productRepository.GetAll()
                .Include(c => c.Categories)
                .Where(p => p.Name.StartsWith(name))
                .Select(p =>
                new ProductItemViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = p.Categories.Name,
                    Quantity = p.Quantity,
                    Price = p.Price,
                    ProductImages = p.ProductImages.Select(i =>
                                                    new ProductImageViewModel
                                                    {
                                                        Id = i.Id,
                                                        Name = i.Name
                                                    }).ToList()
                }).ToList();
            return countProduct;
        }
        public void RemoveProduct(int productId)
        {
            throw new NotImplementedException();
        }

     }
}
