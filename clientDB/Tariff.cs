using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Tariff(string name, double monthCost)
        {
            this.name = name;
            this.monthCost = monthCost;
        }
    }
}
