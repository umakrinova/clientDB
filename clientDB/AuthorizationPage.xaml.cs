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
using System.Security.Cryptography;
using System.IO;
using System.Xml.Serialization;

namespace clientDB
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        const string FileName = "users.xml";
        List<User> users = new List<User>();
        public AuthorizationPage()
        {
            InitializeComponent();
            textBoxLogin.Text = "";
            passwordBox.Password = "";
            users = DeserializeData();
            textBoxLogin.Focus();
        }

        private string CalculateHash(string password)
        {
            MD5 md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
            return Convert.ToBase64String(hash);
        }

        private void buttonLogIn_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < users.Count; i++)
            {
                if (textBoxLogin.Text == users[i].Login && CalculateHash(passwordBox.Password) == users[i].Password)
                {
                    NavigationService.Navigate(new ClientsPage());
                    break;
                }
                else
                {
                    if (i == users.Count - 1)
                    {
                        MessageBox.Show("Неверный логин или пароль");
                        textBoxLogin.Text = "";
                        passwordBox.Password = "";
                    }
                }          
            }
        }

        private void buttonSignUp_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SigningUpPage());
        }

        private List<User> DeserializeData()
        {
            try
            {
                using (FileStream fs = new FileStream(FileName, FileMode.Open))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(List<User>));
                    users = (List<User>)xml.Deserialize(fs);
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Зарегестрированных пользователей нет.");
            }
            catch (Exception)
            {
                MessageBox.Show
                    ("Ошибка чтения из файла. Если файл "
                    + FileName + " существует, но в него не записаны данные о клиентах, удалите файл.");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            return users;
        }
    }
}
