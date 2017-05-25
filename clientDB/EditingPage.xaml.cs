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
using System.Text.RegularExpressions;

namespace clientDB
{
    /// <summary>
    /// Логика взаимодействия для EditingPage.xaml
    /// </summary>
    public partial class EditingPage : Page
    {
        private Client client;
        int index;
        const string FileName = "clients.xml";
        Regex regex = new Regex("[а-яА-Я]");
        ProgramData data;

        public EditingPage(Client client, int index, ProgramData data)
        {
            try
            {
                InitializeComponent();
                this.data = data;
                comboBoxTariffs.ItemsSource = data.Tariffs;
                textBoxSurname.Text = client.Surname;
                textBoxName.Text = client.Name;
                textBoxPatronymic.Text = client.Patronymic;
                textBoxNumber.Text = client.Number;
                comboBoxTariffs.Text = client.Tariff.Name;
                this.index = index;
                textBoxSurname.Focus();
                Logger.Instance.Log("Страница EditingPage открыта успешно");
            }
            catch (Exception)
            {
                Logger.Instance.Log("Открытие страницы EditingPage завершилось с ошибкой");
            }
        }
        private void buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSurname.Text) || !regex.IsMatch(textBoxSurname.Text))
            {
                MessageBox.Show("Необходимо ввести фамилию по-русски.", "Ошибка добавления");
                textBoxSurname.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxName.Text) || !regex.IsMatch(textBoxName.Text))
            {
                MessageBox.Show("Необходимо ввести имя по-русски.", "Ошибка добавления");
                textBoxName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxPatronymic.Text) || !regex.IsMatch(textBoxPatronymic.Text))
            {
                MessageBox.Show("Необходимо ввести отчество по-русски.", "Ошибка добавления");
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
                client = new Client(textBoxSurname.Text, textBoxName.Text, textBoxPatronymic.Text, textBoxNumber.Text,
                    (Tariff)comboBoxTariffs.SelectedItem);
                data.Clients[index] = client;
                SerializeData();
                Logger.Instance.Log("Изменение полей клиента прошло успешно");
            }
            catch (Exception)
            {
                Logger.Instance.Log("Ошибка при изменении полей клиента");
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

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) buttonEdit_Click(this, e);
        }
    }
}
