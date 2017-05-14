using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clientDB
{
    public class ProgramData
    {
        private List<Client> clients;

        public List<Client> Clients
        {
            get { return clients; }
            set { clients = value; }
        }

        private List<Tariff> tariffs;

        public List<Tariff> Tariffs
        {
            get { return tariffs; }
            set { tariffs = value; }
        }
    }
}
