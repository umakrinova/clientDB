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
            try
            {
                InitializeComponent();
                textBoxLogin.Text = "";
                passwordBox.Password = "";
                users = DeserializeData();
                textBoxLogin.Focus();
                Logger.Instance.Log("Страница AuthorizationPage открыта успешно");
            }
            catch (Exception)
            {
                Logger.Instance.Log("Открытие страницы AuthorizationPage завершилось с ошибкой");
            }
        }

        private string CalculateHash(string password)
        {
            try
            {
                MD5 md5 = MD5.Create();
                var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
            catch (Exception)
            {
                Logger.Instance.Log("Ошибка при вычислении хэш-функции");
                throw;
            }
        }

        private void buttonLogIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                for (int i = 0; i < users.Count; i++)
                {
                    if (textBoxLogin.Text == users[i].Login && CalculateHash(passwordBox.Password) == users[i].Password)
                    {
                        Logger.Instance.Log("Выполнен авторизованный вход");
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
                            Logger.Instance.Log("Не удалось совершить авторизацию: неверный логин или пароль");
                        }
                    }
                }
            }
            catch (Exception)
            {
                Logger.Instance.Log("Не удалось совершить авторизацию");
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
                Logger.Instance.Log("Данные о пользователях загружены из файла " + FileName);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Зарегестрированных пользователей нет.");
            }
            catch (Exception)
            {
                MessageBox.Show
                    ("Ошибка чтения из файла. Если файл "
                    + FileName + " существует, но в него не записаны данные о пользователях, удалите файл.");
                Logger.Instance.Log("Ошибка чтения из файла " + FileName);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            return users;
        }

        private void buttonReadOnly_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ReadOnlyClientsPage());
        }
    }
}
