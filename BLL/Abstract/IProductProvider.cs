using BLL.Model;
using ConsoleAppEntityFW.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1;

namespace BLL.Abstract
{
    public interface IProductProvider
    {
        Product AddProduct(ProductAddViewModel productAdd);

        IList<ProductItemViewModel> GetAllProducts();

        //IQueryable<Product> GetAllProductsByName(string name);

        IList<ProductItemViewModel> FindProducts(string name, int quantity=2);
        void RemoveProduct(int productId);

        int CountFindProducts(string name);
    }
}
