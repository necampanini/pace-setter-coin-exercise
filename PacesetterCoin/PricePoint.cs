using System;
using System.Collections.Generic;
using System.Linq;

namespace PacesetterCoin
{
    public class PricePoint
    {
        public DateTime DateTime { get; set; }

        public decimal Price { get; set; }

        public List<PricePoint> SubsequentPricePoints { get; set; } = new List<PricePoint>();

        public PricePoint TargetPricePoint
        {
            get
            {
                if (!SubsequentPricePoints.Any()) return null;

                return SubsequentPricePoints
                    .OrderByDescending(spp => spp.Price)
                    .First();
            }
        }

        public decimal DifferenceFromHighestSubsequentPrice
        {
            get
            {
                if (TargetPricePoint == null) return 0;
                var highest = TargetPricePoint.Price;

                return highest - this.Price;
            }
        }
    }
}
