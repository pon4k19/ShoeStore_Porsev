using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ShoeStore_Porsev
{
    /// <summary>
    /// Логика взаимодействия для OrdersWindow.xaml
    /// </summary>
    public partial class OrdersWindow : Window
    {
        private int _roleClient;
        private int _idUser;

        public OrdersWindow(int roleClient, int idUser)
        {
            _roleClient = roleClient;
            _idUser = idUser;
            InitializeComponent();

            UpdateOrders();

            if (_roleClient == 1)
            {
                AddBt.Visibility = Visibility.Visible;
                DeleteBt.Visibility = Visibility.Visible;
            }
            else
            {
                AddBt.Visibility = Visibility.Collapsed;
                DeleteBt.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdateOrders()
        {
            OrderTovar.ItemsSource = ShoeShopEntities.GetContext().Order.ToList();
        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            var user = ShoeShopEntities.GetContext().User.FirstOrDefault(u => u.id_User == _idUser);
            new MainMenuWindow(user.FullName, _roleClient, _idUser).Show();
            this.Close();
        }

        private void AddBt_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddEditOrderWindow(null, _idUser);
            if (window.ShowDialog() == true)
                UpdateOrders();
        }

        private void DeleteBt_Click(object sender, RoutedEventArgs e)
        {
            var selected = OrderTovar.SelectedItem as Order;
            if (selected == null)
            {
                MessageBox.Show("Выберите заказ для удаления.");
                return;
            }

            if (MessageBox.Show($"Удалить заказ {selected.OrderNumber}?",
                                "Подтверждение", MessageBoxButton.YesNo,
                                MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            try
            {
                var orderProducts = ShoeShopEntities.GetContext().OrderProduct
                                                        .Where(x => x.id_Order == selected.id_Order)
                                                        .ToList();
                ShoeShopEntities.GetContext().OrderProduct.RemoveRange(orderProducts);
                ShoeShopEntities.GetContext().Order.Remove(selected);
                ShoeShopEntities.GetContext().SaveChanges();
                MessageBox.Show("Заказ удалён.");

                UpdateOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка удаления",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OrderTovar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selected = OrderTovar.SelectedItem as Order;
            if (selected == null) return;

            var window = new AddEditOrderWindow(selected, _idUser);
            if (window.ShowDialog() == true)
                UpdateOrders();
        }
    }
}