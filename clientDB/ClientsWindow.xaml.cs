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
    /// Логика взаимодействия для ClientsWindow.xaml
    /// </summary>
    public partial class ClientsWindow : Window
    {
        List<Client> clients = new List<Client>();
        public ClientsWindow()
        {
            InitializeComponent();
            clients.Add(new Client("Иванов", "Иван", "Иванович", "+7(123)-456-7890"));
            RefreshListBox();
        }
        private void RefreshListBox()
        {
            listBoxClients.ItemsSource = null;
            listBoxClients.ItemsSource = clients;
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            var window = new NewClientWindow();
            if (window.ShowDialog().Value)
            {
                clients.Add(window.NewClient);
                RefreshListBox();
            }
        }

        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxClients.SelectedIndex != -1)
            {
                clients.RemoveAt(listBoxClients.SelectedIndex);
                RefreshListBox();
            }
        }

        private void listBoxClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            buttonRemove.IsEnabled = listBoxClients.SelectedIndex != -1;
        }
    }
}
