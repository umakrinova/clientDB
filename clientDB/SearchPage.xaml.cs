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
using System.Text.RegularExpressions;

namespace clientDB
{
    /// <summary>
    /// Логика взаимодействия для SearchPage.xaml
    /// </summary>
    public partial class SearchPage : Page
    {
        List<Client> clients = new List<Client>();
        List<Tariff> tariffs = new List<Tariff>();
        Regex regexSurname, regexName, regexPatronymic;

        public SearchPage(List<Client> clients, List<Tariff> tariffs)
        {
            try
            {
                InitializeComponent();
                this.tariffs = tariffs;
                comboBoxTariffs.ItemsSource = tariffs;
                this.clients = clients;
                textBoxSurname.Focus();
                Logger.Instance.Log("Страница SearchPage открыта успешно");
            }
            catch (Exception)
            {
                Logger.Instance.Log("Открытие страницы SearchPage завершилось с ошибкой");
            }
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxFoundClients != null)
            {
                listBoxFoundClients.Items.Clear();
            }
                try
                {
                    for (int i = 0; i < clients.Count; i++)
                    {
                        if ((string.IsNullOrWhiteSpace(textBoxSurname.Text) || regexSurname.IsMatch(clients[i].Surname)) &&
                            (string.IsNullOrWhiteSpace(textBoxName.Text) || regexName.IsMatch(clients[i].Name)) &&
                            (string.IsNullOrWhiteSpace(textBoxPatronymic.Text) || regexPatronymic.IsMatch(clients[i].Patronymic)) &&
                            ((!textBoxNumber.IsMaskFull) || textBoxNumber.Text == clients[i].Number) &&
                            (comboBoxTariffs.Text == "" || comboBoxTariffs.Text == clients[i].Tariff.Name))
                        {
                            listBoxFoundClients.Items.Add(clients[i]);
                        }
                    }
                    Logger.Instance.Log("Поиск клиентов выполнен успешно");
                }
                catch (Exception)
                {
                    Logger.Instance.Log("Произошла ошибка при поиске клиентов");
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

        private void textBoxSurname_TextChanged(object sender, TextChangedEventArgs e)
        {
            regexSurname = new Regex("^" + textBoxSurname.Text);
            buttonSearch_Click(this, e);
        }

        private void textBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            regexName = new Regex("^" + textBoxName.Text);
            buttonSearch_Click(this, e);
        }

        private void textBoxPatronymic_TextChanged(object sender, TextChangedEventArgs e)
        {
            regexPatronymic = new Regex("^" + textBoxPatronymic.Text);
            buttonSearch_Click(this, e);
        }

        private void textBoxNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            buttonSearch_Click(this, e);
        }

        private void comboBoxTariffs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxTariffs.SelectedIndex != -1) comboBoxTariffs.Text = tariffs[comboBoxTariffs.SelectedIndex].Name;
            else comboBoxTariffs.Text = "";
            buttonSearch_Click(this, e);
        }
    }
}
