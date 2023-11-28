using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RaffleApp
{
    public class User
    {
        private string _name { get; set; }
        private int _nooftickets { get; set; }  //Total Tickets purchased
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public int NoOfTickets
        {
            get => _nooftickets;
            set => _nooftickets = value;
        }
        public User(string name, int tickets)
        {
            Name = name;
            NoOfTickets = tickets;
        }


    }
}
