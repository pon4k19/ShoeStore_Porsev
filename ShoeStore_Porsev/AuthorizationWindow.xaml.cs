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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShoeStore_Porsev
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var authClient = ShoeShopEntities.GetContext().User.FirstOrDefault(x => x.Login == LoginTB.Text && x.Password == PasswordTB.Password);
            string FIOAuthClient = authClient.FullName;
            try
            {
                if (authClient.id_Role == 1)
                {
                    MainMenuWindow mainMenu = new MainMenuWindow(FIOAuthClient, 1);
                    mainMenu.Show();
                    this.Close();
                }
                if (authClient.id_Role == 2)
                {
                    MainMenuWindow mainMenu = new MainMenuWindow(FIOAuthClient, 2);
                    mainMenu.Show();
                    this.Close();
                }
                if (authClient.id_Role == 3)
                {
                    MainMenuWindow mainMenu = new MainMenuWindow(FIOAuthClient, 3);
                    mainMenu.Show();
                    this.Close();
                }                
            }
            catch (Exception ex)
            {

            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainMenuWindow mainMenu = new MainMenuWindow("Гость", 0);
            mainMenu.Show();
            this.Close();
        }
    }
}
