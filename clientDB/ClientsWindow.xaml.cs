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

namespace clientDB
{
    /// <summary>
    /// Логика взаимодействия для ClientsWindow.xaml
    /// </summary>
    public partial class ClientsWindow : Window
    {
        const string FileName = "clients.txt";
        List<Client> clients = new List<Client>();
        List<Tariff> tariffs = new List<Tariff>();

        public ClientsWindow()
        {
            InitializeComponent();
            LoadData();
        }
        private void RefreshListBox()
        {
            listBoxClients.ItemsSource = null;
            listBoxClients.ItemsSource = clients;
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            var window = new NewClientWindow(tariffs);
            if (window.ShowDialog().Value)
            {
                clients.Add(window.NewClient);
                SaveData();
                RefreshListBox();
            }
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
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            var window = new SearchWindow();
            if (window.ShowDialog().Value)
            {
                RefreshListBox(); //what for??
            }
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
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
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
                }
            }
            catch(FileNotFoundException)
            {
                tariffs.Add(new Tariff("Базовый", 300));
            }
            catch
            {
                MessageBox.Show("Ошибка чтения из файла");
            }
            RefreshListBox();
        }
    }
}
