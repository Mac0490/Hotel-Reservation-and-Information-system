using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace AuctionSystem.ORM.Oracle
{
    public class RoomTable
    {
        public static String TABLE_NAME = "Rooms";

        public static String SQL_SELECT = "SELECT r.IdRoom AS IdRoom , r.RoomNumber AS RoomNumber, r.RoomTypes_IdRoomType AS RoomTypes_IdRoomType, rt.RoomTypeName AS RoomTypeName, rt.Price AS Price, r.Floor AS Floor, NVL(r.Description,' ') AS Description" +
                                              " FROM Rooms r" +
                                             " JOIN RoomTypes rt ON rt.IdRoomType = r.RoomTypes_IdRoomType ";
        public static String SQL_SELECT_ID = "SELECT r.IdRoom AS IdRoom, r.RoomNumber AS RoomNumber, r.RoomTypes_IdRoomType AS RoomTypes_IdRoomType, rt.RoomTypeName AS RoomTypeName, rt.Price AS Price, r.Floor AS Floor, NVL(r.Description,' ') AS Description " +
                                                  "FROM Rooms  r " +
                                                " JOIN RoomTypes rt ON rt.IdRoomType = r.RoomTypes_IdRoomType " +
                                                  "WHERE IdRoom=:idRoom";
        public static String SQL_SELECT_LAST_ID_ROOM = "SELECT MAX(idRoom) AS LAST_ID FROM ROOMS ";
       // public static String SQL_SELECT_NAME = "SELECT * FROM Category WHERE name=:name";
        public static String SQL_INSERT = "INSERT INTO Rooms (IdRoom, RoomNumber, RoomTypes_IdRoomType, Floor, Description) VALUES (:idRoom, :roomNumber, :idroomtype, :floor, :description)";
        public static String SQL_DELETE_ID = "DELETE FROM Rooms WHERE IdRoom=:idRoom";
        public static String SQL_UPDATE = "UPDATE Rooms SET RoomNumber=:roomNumber, RoomTypes_IdRoomType=:idRoomType, Floor=:floor, Description=:description WHERE IdRoom=:idRoom";
        //public static String SQL_UPDATE = "UPDATE Rooms SET RoomNumber=:roomNumber, RoomTypes_IdRoomType=:idRoomType, Floor=:floor, Description=:description WHERE IdRoom =:idRoom";


        #region metody
        /// <summary>
        /// Insert the record.
        /// </summary>
        public int insert(Room room, Database pDb = null)
        {
            Database db;
            if (pDb == null)
            {
                db = new Database();
                db.Connect();
            }
            else
            {
                db = (Database)pDb;
            }

            room.IdRoom = createIdRoom();

            OracleCommand command = db.CreateCommand(SQL_INSERT);
            PrepareCommand(command, room);
            int ret = db.ExecuteNonQuery(command);

            db.Close();
            return ret;
        }

        public int createIdRoom()
        {
            Database db = new Database();
            db.Connect();

            int lastId = 0;

            Object lastIdScalar = db.CreateCommand(SQL_SELECT_LAST_ID_ROOM).ExecuteScalar();

            if (lastIdScalar != null)
            {
                lastId = Convert.ToInt32(lastIdScalar);
            }

            db.Close();

            return ++lastId;
        }

        /// <summary>
        /// Update the record.
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public int update(Room room, Database pDb = null)
        {
            Database db = new Database();
            db.Connect();

            OracleCommand command = db.CreateCommand(SQL_UPDATE);
            PrepareCommand(command, room);
            int ret = db.ExecuteNonQuery(command);

            db.Close();
            return ret;
        }


        /// <summary>
        /// Select records.
        /// </summary>
        public Collection<Room> select(Database pDb = null)
        {
            Database db;
            if (pDb == null)
            {
                db = new Database();
                db.Connect();
            }
            else
            {
                db = (Database)pDb;
            }

            OracleCommand command = db.CreateCommand(SQL_SELECT);
            OracleDataReader reader = db.Select(command);

            Collection<Room> room = Read(reader);
            reader.Close();
            db.Close();
            return room;
        }

        /// <summary>
        /// Select records for category.
        /// </summary>
       public Room select(int IdRoom)
        {
            Database db = new Database();
            db.Connect();
            OracleCommand command = db.CreateCommand(SQL_SELECT_ID);

            command.Parameters.AddWithValue(":idRoom", IdRoom);
            OracleDataReader reader = db.Select(command);

            Collection<Room> rooms = Read(reader);
            Room room = null;
            if (rooms.Count == 1)
            {
                room = rooms[0];
            }
            reader.Close();
            db.Close();
            return room;
        }
        /// <summary>
        /// Delete the record.
        /// </summary>
        public int delete(int id, Database pDb = null)
        {
            Database db = new Database();
            db.Connect();
            OracleCommand command = db.CreateCommand(SQL_DELETE_ID);

            command.Parameters.AddWithValue(":id", id);
            int ret = db.ExecuteNonQuery(command);

            db.Close();
            return ret;
        }
        #endregion

        /// <summary>
        /// Select the record for a name.
        /// </summary>
       /* public static Room SelectForName(string pName, Database pDb = null)
        {
            Database db;
            if (pDb == null)
            {
                db = new Database();
                db.Connect();
            }
            else
            {
                db = pDb;
            }

            OracleCommand command = db.CreateCommand(SQL_SELECT_NAME);

            command.Parameters.AddWithValue(":rolename", pName);
            OracleDataReader reader = db.Select(command);

            Collection<Room> rooms = Read(reader);
            Room room = null;
            if (rooms.Count == 1)
            {
                room = rooms[0];
            }
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return room;
        }*/

        /// <summary>
        /// Prepare a command.
        /// </summary>
        private static void PrepareCommand(OracleCommand command, Room room)
        {
            command.BindByName = true;
            command.Parameters.AddWithValue(":idRoom", room.IdRoom);
            command.Parameters.AddWithValue(":roomNumber", room.RoomNumber);
            command.Parameters.AddWithValue(":idRoomType", room.IdRoomType);
            command.Parameters.AddWithValue(":floor", room.Floor);
            command.Parameters.AddWithValue(":description", room.Description == null ? DBNull.Value : (object)room.Description);
        }

        private static Collection<Room> Read(OracleDataReader reader)
        {
            Collection<Room> rooms = new Collection<Room>();

            while (reader.Read())
            {
                Room room = new Room();
                room.IdRoom = reader.GetInt32(reader.GetOrdinal("IdRoom"));
                room.RoomNumber = reader.GetInt32(reader.GetOrdinal("RoomNumber"));
                room.IdRoomType = reader.GetInt32(reader.GetOrdinal("RoomTypes_IdRoomType"));
                room.RoomType = new RoomType(room.IdRoomType, reader.GetString(reader.GetOrdinal("RoomTypeName")), reader.GetInt32(reader.GetOrdinal("Price")));
                room.Floor = reader.GetInt32(reader.GetOrdinal("Floor"));
                room.Description = reader.GetString(reader.GetOrdinal("Description"));
               
                /* if (!reader.IsDBNull(++i))
                {
                    User.LastVisit = reader.GetDateTime(i);
                }
                User.Type = reader.GetString(++i);*/

                rooms.Add(room);
            }
            return rooms;
        }

    }
}