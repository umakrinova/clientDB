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
using System.Xml.Serialization;

namespace clientDB
{
    /// <summary>
    /// Логика взаимодействия для ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {
        const string FileName = "clients.xml";
        ProgramData data;

        public ClientsPage()
        {
            InitializeComponent();
            try
            {
                data = DeserializeData();
            }
            catch
            {
                MessageBox.Show
                    ("Ошибка чтения из файла. Если файл "
                    + FileName + " существует, но в него не записаны данные о клиентах, удалите файл.");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        public ClientsPage(Client newClient)
        {
            InitializeComponent();
            data = DeserializeData();
            data.Clients.Add(newClient);
            SaveData();
            RefreshListBox();
        }

        public ClientsPage(Client client, int index)
        {
            InitializeComponent();
            data = DeserializeData();
            data.Clients[index] = client;
            SaveData();
            RefreshListBox();
        }

        private void RefreshListBox()
        {
            listBoxClients.ItemsSource = null;
            listBoxClients.ItemsSource = data.Clients;
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NewClientPage(data.Tariffs));
        }

        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxClients.SelectedIndex != -1)
            {
                data.Clients.RemoveAt(listBoxClients.SelectedIndex);
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
            NavigationService.Navigate(new SearchPage(data.Clients, data.Tariffs));    
        }

        private void buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxClients.SelectedIndex != -1)
            {
                NavigationService.Navigate(new EditingPage((Client) listBoxClients.SelectedItem, 
                    listBoxClients.SelectedIndex, data.Tariffs));               
            }
        }

        private void SaveData()
        {
            XmlSerializer xml = new XmlSerializer(typeof(ProgramData));
            using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate))
            {
                xml.Serialize(fs, data);
            }
        }

        private ProgramData DeserializeData()
        {
            try
            {
                using (FileStream fs = new FileStream(FileName, FileMode.Open))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(ProgramData));
                    data = (ProgramData)xml.Deserialize(fs);
                }
            }
            catch (FileNotFoundException)
            {
                data = new ProgramData();
                data.Tariffs = new List<Tariff>();
                data.Clients = new List<Client>();
                data.Tariffs.Add(new Tariff("Базовый", 300));
                data.Tariffs.Add(new Tariff("Продвинутый", 500));
            }
            catch (Exception)
            {
                MessageBox.Show
                    ("Ошибка чтения из файла. Если файл существует, но в него не записаны данные о клиентах, удалите файл.");
            }

            SaveData();
            RefreshListBox();
            return data;
        }
    }
}
