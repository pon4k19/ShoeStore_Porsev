using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.IO;

namespace ShoeStore_Porsev
{
    /// <summary>
    /// Логика взаимодействия для AddEditProductWindow.xaml
    /// </summary>
    public partial class AddEditProductWindow : Window
    {
        private Product _curProduct;
        public AddEditProductWindow(Product product)
        {
            InitializeComponent();
            if (product == null)
            {
                _curProduct = new Product();
                _curProduct.Photo = null;
                _curProduct.Photo = System.IO.Path.Combine(@"C:\Users\student\Documents\лабы ДЭ обувь\ботинки\", "picture.png");

                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var rnd = new Random();
                _curProduct.Article = new string(Enumerable.Repeat(chars, 5).Select(s => s[rnd.Next(s.Length)]).ToArray());
            }
            else
            {
                _curProduct = product;
            }
            DataContext = _curProduct;
            if (_curProduct.id_Product == 0)
            {
                IdLabel.Visibility = Visibility.Collapsed;
                IdTextBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                IdLabel.Visibility = Visibility.Visible;
                IdTextBox.Visibility = Visibility.Visible;
                IdTextBox.IsReadOnly = true;
            }
            CategoryCB.ItemsSource = ShoeShopEntities.GetContext().Category.ToList();
            ManufacturerCB.ItemsSource = ShoeShopEntities.GetContext().Manufacturer.ToList();
            ProviderCB.ItemsSource = ShoeShopEntities.GetContext().Provider.ToList();
            ProductNameCB.ItemsSource = ShoeShopEntities.GetContext().ProductName.ToList();

        }

        private void SelectImageBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string targetDirectory = @"C:\Users\student\Documents\лабы ДЭ обувь\ботинки\";

                OpenFileDialog dialog = new OpenFileDialog();
                dialog.DefaultExt = ".png";
                dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                if (dialog.ShowDialog() == true)
                {
                    string fileName = dialog.SafeFileName;
                    string newPath = System.IO.Path.Combine(targetDirectory, fileName);

                    var source = new BitmapImage();
                    source.BeginInit();
                    source.CacheOption = BitmapCacheOption.OnLoad;
                    source.UriSource = new Uri(dialog.FileName);
                    source.EndInit();

                    var resized = new TransformedBitmap(source,
                        new ScaleTransform(300.0 / source.PixelWidth, 200.0 / source.PixelHeight));
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(resized));
                    using (var fs = new System.IO.FileStream(newPath, System.IO.FileMode.Create))
                    {
                        encoder.Save(fs);
                    }
                    if (!string.IsNullOrWhiteSpace(_curProduct.Photo)
    && _curProduct.Photo != newPath
    && System.IO.File.Exists(_curProduct.Photo)
    && !_curProduct.Photo.EndsWith("picture.png", StringComparison.OrdinalIgnoreCase))
                    {
                        try { System.IO.File.Delete(_curProduct.Photo); } catch { }
                    }
                    _curProduct.Photo = newPath;
                    var preview = new BitmapImage();
                    preview.BeginInit();
                    preview.CacheOption = BitmapCacheOption.OnLoad;
                    preview.UriSource = new Uri(newPath);
                    preview.EndInit();
                    PhotoImg.Source = preview;
                    
                    FileNameTB.Text = newPath;
                    MessageBox.Show("Изображение товара успешно установлено!");
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка работы с изображением: {ex.Message}",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_curProduct.Unit))
            {
                MessageBox.Show("Укажите единицу измерения.");
                return;
            }

            if (_curProduct.Category == null)
            {
                MessageBox.Show("Выберите категорию.");
                return;
            }

            if (_curProduct.Manufacturer == null)
            {
                MessageBox.Show("Выберите производителя.");
                return;
            }

            if (_curProduct.Provider == null)
            {
                MessageBox.Show("Выберите поставщика.");
                return;
            }

            if (_curProduct.Price < 0)
            {
                MessageBox.Show("Цена не может быть отрицательной.");
                return;
            }

            try
            {
                if (_curProduct.id_Product == 0)
                {
                    _curProduct.Unit = "шт.";
                    ShoeShopEntities.GetContext().Product.Add(_curProduct);
                }

                ShoeShopEntities.GetContext().SaveChanges();
                MessageBox.Show("Данные сохранены.");

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка сохранения",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
