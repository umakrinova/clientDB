using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit;
using System.Xml.Serialization;

namespace clientDB
{
    public class Client
    {
        private string surname;

        public string Surname
        {
            get { return surname; }
            set { surname = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string patronymic;

        public string Patronymic
        {
            get { return patronymic; }
            set { patronymic = value; }
        }

        private string number;

        public string Number
        {
            get { return number; }
            set { number = value; }
        }

        public Client(string surname, string name, string patronymic, string number, Tariff tariff)
        {
            this.surname = surname;
            this.name = name;
            this.patronymic = patronymic;
            this.number = number;
            if (tariff != null)
            {
                tariffId = tariff.Id;
            }
            else
            {
                tariffId = 1;
            }
        }

        public Client() { }

        private Tariff tariff;

        [XmlIgnore]
        public Tariff Tariff
        {
            get { return tariff; }
            set { tariff = value; }
        }

        private int tariffId;

        public int TariffId
        {
            get { return tariffId; }
            set { tariffId = value; }
        }

        public string Info
        {
            get
            {
                try
                {
                    return $"{surname} {name} {patronymic}  {number}   {tariff.Name}   {tariff.MonthCost}";
                }
                catch (Exception)
                {
                MessageBox.Show
                    ("Ошибка формирования информационной строки о пользователе. Возможно, для него не были определены некоторые поля");
                Logger.Instance.Log("Ошибка формирования информационной строки о пользователе");
                return null;
            }
        }
        }
    }
}
