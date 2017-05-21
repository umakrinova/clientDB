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
using System.Xml.Serialization;
using System.IO;

namespace clientDB
{
    /// <summary>
    /// Логика взаимодействия для NewClientPage.xaml
    /// </summary>
    public partial class NewClientPage : Page
    {
        const string FileName = "clients.xml";
        ProgramData data;
        public NewClientPage(List<Tariff> tariffs, ProgramData data)
        {
            try
            {
                InitializeComponent();
                comboBoxTariffs.ItemsSource = tariffs;
                this.data = data;
                Logger.Instance.Log("Страница NewClientPage открыта успешно");
            }
            catch (Exception)
            {
                Logger.Instance.Log("Открытие страницы NewClientPage завершилось с ошибкой");
            }
        }

        Client newClient;

        public Client NewClient
        {
            get { return newClient; }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSurname.Text))
            {
                MessageBox.Show("Необходимо ввести фамилию.", "Ошибка добавления");
                textBoxSurname.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                MessageBox.Show("Необходимо ввести имя.", "Ошибка добавления");
                textBoxName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxPatronymic.Text))
            {
                MessageBox.Show("Необходимо ввести отчество.", "Ошибка добавления");
                textBoxPatronymic.Focus();
                return;
            }

            if (!textBoxNumber.IsMaskFull)
            {
                MessageBox.Show("Необходимо ввести номер телефона.", "Ошибка добавления");
                textBoxNumber.Focus();
                return;
            }

            if (comboBoxTariffs.SelectedItem == null)
            {
                MessageBox.Show("Необходимо выбрать тариф.", "Ошибка добавления");
                comboBoxTariffs.Focus();
                return;
            }

            try
            {
                newClient = new Client(textBoxSurname.Text, textBoxName.Text, textBoxPatronymic.Text, textBoxNumber.Text, 
                    (Tariff)comboBoxTariffs.SelectedItem);
                data.Clients.Add(newClient);
                SerializeData();
                Logger.Instance.Log("Создание клиента прошло успешно");
            }
            catch (Exception)
            {
                Logger.Instance.Log("Ошибка при создании клиента");
            }
            try
            {
                Logger.Instance.Log("Совершен переход на страницу ClientsPage");
                NavigationService.Navigate(new ClientsPage());
            }
            catch (Exception)
            {
                Logger.Instance.Log("Переход на страницу ClientsPage завершился с ошибкой");
            }
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Logger.Instance.Log("Совершен переход на страницу ClientsPage");
                NavigationService.Navigate(new ClientsPage());
            }
            catch (Exception)
            {
                Logger.Instance.Log("Переход на страницу ClientsPage завершился с ошибкой");
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
    }
}
