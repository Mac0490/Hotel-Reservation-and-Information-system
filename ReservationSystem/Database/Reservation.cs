using System;
using System.Collections.Generic;
using System.Text;

namespace AuctionSystem.ORM
{
    public class Reservation
    {
        public int IdReservation { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime EndReservation { get; set; }
        public int IdRoom { get; set; }
        public int IdUser { get; set; }
        public string Description { get; set; }
        public Cancelation  Cancelation { get; set; }

        public Payment Payment { get; set; }

        // public string DescriptionDetail { get; set; }
        // public DateTime Creation { get; set; }
        // public DateTime End { get; set; }
        //  public int IdCategory { get; set; }
        //  public Category Category { get; set; }
        // public int? MaxBidValue { get; set; } //int with null value. Call .HasValue to determine if a int value was set.
        //  public int? MinBidValue { get; set; } //int with null value. Call .HasValue to determine if a int value was set.
        //  public int? IdMaxBidUser { get; set; } //int with null value. Call .HasValue to determine if a int value was set.

        // public User Owner { get; set; }
        // public User MaxBidder { get; set; }

    }
}