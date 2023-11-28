
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Xml.Linq;

namespace RaffleApp
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            Raffle raffleApp = new Raffle(100);
            Program p = new Program();
            p.StartRaffle(raffleApp); 
           
        }
        public  void StartRaffle(Raffle raffleApp)
        {
            try
            {
              
               
                while (true)
                {

                    ShowMenu(raffleApp);
                    /*get input 1,2,3 from the main menu
                     * 
                    Welcome to My Raffle App
                    Status: Draw has not started
                    [1] Start a New Draw
                    [2] Buy Tickets
                    [3] Run Raffle
                    User can enter `1`, `2` or `3` to proceed
                    input:
                    */
                    char option = GetMenuInput();
                    

                    switch (option)
                    {
                        case '1':
                            StartNewDraw(raffleApp);
                            Console.ReadLine();
                            Console.WriteLine();
                            break;

                        case '2':
                            //show main menu if draw not started 
                            if(raffleApp.DrawStatus == Raffle.Status.NOTSTARTED)
                            {
                                Console.WriteLine("Draw not started!. Please Start New Draw before buy\r\n");
                                continue;
                            }
                            Console.WriteLine("Enter your name, no of tickets to purchase ex: jitesh,2 ");
                            (string name, int nooftickets) = GetBuyInput(raffleApp); //get input from user
                            List<string> tickets = GenerateTicket(nooftickets);
                            BuyTicket(raffleApp, name, nooftickets, tickets);
                            break;


                        case '3':
                            //show main menu if draw not started 
                            if (raffleApp.DrawStatus == Raffle.Status.NOTSTARTED)
                            {
                                Console.WriteLine("Draw not started!. Please Start New Draw & buy\r\n");
                                continue;
                            }
                            Console.WriteLine("Running Raffle..");
                            string winningTicket = GetWinningTicket();
                            Console.WriteLine("Winning Ticket is " + winningTicket);

                            RunRaffle(raffleApp, winningTicket);
                            break;

                        default:
                            Console.WriteLine("Invalid input. Please enter 1, 2, or 3.");
                            break;

                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error occured in the Application. Please restart the Application");
            }

        }

        /*
         * Clear Users , Tickets if any when new draw started
         * Change the draw status to ONGOING
         */
        public static void StartNewDraw(Raffle raffleApp)
        {
            Console.WriteLine("New Raffle draw has been started. Initial pot size: $" + raffleApp.PotSize);
            raffleApp.Users.Clear();
            raffleApp.Tickets.Clear();
            raffleApp.DrawStatus = Raffle.Status.ONGOING;
            Console.WriteLine("Press any key to return to main menu");
           
        }

        /*
         *  Add user and Ticket when buy tickets
         *  update the pot size  = no of tickets  * 5
         */
        public static void BuyTicket(Raffle raffleApp,string name,int nooftickets,List<string> tickets)
        {
          
            raffleApp.AddNewUser(name, nooftickets);

           
            Console.WriteLine("Hi " + name + ", you have purchased " + nooftickets + " ticket(s)");
            int index = 1;
            foreach (string ticket in tickets)
            {
                Console.WriteLine("Ticket " + index++ + ": " + ticket);
                raffleApp.AddNewTicket(name, ticket);
            }
            raffleApp.UpdatePotPrize(nooftickets);
            //raffleApp.UpdateUserTicketCount(name, nooftickets);
            Console.WriteLine();
            Console.WriteLine("Press any key to return to main menu");
            Console.WriteLine();
        }

         /*
          * Calculate & Update the Group (0 to 5) in the ticket list
          * Print Winners list from Group 2 to 5
          * 
          * Change the draw status to NONSTARTED after the draw 
         */
        public static void RunRaffle(Raffle raffleApp,string winningTicket)
        {
           
            raffleApp.UpdateWinnersGroup(winningTicket);
            raffleApp.printWinners();
            raffleApp.DrawStatus = Raffle.Status.NOTSTARTED;
        }

        /*
         * Show menu when the application starts
         * users to select 1,2,3
         * 
         *  Welcome to My Raffle App
            Status : Draw has not started
            [1] Start a New Draw
            [2] Buy Tickets
            [3] Run Raffle
            User can enter `1`, `2` or `3` to proceed
            input :

         */
        public static void ShowMenu(Raffle raffleApp)
        {
            Console.WriteLine("Welcome to My Raffle App");
            Console.WriteLine("Status : " + raffleApp.GetStatusDescription());
            Console.WriteLine("[1] Start a New Draw");
            Console.WriteLine("[2] Buy Tickets");
            Console.WriteLine("[3] Run Raffle");
            Console.WriteLine("User can enter `1`, `2` or `3` to proceed ");
        }

        /*
         Get input from user  1 , 2 or  3
         IF invalid input 
         input : a
         Invalid input. Please enter 1, 2, or 3.
         input :
         */
        public static char GetMenuInput()
        {
            int option;
            while (true)
            {
                Console.Write("input : ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out option))
                {
                    if(option ==1 || option == 2 || option ==3)
                    break;
                }
                Console.WriteLine("Invalid input. Please enter 1, 2, or 3.");
            }
            return (char)(option+48);
            
        }

        /*
         * Get input from user to buy ticket in the format name,number of ticket
         * ex:jitesh,2   
         * max 5 tickets / per for the draw
         * return name,number of tickets
         
           Enter your name, no of tickets to purchase ex: jitesh,2
           input : jeya,2 (user enter jeya,2)
           System generate the following output for valid name & no of tickets
           Hi jeya, you have purchased 2 ticket(s)
            Ticket 1: 1 4 7 10 12
            Ticket 2: 4 6 8 11 12
           Press any key to return to main menu

          IF already purchased all 5 tickets
          Enter your name, no of tickets to purchase ex: jitesh,2
          input : jeya,3
          Already purchased (5) tickets
          input :
         */
        public static (string, int) GetBuyInput(Raffle raffleApp)
        {
            string name = string.Empty;
            int noOfTicket = 0;
            while(true)
            {
                Console.Write("input : ");
                try
                {
                    string[] input = Console.ReadLine().Split(",");
                    name = input[0].Trim();
                    
                    int ticketCount =  raffleApp.GetUserTicketCount(name);//previouosly purchased tickets count
                    int ticketToPurchase = 5 - ticketCount;//remaining ticket to purchase

                    if (ticketToPurchase ==  0) //already purchase 5 tickets.
                    {
                        Console.WriteLine("Already purchased (" + ticketCount + ") tickets");
                        continue;
                    }
                    //if input lenght is not 2 or name is emtpy or name contains other than alphabet or ticket is not integer
                    //display error and continue get input again
                    //if existing user, it display previously purchased tickets so user can enter remaining allowed tickets
                    if (input.Length != 2 ||  name.Length <= 0 || name.Any(c => !char.IsLetter(c)) || !int.TryParse(input[1], out noOfTicket))
                    {
                        Console.WriteLine("Invalid Input. Enter your name, no of tickets to purchase ex: jitesh,2 (max " + ticketToPurchase + " tickets)");
                        if (ticketCount > 0) //if already purchased
                        {
                            Console.WriteLine("Already purchased (" + ticketCount + ") tickets");
                        }

                        continue;
                    }

                    //if buy more than allowed tickets to purchase or 0,it show error and how many purchased already
                    if(noOfTicket> ticketToPurchase || noOfTicket < 1 )
                    {
                        Console.WriteLine("Invalid Input. Enter your name, no of tickets to purchase ex: jitesh,2 (max " + ticketToPurchase + " tickets)");
                        if (ticketCount > 0)
                        {
                            Console.WriteLine("Already purchased (" + ticketCount + ") tickets");
                        }
                        continue;
                    }
                    break;
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Invalid Input. Enter your name, no of tickets to purchase ex: jitesh,2 \r\n");
                }
                
            }
            return (name, noOfTicket);
        }

        public static List<string> GenerateTicket(int noOfTickets)
        {
            string ticket = string.Empty;
            List<string> tickets = new List<string>();

            while (noOfTickets > 0)
            {
                ticket = string.Empty;
                ticket = RandomGenerator.GetDistinctRandomNumber();
                tickets.Add(ticket);
                noOfTickets--;
            }
            return tickets;
        }

        /*
        * Generate the winning ticket
        */
        public static string GetWinningTicket()
        {
            return GenerateTicket(1)[0];
        }

    }

    

  

   
}
