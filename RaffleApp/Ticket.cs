using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RaffleApp
{
    public class Ticket
    {
        private string _name { get; set; }
        //ticket purchased
        private string _ticketPurchased { get; set; }
        //Store Group 0 to 5 as 0 to 5
        //Group 0  as 0 (0 matching number)
        //Group 1  as 1 (1 matching number)
        //Group 2  as 2 (2 matching number)
        //Group 3  as 3 (3 matching number)
        //Group 4  as 4 (4 matching number)
        //Group 5  as 5 (5 matching number)
        private int _winninggroup { get; set; }    

        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public string TicketPurchased
        {
            get => _ticketPurchased;
            set => _ticketPurchased = value;
        }

        public int WinningGroup
        {
            get => _winninggroup;
            set => _winninggroup = value;
        }
        public Ticket(string name, string ticket)
        {
            Name = name;
            TicketPurchased = ticket;
            WinningGroup = 0; // Is udpated when run the Raffle
        }
    }
}
