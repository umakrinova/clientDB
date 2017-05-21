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
                    ("Ошибка чтения из файла. Если файл "
                    + FileName + " существует, но в него не записаны данные о клиентах, удалите файл.");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            RefreshListBox();
            return data;
        }

        private void RefreshListBox()
        {
            listBoxClients.ItemsSource = null;
            listBoxClients.ItemsSource = data.Clients;
        }
    }
}
