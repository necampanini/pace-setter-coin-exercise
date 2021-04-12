using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PacesetterCoin
{
    class Program
    {
        static void Main(string[] args)
        {
            // read in file, make ordered list of price point objects (date & price)
            var pricePoints = MakePricePoints();

            foreach (var current in pricePoints)
            {
                // using LINQ on the price point object, get the highest price point in all subsequent dates
                current.SubsequentPricePoints = pricePoints.Where(p => p.DateTime > current.DateTime).ToList();
            }

            var buyOn = pricePoints.OrderByDescending(p => p.DifferenceFromHighestSubsequentPrice).First();

            Console.WriteLine($"buy on: {buyOn.DateTime} at {buyOn.Price}");
            Console.WriteLine($"to sell on : {buyOn.TargetPricePoint.DateTime} for {buyOn.TargetPricePoint.Price}");

            var profit = buyOn.TargetPricePoint.Price - buyOn.Price;

            // not positive if there's some special return percentage calculation
            // assuming its just dividing profit by investment, then multiplied by 100

            var percentageReturn = (profit / buyOn.Price) * 100;
            Console.WriteLine($"Profit {profit:C}");
            Console.WriteLine($"for return of {percentageReturn:N}%");

        }

        private static List<PricePoint> MakePricePoints()
        {
            var list = new List<PricePoint>();

            string line;
            var file = new StreamReader(@"C:\2018-pacesetter-coin.txt");

            while ((line = file.ReadLine()) != null)
            {
                var array = line.Split('\t');

                // skip first line
                if (array[0] == "Date")
                {
                    continue;
                }

                // could use try/catches for safety
                DateTime.TryParse(array[0], out var date);
                decimal.TryParse(array[1], out var price);

                // can parse
                if (date == DateTime.MinValue) continue;

                var newPricePoint = new PricePoint
                {
                    DateTime = date, Price = price
                };

                list.Add(newPricePoint);
            }

            return list.OrderBy(x => x.DateTime).ToList();
        }
    }
}
