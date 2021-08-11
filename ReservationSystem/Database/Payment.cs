using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.ORM
{
    public class Payment
    {
        public Payment(DateTime paymentDate)
        {
            PaymentDate = paymentDate;
            
            
        }

        public DateTime PaymentDate { get; set; }
        
    }
}
