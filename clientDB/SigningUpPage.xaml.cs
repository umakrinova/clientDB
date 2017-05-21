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
using System.Xml.Serialization;
using System.IO;

namespace clientDB
{
    /// <summary>
    /// Логика взаимодействия для SigningUpPage.xaml
    /// </summary>
    public partial class SigningUpPage : Page
    {
        User newUser;
        List<User> users = new List<User>();
        const string FileName = "users.xml";

        public SigningUpPage()
        {
            try
            {
                InitializeComponent();
                users = DeserializeData();
                Logger.Instance.Log("Страница SigningUpPage открыта успешно");
            }
            catch (Exception)
            {
                MessageBox.Show
                    ("Ошибка чтения из файла. Если файл "
                    + FileName + " существует, но в него не записаны данные о пользователях, удалите файл.");
                Logger.Instance.Log("Открытие страницы SigningUpPage завершилось с ошибкой: произошла ошибка чтения из файла");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
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

        private void buttonSignUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool correct = true;

                if (string.IsNullOrWhiteSpace(textBoxLogin.Text) || string.IsNullOrWhiteSpace(passwordBox.Password))
                {
                    correct = false;
                    MessageBox.Show("Необходимо ввести логин и пароль", "Ошибка создания нового пользователя");
                }

                foreach (var user in users)
                {
                    if (textBoxLogin.Text == user.Login)
                    {
                        correct = false;
                        MessageBox.Show("Данный логин уже используется", "Ошибка создания нового пользователя");
                        break;
                    }
                }

                if (correct == true)
                {
                    newUser = new User(textBoxLogin.Text, CalculateHash(passwordBox.Password));
                    users.Add(newUser);
                    SerializeData();
                    NavigationService.Navigate(new AuthorizationPage());
                }
            }
            catch (Exception)
            {
                Logger.Instance.Log("Не удалось создать нового пользователя");
                throw;
            }
        }

        private void SerializeData()
        {
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(List<User>));
                using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate))
                {
                    xml.Serialize(fs, users);
                }
                Logger.Instance.Log("Данные записаны в файл " + FileName);
            }
            catch (Exception)
            {
                Logger.Instance.Log("Ошибка записи в файл " + FileName);
            }
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
                Logger.Instance.Log("Данные считаны из файла " + FileName);
            }
            catch (FileNotFoundException)
            {

            }
            catch (Exception)
            {
                MessageBox.Show
                    ("Ошибка чтения из файла. Если файл "
                    + FileName + " существует, но в него не записаны данные о клиентах, удалите файл.");
                Logger.Instance.Log("Считывание данных из файла " + FileName + " завершилось с ошибкой");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            return users;
        }
    }
}
