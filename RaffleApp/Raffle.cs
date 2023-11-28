using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RaffleApp
{
    public class Raffle
    {
        public enum Status { NOTSTARTED, ONGOING };
        private double _initialPotSize { get; set; }
        private Status _status { get; set; }
        private double _potSize { get; set; }
        private List<User> _users = new List<User>();
        private List<Ticket> _tickets = new List<Ticket>();
        private string _winningTicket = string.Empty;

        public Status  DrawStatus
        {
            get => _status;
            set => _status = value;
        }
        public double InitialPotSize
        {
            get => _initialPotSize;
            set => _initialPotSize = value;
        }
        public double PotSize
        {
            get => _potSize;
            set => _potSize = value;
        }
        public string WinningTicket
        {
            get => _winningTicket;
            set => _winningTicket = value;
        }

        public List<User> Users
        {
            get => _users;
            set => _users = value;

        }

        public List<Ticket> Tickets
        {
            get => _tickets;
            set => _tickets = value;

        }

        /*
         * Construction with inital Pot Size (100)
         * Winning ticket not generated when applicaton starts
         * status is NOTSTARTED  GAME not started
         * potSize = initial Pot Size when application starts
         */
        public Raffle(int initialPotSize)
        {
            InitialPotSize = initialPotSize;
            DrawStatus = (int)Status.NOTSTARTED;
            WinningTicket= "";
            PotSize = initialPotSize;
        }

        //public double GetPotPrize()
        //{
        //    return this.potPrize;
        //}

        /*
         * Update the Pot Size when purchased ticket
         * each ticket costs $5
         */
        public void UpdatePotPrize(int noofTickets)
        {
            PotSize += noofTickets * 5;
        }

        /*
          Update Pot Size  for new game with
          Initial Pot Size + Ramining Amount from previous game
         */
        public void UpdatePotPrize(double remainingPrize)
        {
            PotSize = remainingPrize + InitialPotSize;
        }

        /* Add user to the list of users who buy ticket
         * noOfTicketss - no of tickets purchased
        */
        public void AddNewUser(string name, int noOfTickets)
        {
            User user = Users.Find(x => x.Name == name);
            if (user != null)
            {
                user.NoOfTickets += noOfTickets;
            }
            else
            {
                var newUser = new User(name, noOfTickets);
                Users.Add(newUser);
            }
        }

        /*
         Add when existing user to purchase ticket
         noOfTicketss - no of tickets purchased
        */
        public void UpdateUserTicketCount(string name, int noOfTickets)
        {
            User user = Users.Find(x => x.Name == name);
            if (user != null)
            {
                user.NoOfTickets += noOfTickets;
            }
        }

        /*
        Add when existing user to purchase ticket
        noOfTicketss - no of tickets purchased
       */
        public int GetUserTicketCount(string name)
        {
            User user = Users.Find(x => x.Name == name);
            if (user != null)
            {
                return user.NoOfTickets;
            }
            return 0;
        }

        /*
         * Add ticket purchased to ticket list
         * ticket - the ticket generated
         */
        public void AddNewTicket(string name, string ticket)
        {
            Ticket newTicket = new Ticket(name, ticket);
            Tickets.Add(newTicket);

        }

        /*
         * Get the descriptin of the message to diplay in the menu
         */
        public string GetStatusDescription()
        {
            if (DrawStatus == Status.NOTSTARTED)
                return "Draw has not started";
            else
                return "Draw is ongoing. Raffle pot size is $" + PotSize;
        }



        /*
         * loop through all the tickets purchased  and 
         * check how many of the numbers exists in winning Ticket
         * and update that count as 0,1,2,3,4,5  as group
         * gruop 2 - ticket matches 2 numbers from winning ticket
         * 
         * Ticket list updated with calculated group
         * jeya   , 5 6 7 8 9 ,   2  -  Group 2
         * jitesh , 7 9 12 14 15, 3  -  Group 3
         */
        public void UpdateWinnersGroup(string winningTicket)
        {
            
            foreach (Ticket ticket in Tickets)
            {
                string[] t = ticket.TicketPurchased.Trim().Split(' ');
                string[] wt = winningTicket.Trim().Split(' ');

                int winCount = 0; //how many matching numbers found in winning tikcet
                foreach (string str in t)
                {
                    foreach (string wStr in wt)
                    {
                        if (str.Trim() == wStr.Trim())
                        {
                            winCount++;
                            break;
                        }
                    }

                }
                ticket.WinningGroup = winCount; //update winning group to no of mathcing
            }
        }

        /*
         * Print Winners from Group 2 to Group 5 
         * with name, number of winning tickets and prize under each group
         * winningPricePerc -Each group has specific percentage of amount as price from pot size
         * possibleGroups - To print all Groups even without any winning number as Nil. 
         * remainingPrize - deduct prizes from each group and carry forward to the next game
         * 
            Print the winners list--

            Group 2 Winners:
              jitesh with  1 winning ticket(s) -$25.00

            Group 3 Winners:
            Nil

            Group 4 Winners:
            Nil

            Group 5  Winners (Jackpot):
            Nil
         */
        public void printWinners()
        {
            double remainingPrize = PotSize;
            var possibleGroups = new List<int> { 1, 2, 3, 4, 5 };
            var winningPricePerc = new List<int> { 0, 10, 15, 25, 50 };

           
            var result = possibleGroups
                .GroupJoin(
                    Tickets,
                    possibleGroup => possibleGroup,
                    ticket => ticket.WinningGroup,
                    (possibleGroup, groupTickets) => new
                    {
                        Group = possibleGroup,
                        Users = groupTickets
                            .GroupBy(u => u.Name)
                            .Select(groupByName => new
                            {
                                Name = groupByName.Key,
                                Count = groupByName.Count()
                            }),
                        TotalCount = groupTickets.Count() //to calculate the prize for the group
                    })
                .Where(x => x.Group > 1) //print only from Group 2 to Group 5
                .OrderBy(x => x.Group);


            Console.WriteLine();

            foreach (var group in result)
            {
                if (group.Group < 5)
                    Console.WriteLine($"Group {group.Group} Winners:");
                else
                    Console.WriteLine($"Group {group.Group}  Winners (Jackpot):");

                //group prize = pot size * 10% if Group 2
                double groupPrize = PotSize * winningPricePerc[group.Group - 1] / 100;

                // eech winning ticket get  group prize / total winning ticekt in that group
                double userPrize = groupPrize / group.TotalCount;
                try
                {
                    foreach (var user in group.Users)
                    {
                        double prize = user.Count * userPrize;

                        Console.WriteLine($"  {user?.Name} with  {user?.Count} winning ticket(s) -$" + prize.ToString("0.00"));
                    }
                    if (group.Users.Count() < 1)
                    {
                        Console.WriteLine("Nil"); //if no winning tickets for that group
                    }
                    else
                    {
                        remainingPrize -= groupPrize; //deduct the pot prize 
                    }

                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Nil");
                }
            }
            //update the pot size with remaining amount for the new game
            //new game will start with initial pot size (100) + remaining amount of privious game
            UpdatePotPrize(remainingPrize);
            Console.WriteLine();
        }

    }
}
