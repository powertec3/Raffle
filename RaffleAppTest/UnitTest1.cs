using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace RaffleApp
{
   
    public class RaffleAppTests
    {
        [Test]
        public void TestRaffles()
        {
            Console.WriteLine("Hhello Happy Tesing");
        }

        [Test]
        public  void TestShowMenu()
        {
            Raffle raffleApp = new Raffle(100);
            Program.ShowMenu(raffleApp);
        }
        [Test]
        public  void TestStartNewDraw()
        {
            Raffle raffleApp = new Raffle(100);
            Program.StartNewDraw(raffleApp);

            Assert.IsEmpty(raffleApp.Users);
            Assert.IsEmpty(raffleApp.Tickets);
            Assert.AreEqual(100, raffleApp.InitialPotSize);
            Assert.AreEqual(100, raffleApp.PotSize);
            
        }

        [Test]
        public void TestBuyTickets()
        {
            Raffle raffleApp = new Raffle(100);
            Program.StartNewDraw(raffleApp);
            List<string> tickets = Program.GenerateTicket(2);
            Program.BuyTicket(raffleApp,"jeya",2,tickets);
            
            Assert.AreEqual(1, raffleApp.Users.Count());
            Assert.AreEqual(2, raffleApp.Tickets.Count);
            Assert.AreEqual(110, raffleApp.PotSize);
        }
        

        [Test]
        [TestCase("jeya",1, "2 4 9 10 13")]
        
        public void TestRunRaffle2(string name,int noofticket,string ticket)
        {
            Raffle raffleApp = new Raffle(100);
            Program.StartNewDraw(raffleApp);
            List<string> tickets = ticket.Split(',').ToList();
            Program.BuyTicket(raffleApp,name,noofticket,tickets);

            var capturedStdOut = CapturedStdOut(() =>
            {
                Program.RunRaffle(raffleApp, "4 6 8 10 12");
            });

            Console.WriteLine(capturedStdOut);

            StringAssert.Contains("Group 2 Winners:", capturedStdOut);
            StringAssert.Contains("jeya with  1 winning ticket(s)", capturedStdOut);
            
        }

        [Test]
        [TestCase("marina",2, "1 4 6 12 15, 5 7 9 11 14")]
        public void TestRunRaffle3(string name, int noofticket, string ticket)
        {

            Raffle raffleApp = new Raffle(100);
            Program.StartNewDraw(raffleApp);
            List<string> tickets = ticket.Split(',').ToList();
            Program.BuyTicket(raffleApp, name, noofticket, tickets);

            var capturedStdOut = CapturedStdOut(() =>
            {
                Program.RunRaffle(raffleApp, "4 6 8 10 12");
            });
            Console.WriteLine(capturedStdOut);

            StringAssert.Contains("Group 4 Winners:", capturedStdOut);
            StringAssert.Contains("marina with  1 winning ticket(s)", capturedStdOut);
        }

        [Test]
        public void TestRandomGenerator()
        {
            string ticket = RandomGenerator.GetDistinctRandomNumber();

            Assert.AreEqual(5, ticket.Trim().Split(" ").Length);
        }

        string CapturedStdOut(Action callback)
        {
            TextWriter originalStdOut = Console.Out;

            using var newStdOut = new StringWriter();
            Console.SetOut(newStdOut);

            callback.Invoke();
            var capturedOutput = newStdOut.ToString();

            Console.SetOut(originalStdOut);

            return capturedOutput;
        }
    }
    
}