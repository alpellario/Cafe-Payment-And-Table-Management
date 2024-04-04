namespace MarinaCafeProject
{
    internal class CafeProductComputation
    {
        double unitCost;
        double unitRequiredSize;
        int onHandQuantity;
        double unitRequiredQuantity;
        double unitRemainQuantity;
        double remainQuantity;
        double unitProfit;

        public CafeProductComputation()
        {

        }

        public double GetUnitCost(double RawSize, double UnitPrice, double ProductSize)
        {
            if (UnitPrice != 0 || ProductSize != 0)
            {
                return unitCost = (RawSize * UnitPrice) / ProductSize;
            }
            else return 0;

        }

        public double GetUnitRequiredQuantity(double RawSize, double ProductSize)
        {
            if (ProductSize != 0)
            {
                return unitRequiredQuantity = RawSize / ProductSize;
            }
            else return 0;

        }
        public double GetUnitRemainQuantity(double OnHandQuantity, double UnitRequiredQuantity)
        {
            return unitRemainQuantity = OnHandQuantity - UnitRequiredQuantity;
        }

        public double GetRemainQuantity(double OnHandQuantity, int ProductCount, double UnitRequiredQuantity)
        {
            return remainQuantity = OnHandQuantity - (ProductCount * UnitRequiredQuantity);
        }

        public double GetUnitProfit(double ProductPrice, int ProductCount, double TotalCost)
        {
            return unitProfit = (ProductPrice * ProductCount) - TotalCost;
        }

        /// <summary>
        /// For Payment Screen
        /// </summary>
        public double CalculateRemainingAmount(int ProductCount, int PaidProductQty, double ProductPrice)
        {
            double totalAmount = 0;

            totalAmount = (ProductCount - PaidProductQty) * ProductPrice;

            return totalAmount;
        }

        public double CalculateReceivedAmount(int PaidProductQty, double ProductPrice)
        {
            double totalAmount = 0;

            totalAmount = PaidProductQty * ProductPrice;

            return totalAmount;
        }

        public double CalculateDiscount(double ProductPrice, double discount)
        {
            double discountAmount = 0;

            discountAmount = ProductPrice * discount / 100;

            string _amount = discount.ToString("0.##");

            return discountAmount;
        }
    }
}