using System;

namespace MarinaCafeProject
{
    internal class PaymentAmountDetail
    {
        public double PaidCardAmount { get; set; }
        public double PaidCashAmount { get; set; }

        public void PrintCashCardInfo()
        {
            Console.WriteLine("Total Card : " + PaidCardAmount + ", Total Cash : " + PaidCashAmount);
        }

        public void ClearAmount()
        {
            this.PaidCardAmount = 0;
            this.PaidCashAmount = 0;
        }
    }
}