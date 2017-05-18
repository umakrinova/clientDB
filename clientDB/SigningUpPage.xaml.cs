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
            InitializeComponent();
            users = DeserializeData();
        }

        private string CalculateHash(string password)
        {
            MD5 md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
            return Convert.ToBase64String(hash);
        }

        private void buttonSignUp_Click(object sender, RoutedEventArgs e)
        {
            newUser = new User(textBoxLogin.Text, CalculateHash(passwordBox.Password));
            users.Add(newUser);
            SerializeData();
            NavigationService.Navigate(new AuthorizationPage());
        }

        private void SerializeData()
        {
            XmlSerializer xml = new XmlSerializer(typeof(List<User>));
            using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate))
            {
                xml.Serialize(fs, users);
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
            }
            catch (FileNotFoundException)
            {

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
