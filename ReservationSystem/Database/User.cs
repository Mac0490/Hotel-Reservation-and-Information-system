using System;
using System.Collections.ObjectModel;

namespace AuctionSystem.ORM
{
    public class User
    {
        public int IdUser { get; set; }
        public String email { get; set; }
        public String Login { get; set; }
        public String Password { get; set; }
        public String PhoneNumber { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
        public String IdCard_NO { get; set; }
        public int CancelationCount { get; set; }

        public int IdRole { get; set; }

        public Role Role { get; set; }

        public Collection<Reservation> Reservations { get; set; }

        public Reservation ReservationTemp { get; set; }
        //Artificial columns (physically not in the database)
        // public String FullName { get { return this.Name + " " + this.Surname; } }
    }
}