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
        ProgramData data = new ProgramData();

        public ClientsPage()
        {
            try
            {
                InitializeComponent();
                data = DeserializeData();
                Logger.Instance.Log("Страница ClientsPage открыта успешно");
            }
            catch (Exception)
            {
                MessageBox.Show
                    ("Ошибка чтения из файла. Если файл "
                    + FileName + " существует, но в него не записаны данные о клиентах, удалите файл.");
                Logger.Instance.Log("Открытие страницы ClientsPage завершилось с ошибкой: произошла ошибка чтения из файла");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private void RefreshListBox()
        {
            try
            {
                listBoxClients.ItemsSource = null;
                listBoxClients.ItemsSource = data.Clients;
                Logger.Instance.Log("Список клиентов, отображающихся на ClientsPage, был обновлён");
            }
            catch (Exception)
            {
                Logger.Instance.Log("Обновление списка клиентов, отображающихся на ClientsPage, завершилось с ошибкой");
            }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Logger.Instance.Log("Совершен переход на страницу NewClientPage");
                NavigationService.Navigate(new NewClientPage(data.Tariffs, data));
            }
            catch (Exception)
            {
                Logger.Instance.Log("Переход на страницу NewClientPage завершился с ошибкой");
            }
        }

        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listBoxClients.SelectedIndex != -1)
                {
                    data.Clients.RemoveAt(listBoxClients.SelectedIndex);
                    RefreshListBox();
                    if (data.Clients.Count != 0) SerializeData();
                    else File.Delete(FileName);
                    Logger.Instance.Log("Клиент был удалён");
                }
            }
            catch (Exception)
            {
                Logger.Instance.Log("Удаление клиента завершилось с ошибкой");
            }
        }

        private void listBoxClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            buttonRemove.IsEnabled = listBoxClients.SelectedIndex != -1;
            buttonEdit.IsEnabled = listBoxClients.SelectedIndex != -1;
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Logger.Instance.Log("Совершен переход на страницу SearchPage");
                NavigationService.Navigate(new SearchPage(data.Clients, data.Tariffs));
            }
            catch (Exception)
            {
                Logger.Instance.Log("Переход на страницу SearchPage завершился с ошибкой");
            }    
        }

        private void buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listBoxClients.SelectedIndex != -1)
                {
                    Logger.Instance.Log("Совершен переход на страницу EditingPage");
                    NavigationService.Navigate(new EditingPage((Client)listBoxClients.SelectedItem,
                        listBoxClients.SelectedIndex, data));
                }
            }
            catch (Exception)
            {
                Logger.Instance.Log("Переход на страницу EditingPage завершился с ошибкой");
            }
        }

        private void SerializeData()
        {
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(ProgramData));
                using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate))
                {
                    xml.Serialize(fs, data);
                }
                Logger.Instance.Log("Данные записаны в файл " + FileName);
            }
            catch (Exception)
            {
                Logger.Instance.Log("Ошибка записи в файл " + FileName);
            }
        }

        private ProgramData DeserializeData()
        {
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(ProgramData));
                using (FileStream fs = new FileStream(FileName, FileMode.Open))
                {
                    data = (ProgramData)xml.Deserialize(fs);
                }

                    foreach (var client in data.Clients)
                    {
                        int i = 0;
                        while (i < data.Tariffs.Count && data.Tariffs[i].Id != client.TariffId)
                            i++;
                        if (i < data.Tariffs.Count)
                            client.Tariff = data.Tariffs[i];
                    }

                Logger.Instance.Log("Данные считаны из файла " + FileName);
            }
            catch (FileNotFoundException)
            {
                data.Tariffs = new List<Tariff>();
                data.Clients = new List<Client>();
                data.Tariffs.Add(new Tariff(1, "Базовый", 300));
                data.Tariffs.Add(new Tariff(2, "Продвинутый", 500));
                Logger.Instance.Log("Не найден файл с данными о клиентах. Созданы 2 тарифа по умолчанию");
            }
            catch (Exception)
            {
                MessageBox.Show
                    ("Ошибка чтения из файла. Если файл "
                    + FileName + " существует, но в него не записаны данные о клиентах, удалите файл.");
                Logger.Instance.Log("Считывание данных из файла " + FileName + " завершилось с ошибкой");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            RefreshListBox();
            return data;
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete) buttonRemove_Click(this, e);
        }
    }
}
