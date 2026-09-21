using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace ShoeStore_Porsev
{
    /// <summary>
    /// Логика взаимодействия для MainMenuWindow.xaml
    /// </summary>
    public partial class MainMenuWindow : Window
    {
        private int PRDID = 0;
        int sortMode = -1;

        public MainMenuWindow(string FIOAuthClient, int roleClient)
        {
            InitializeComponent();
            FIOTB.Text = FIOAuthClient;
            if (roleClient == 1)
            {
                SearchLB.Visibility = Visibility.Visible;
                SearchTB.Visibility = Visibility.Visible;
                GrpBx.Visibility = Visibility.Visible;
                PrdCB.Visibility = Visibility.Visible;
                PrdLB.Visibility = Visibility.Visible;
                RstBt.Visibility = Visibility.Visible;
            }
            if (roleClient == 2)
            {
                SearchLB.Visibility = Visibility.Visible;
                SearchTB.Visibility = Visibility.Visible;
                GrpBx.Visibility = Visibility.Visible;
                PrdCB.Visibility = Visibility.Visible;
                PrdLB.Visibility = Visibility.Visible;
                RstBt.Visibility = Visibility.Visible;
            }
            if (roleClient == 3)
            {
                SearchLB.Visibility = Visibility.Visible;
                SearchTB.Visibility = Visibility.Visible;
            }

            ListTovar.ItemsSource = ShoeShopEntities.GetContext().Product.ToList();
            var providerList = ShoeShopEntities.GetContext().Provider.ToList();
            providerList.Insert(0, new Provider { id_Provider = 0, ProviderName = "Все поставщики"});
            PrdCB.ItemsSource = providerList;
            PrdCB.SelectedIndex = 0;
        }
        private void Filter()
        {
            var SearchList = ShoeShopEntities.GetContext().Product.ToList();
            if (SearchTB.Text != "")
            {
                SearchList = SearchList.Where(x => (x.ProductName.ProductName1.ToLower().Contains(SearchTB.Text.ToLower())) ||
                    (x.Provider.ProviderName.ToLower().Contains(SearchTB.Text.ToLower())) ||
                    (x.Manufacturer.ManufacturerName.ToLower().Contains(SearchTB.Text.ToLower())) || 
                    (x.Category.CategoryName.ToLower().Contains(SearchTB.Text.ToLower())) ||
                    (x.Description.ToLower().Contains(SearchTB.Text.ToLower()))).ToList();
            }
            if (PRDID > 0)
            {
                SearchList = SearchList.Where(x => x.id_Provider == PRDID).ToList();
            }
            if (sortMode == 0)
            {
                SearchList = SearchList.OrderBy(x => x.Stock).ToList();
            }
            if (sortMode == 1)
            {
                SearchList = SearchList.OrderByDescending(x => x.Stock).ToList();
            }
            ListTovar.ItemsSource = SearchList.ToList();
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void PrdCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PrdCB.SelectedValue != null)
            {
                PRDID = (int)PrdCB.SelectedValue;
            }
            Filter();
        }

        private void RstBt_Click(object sender, RoutedEventArgs e)
        {
            PrdCB.SelectedIndex = 0;
            SearchTB.Text = "";
            sortMode = -1;
            ListTovar.ItemsSource = ShoeShopEntities.GetContext().Product.ToList();
        }

        private void OrderByBtn_Click(object sender, RoutedEventArgs e)
        {
            sortMode = 0;
            Filter();
        }

        private void OrderByDesBtn_Click(object sender, RoutedEventArgs e)
        {
            sortMode = 1;
            Filter();
        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow authWindow = new AuthorizationWindow();
            authWindow.Show();
            this.Close();
        }
    }
    
}
