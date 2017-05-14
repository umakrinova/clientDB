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

namespace clientDB
{
    /// <summary>
    /// Логика взаимодействия для EditingPage.xaml
    /// </summary>
    public partial class EditingPage : Page
    {
        private Client client;
        int index;

        public EditingPage(Client client, int index, List<Tariff> tariffs)
        {
            InitializeComponent();
            comboBoxTariffs.ItemsSource = tariffs;
            textBoxSurname.Text = client.Surname;
            textBoxName.Text = client.Name;
            textBoxPatronymic.Text = client.Patronymic;
            textBoxNumber.Text = client.Number;
            comboBoxTariffs.Text = client.Tariff.Name;
            this.index = index;
        }
        private void buttonEdit_Click(object sender, RoutedEventArgs e)
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

            client = new Client(textBoxSurname.Text, textBoxName.Text, textBoxPatronymic.Text, textBoxNumber.Text);
            client.Tariff = comboBoxTariffs.SelectedItem as Tariff;
            NavigationService.Navigate(new ClientsPage(client, index));
        }
    }
}
