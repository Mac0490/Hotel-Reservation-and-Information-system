using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.ORM
{
    public class Cancelation
    {
        public Cancelation(DateTime cancelationDate, string reason)
        {
            CancelationDate = cancelationDate;
            Reason = reason;
            
        }

        public DateTime CancelationDate { get; set; }
        public String Reason { get; set; }
    }
}
