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

namespace clientDB
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Logger.Instance.Log("ПРОГРАММА ЗАПУЩЕНА");
            InitializeComponent();
            try
            {
                Logger.Instance.Log("Совершен переход на страницу AuthorizationPage");
                frameMain.Navigate(new AuthorizationPage());
            }
            catch (Exception)
            {
                Logger.Instance.Log("Переход на страницу AuthorizationPage завершился с ошибкой");
                throw;
            }
        }

        private void buttonChangeUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Logger.Instance.Log("Совершен переход на страницу AuthorizationPage");
                frameMain.Navigate(new AuthorizationPage());
            }
            catch (Exception)
            {
                Logger.Instance.Log("Переход на страницу AuthorizationPage завершился с ошибкой");
                throw;
            }
        }
    }
}
