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
    /// Логика взаимодействия для ReadOnlyClientsPage.xaml
    /// </summary>
    public partial class ReadOnlyClientsPage : Page
    {
        const string FileName = "clients.xml";
        ProgramData data;

        public ReadOnlyClientsPage()
        {
            Logger.Instance.Log("Выполнен вход без авторизации");
            try
            {
                InitializeComponent();
                data = DeserializeData();
                Logger.Instance.Log("Страница ReadOnlyClientsPage открыта успешно");
            }
            catch
            {
                MessageBox.Show
                    ("Ошибка чтения из файла. Если файл "
                    + FileName + " существует, но в него не записаны данные о клиентах, удалите файл.");
                Logger.Instance.Log("Открытие страницы ReadOnlyClientsPage завершилось с ошибкой: произошла ошибка чтения из файла");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
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
                data = new ProgramData();
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

        private void RefreshListBox()
        {
            try
            {
                listBoxClients.ItemsSource = null;
                listBoxClients.ItemsSource = data.Clients;
                Logger.Instance.Log("Список клиентов, отображающихся на ReadOnlyClientsPage, был обновлён");
            }
            catch (Exception)
            {
                Logger.Instance.Log("Обновление списка клиентов, отображающихся на ReadOnlyClientsPage, завершилось с ошибкой");
            }
        }
    }
}
