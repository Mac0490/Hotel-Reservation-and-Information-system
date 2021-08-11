using System;
using System.Collections.Generic;
using System.Text;

namespace AuctionSystem.ORM
{
    public class Room
    {
        public int IdRoom { get; set; }
        public int RoomNumber { get; set; }
        public int IdRoomType { get; set; }
        public int Floor { get; set; }

        public String Description { get; set; }

        public RoomType RoomType { get; set; }

    }
}