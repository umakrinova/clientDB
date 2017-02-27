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

namespace clientDB
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        List<Client> clients = new List<Client>();

        bool surnameEntered = false;
        private void textBoxSurname_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!surnameEntered)
            {
                textBoxSurname.Text = "";
                textBoxSurname.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void textBoxSurname_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxSurname.Text))
                surnameEntered = true;
            else
            {
                textBoxSurname.Text = "Иванов";
                surnameEntered = false;
                textBoxSurname.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        bool nameEntered = false;
        private void textBoxName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!nameEntered)
            {
                textBoxName.Text = "";
                textBoxName.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void textBoxName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxName.Text))
                nameEntered = true;
            else
            {
                textBoxName.Text = "Иван";
                nameEntered = false;
                textBoxName.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        bool patronymicEntered = false;
        private void textBoxPatronymic_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!patronymicEntered)
            {
                textBoxPatronymic.Text = "";
                textBoxPatronymic.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void textBoxPatronymic_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxPatronymic.Text))
                 patronymicEntered = true;
            else
            {
                textBoxPatronymic.Text = "Иванович";
                patronymicEntered = false;
                textBoxPatronymic.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            var client = new Client(textBoxSurname.Text, textBoxName.Text, textBoxPatronymic.Text, textBoxNumber.Text);
            clients.Add(client);
            textBoxSurname.Text = "";
            textBoxName.Text = "";
            textBoxPatronymic.Text = "";
            textBoxNumber.Text = "";
        }
    }
}
