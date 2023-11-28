using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaffleApp
{
    public  static class RandomGenerator
    {
        /*
        * Generate 5 distinct random numbers between 1 and 15
        * and return as a string ex: "5 7 9 10 23"
        */
        public static string GetDistinctRandomNumber()
        {
            Random random = new Random();
            HashSet<int> distinctNumbers = new HashSet<int>();
            string randomNumbers = string.Empty;

            while (distinctNumbers.Count < 5)
            {
                int randomNumber = random.Next(1, 16);
                distinctNumbers.Add(randomNumber);
            }
            List<int> sortedList = distinctNumbers.ToList();
            sortedList.Sort();


            foreach (int number in sortedList)
            {
                randomNumbers += number.ToString() + " ";
            }
            return randomNumbers.Trim();
        }

    }
}
