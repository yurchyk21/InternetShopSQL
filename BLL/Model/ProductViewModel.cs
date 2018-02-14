using ConsoleAppEntityFW.Entitys;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WpfApp1;

namespace BLL.Model
{
    public class ProductAddViewModel
    {
        public string Name { get; set; }

        public int CategoryId { get; set; }

        public float Price { get; set; }

        public int Quantity { get; set; }

        //List<ProductAddImageViewModel> ImageSave { get; set; }
        public virtual PhotoCollection Images { get; set; }
    }

    public class ProductItemViewModel
    {
        private string _path = Environment.CurrentDirectory + ConfigurationManager.AppSettings["ImageStore"].ToString();
        public int Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public float Price { get; set; }

        public int Quantity { get; set; }

        public DateTime DateCreate { get; set; }

        //public List<string> ProductImages { get; set; }

        public List<ProductImageViewModel> ProductImages { get; set; }

        public string FirstImageSmall
        {
            get
            {
                string imageName = "s_default.jpg";
                var image = ProductImages.FirstOrDefault();
                if (image != null)
                    return image.GetImageSmall;
                else
                    return _path + imageName;
            }
        }

        public string FirstImageOriginal
        {
            get
            {
                string imageName = "o_default.jpg";
                var image = ProductImages.FirstOrDefault();
                if (image != null)
                    return image.GetImageOriginal;
                else
                    return _path + imageName;
            }
        }
    }


    public class ProductImageViewModel
    {
        private string _path = Environment.CurrentDirectory + ConfigurationManager.AppSettings["ImageStore"].ToString();
        public int Id { get; set; }

        public string Name { get; set; }

        public string GetImageSmall
        {
            get
            {
                return _path + "s_" + Name;
            }
        }

        public string GetImageOriginal
        {
            get
            {
                return _path + "o_" + Name;
            }
        }
    }

}
