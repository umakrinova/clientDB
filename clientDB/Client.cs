using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit;

namespace clientDB
{
    class Client
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

        public Client(string surname, string name, string patronymic, string number)
        {
            this.surname = surname;
            this.name = name;
            this.patronymic = patronymic;
            this.number = number;
        }
    }
}
