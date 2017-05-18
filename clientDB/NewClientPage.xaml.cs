﻿using System;
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
    /// Логика взаимодействия для NewClientPage.xaml
    /// </summary>
    public partial class NewClientPage : Page
    {
        public NewClientPage(List<Tariff> tariffs)
        {
            InitializeComponent();
            comboBoxTariffs.ItemsSource = tariffs;
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

            //if (comboBoxTariffs.SelectedItem == null)
            //{
            //    MessageBox.Show("Необходимо выбрать тариф.", "Ошибка добавления");
            //    comboBoxTariffs.Focus();
            //    return;
            //}

            newClient = new Client(textBoxSurname.Text, textBoxName.Text, textBoxPatronymic.Text, textBoxNumber.Text);
            newClient.Tariff = comboBoxTariffs.SelectedItem as Tariff;
            NavigationService.Navigate(new ClientsPage(newClient));
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ClientsPage());
        }
    }
}