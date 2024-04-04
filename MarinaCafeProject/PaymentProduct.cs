using System;

namespace MarinaCafeProject
{
    internal class PaymentProduct
    {
        public int ProductId { get; set; }
        public int ProductCount { get; set; }
        public double ProductPrice { get; set; }
        public int PaidProductQty { get; set; }


        public PaymentProduct Clone()
        {
            return new PaymentProduct
            {
                ProductId = this.ProductId,
                ProductCount = this.ProductCount,
                ProductPrice = this.ProductPrice,
                PaidProductQty = this.PaidProductQty

            };
        }

        public void PrintProductInfo()
        {
            Console.WriteLine("Product ID :" + ProductId + ", Product Count :" + ProductCount + ", Product Price : " + ProductPrice + ", Paid Product Qty : " + PaidProductQty + ", Remaining This Product Amount : " + (ProductCount - PaidProductQty) * ProductPrice);
        }
    }
}