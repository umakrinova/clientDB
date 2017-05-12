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
    /// Логика взаимодействия для SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        List<Client> clients = new List<Client>();
        public SearchWindow(List<Client> clients, List<Tariff> tariffs)
        {
            InitializeComponent();
            comboBoxTariffs.ItemsSource = tariffs;
            this.clients = clients;
        }
        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            listBoxFoundClients.Items.Clear();
            if ((!textBoxNumber.IsMaskFull) && (textBoxNumber.Text != "+7(___)-___-____"))
            {
                MessageBox.Show("Необходимо ввести полный номер или не вводить никакой.");
            }
            else
            {
                for (int i = 0; i < clients.Count; i++)
                {
                    if ((string.IsNullOrWhiteSpace(textBoxSurname.Text) || textBoxSurname.Text == clients[i].Surname) &&
                        (string.IsNullOrWhiteSpace(textBoxName.Text) || textBoxName.Text == clients[i].Name) &&
                        (string.IsNullOrWhiteSpace(textBoxPatronymic.Text) || textBoxPatronymic.Text == clients[i].Patronymic) &&
                        ((!textBoxNumber.IsMaskFull) || textBoxNumber.Text == clients[i].Number)&&
                        (comboBoxTariffs.SelectedItem==null || comboBoxTariffs.SelectedItem==clients[i].Tariff))
                    {
                        listBoxFoundClients.Items.Add(clients[i]);
                    }
                }   
            }
            textBoxSurname.Text = "";
            textBoxName.Text = "";
            textBoxPatronymic.Text = "";
            textBoxNumber.Text = "";
            comboBoxTariffs.SelectedItem = null;
        }
    }
}
