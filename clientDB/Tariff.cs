using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace clientDB
{
    public class Tariff
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private double monthCost;

        public double MonthCost
        {
            get { return monthCost; }
            set { monthCost = value; }
        }

        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public Tariff(int id)
        {
            this.id = id;
        }

        public Tariff(int id, string name, double monthCost)
        {
            this.id = id;
            this.name = name;
            this.monthCost = monthCost;
        }

        public Tariff() { }
    }
}
