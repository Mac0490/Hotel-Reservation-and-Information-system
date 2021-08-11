using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.ORM
{
    public class RoomType
    {
        public RoomType(int idRoomType, string roomTypeName, int price)
        {
            IdRoomType = idRoomType;
            RoomTypeName = roomTypeName;
            Price = price;
        }

        public int IdRoomType { get; set; }

        public String RoomTypeName { get; set; }

        public int Price { get; set; }
    }
}
