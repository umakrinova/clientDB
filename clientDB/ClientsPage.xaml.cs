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
using System.IO;

namespace clientDB
{
    /// <summary>
    /// Логика взаимодействия для ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {
        const string FileName = "clients.txt";
        List<Client> clients = new List<Client>();
        List<Tariff> tariffs = new List<Tariff>();

        public ClientsPage()
        {
            InitializeComponent();
            LoadData();
        }

        public ClientsPage(Client newClient)
        {
            InitializeComponent();
            LoadData();
            clients.Add(newClient);
            SaveData();
            RefreshListBox();
        }

        public ClientsPage(Client client, int index)
        {
            InitializeComponent();
            LoadData();
            clients[index] = client;
            SaveData();
            RefreshListBox();
        }

        private void RefreshListBox()
        {
            listBoxClients.ItemsSource = null;
            listBoxClients.ItemsSource = clients;
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NewClientPage(tariffs));
        }

        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxClients.SelectedIndex != -1)
            {
                clients.RemoveAt(listBoxClients.SelectedIndex);
                SaveData();
                RefreshListBox();
            }
        }

        private void listBoxClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            buttonRemove.IsEnabled = listBoxClients.SelectedIndex != -1;
            buttonEdit.IsEnabled = listBoxClients.SelectedIndex != -1;
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SearchPage(clients, tariffs));    
        }

        private void SaveData()
        {
            using (var sw = new StreamWriter(FileName))
            {
                foreach (var client in clients)
                {
                    sw.WriteLine($"{client.Surname}:{client.Name}:{client.Patronymic}:{client.Number}:{client.Tariff.Name}:{ client.Tariff.MonthCost}");
                }
            }
        }

        private void LoadData()
        {
            try
            {
                clients = new List<Client>();
                tariffs = new List<Tariff>();
                using (var sr = new StreamReader(FileName))
                {
                    string line="";
                    while (!sr.EndOfStream)
                    {
                        line = sr.ReadLine();
                        var parts = line.Split(':');
                        if (parts.Length == 6)
                        {
                            int i = 0;
                            while (i < tariffs.Count && tariffs[i].Name != parts[4])
                                i++;
                            Tariff t;
                            if (i < tariffs.Count)
                                t = tariffs[i];
                            else
                            {
                                t = new Tariff(parts[4], double.Parse(parts[5]));
                                tariffs.Add(t);
                            }

                            var client = new Client(parts[0], parts[1], parts[2], parts[3]);
                            client.Tariff = t;
                            clients.Add(client);
                        }
                    }
                    if (line == "")
                    {
                        tariffs.Add(new Tariff("Базовый", 300));
                        tariffs.Add(new Tariff("Продвинутый", 500));
                    }
                }
            }
            catch (FileNotFoundException)
            {
                tariffs.Add(new Tariff("Базовый", 300));
                tariffs.Add(new Tariff("Продвинутый", 500));
            }
            catch
            {
                MessageBox.Show("Ошибка чтения из файла");
            }
            RefreshListBox();
        }

        private void buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxClients.SelectedIndex != -1)
            {
                NavigationService.Navigate(new EditingPage((Client) listBoxClients.SelectedItem, 
                    listBoxClients.SelectedIndex, tariffs));               
            }
        }
    }
}
