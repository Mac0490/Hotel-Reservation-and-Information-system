using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.ORM
{
    public class Role
    {
        public Role(int idRool, string roolName)
        {
            IdRool = idRool;
            RoolName = roolName;
        }

        public int IdRool { get; set; }

        public String RoolName { get; set; }

    }
}
