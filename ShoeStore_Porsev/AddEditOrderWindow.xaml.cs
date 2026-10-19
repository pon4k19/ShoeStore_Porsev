using System;
using System.Linq;
using System.Windows;

namespace ShoeStore_Porsev
{
    public partial class AddEditOrderWindow : Window
    {
        private Order _curOrder;
        private int _idUser;

        public AddEditOrderWindow(Order order, int idUser)
        {
            InitializeComponent();
            _idUser = idUser;

            if (order == null)
            {
                _curOrder = new Order
                {
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(7),
                    id_User = _idUser,
                    PickupCode = new Random().Next(100, 999)
                };
                var orderList = ShoeShopEntities.GetContext().Order.ToList();
                int maxNumber = orderList.Any() ? orderList.Max(x => x.OrderNumber) : 0;
                _curOrder.OrderNumber = maxNumber + 1;
            }
            else
            {
                _curOrder = order;
            }

            DataContext = _curOrder;

            if (_curOrder.id_Order == 0)
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
            StatusCB.ItemsSource = ShoeShopEntities.GetContext().Status.ToList();
            PickupPointCB.ItemsSource = ShoeShopEntities.GetContext().PickupPoint.ToList();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_curOrder.OrderNumber <= 0)
            {
                MessageBox.Show("Укажите артикул заказа.");
                return;
            }
            if (_curOrder.Status == null)
            {
                MessageBox.Show("Выберите статус заказа.");
                return;
            }
            if (_curOrder.PickupPoint == null)
            {
                MessageBox.Show("Выберите адрес пункта выдачи.");
                return;
            }
            if (_curOrder.OrderDate == default(DateTime))
            {
                MessageBox.Show("Укажите дату заказа.");
                return;
            }
            if (_curOrder.DeliveryDate == default(DateTime))
            {
                MessageBox.Show("Укажите дату выдачи.");
                return;
            }
            if (_curOrder.DeliveryDate < _curOrder.OrderDate)
            {
                MessageBox.Show("Дата выдачи не может быть раньше даты заказа.");
                return;
            }

            try
            {
                if (_curOrder.id_Order == 0)
                {
                    ShoeShopEntities.GetContext().Order.Add(_curOrder);
                }

                ShoeShopEntities.GetContext().SaveChanges();
                MessageBox.Show("Данные сохранены.");

                DialogResult = true;
            }
            catch { }
        }
    }
}