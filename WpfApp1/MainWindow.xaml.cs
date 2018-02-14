﻿using BLL.Abstract;
using BLL.Model;
using BLL.Provider;
using ConsoleAppEntityFW.Entitys;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    
    public partial class MainWindow : Window
    {
        private readonly IProductProvider _productProvider;
        public BLL.Model.PhotoCollection Photos;
        public MainWindow()
        {
            InitializeComponent();
            _productProvider = new ProductProvider();
            //_productProvider.GetAllProducts();
            Photos = (BLL.Model.PhotoCollection)(this.Resources["Photos"] as ObjectDataProvider).Data;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WindowAdd wnd = new WindowAdd();

            if (wnd.ShowDialog() == true)
            {
                wnd = null;
               
            }
            //Directory.Delete(Environment.CurrentDirectory + ConfigurationManager.AppSettings["ImagePath"].ToString(), true);


        }

        private void btnAddShowFilter_Click(object sender, RoutedEventArgs e)
        {
            WindowWorkFilter dlg = new WindowWorkFilter();
            dlg.ShowDialog();
        }

        private void buttonAC_Click(object sender, RoutedEventArgs e)
        {
            WindowAutoComplete dlg = new WindowAutoComplete();
            dlg.ShowDialog();
        }

        private void tlACPeople_Populating(object sender, PopulatingEventArgs e)
        {
            string text = tlACPeople.Text;
            var count = _productProvider.CountFindProducts(text);
            var result = _productProvider.FindProducts(text);
            if (count>result.Count)
            {
                result.Add(new ProductItemViewModel
                {
                    Id = 0,
                    Name = "..."

                });
            }

            tlACPeople.ItemsSource = result;
            tlACPeople.PopulateComplete();

        }
        private void tlACPeople_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tlACPeople.SelectedItem != null)
            {
                var product = tlACPeople.SelectedItem as ProductItemViewModel;
                lblProductName.Content = product.Name;
                lblProductPrice.Content = product.Price;
                lblProductQty.Content = product.Quantity;
                if (Photos.Count > 0)
                {
                    Photos.Clear();
                }
                Photos.AddProductImagesSmall(product.ProductImages);
                ImageOfProduct.Source = new BitmapImage(new Uri(product.FirstImageOriginal));
            }
        }

        private void PhotosListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tlACPeople.SelectedItem != null)
            {
                if (PhotosListBox.SelectedIndex != -1)
                {
                    ImageOfProduct.Source = new BitmapImage(new Uri(((Photo)PhotosListBox.SelectedItem).SourceOriginal));
                }
            }
        }
    }
}
