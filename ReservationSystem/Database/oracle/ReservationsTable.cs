using System;
using System.Collections.ObjectModel;
using Oracle.ManagedDataAccess.Client;
using AuctionSystem.ORM;
using Oracle.ManagedDataAccess.Types;
using Oracle.ManagedDataAccess;

namespace AuctionSystem.ORM.Oracle
{
    public class ReservationsTable
    {
        public static String SQL_SELECT = "SELECT res.IdReservation AS IdReservation , res.ReservationDate AS ReservationDate, res.EndReservation AS EndReservation, res.Rooms_IdRoom AS Rooms_IdRoom, res.Users_IdUser AS Users_IdUser, c.CancelationDate AS CancelationDate, c.Reason AS Reason, p.PaymentDate AS PaymentDate, NVL(res.Description,' ') AS Description " +
                                             "FROM Reservations res" +
                                               " LEFT JOIN Cancelations c ON c.Reservations_IdReservation = res.IdReservation " +
                                                 "LEFT JOIN Payments p ON p.Reservations_IdReservation = res.IdReservation ";
        public static String SQL_SELECT_HISTORICAL = "SELECT res.IdReservation AS IdReservation , res.ReservationDate AS ReservationDate, res.EndReservation AS EndReservation, res.Rooms_IdRoom AS Rooms_IdRoom, res.Users_IdUser AS Users_IdUser, c.CancelationDate AS CancelationDate, c.Reason AS Reason, p.PaymentDate AS PaymentDate, NVL(res.Description,' ') AS Description " +
                                                    "FROM Reservations res" +
                                                    " LEFT JOIN Cancelations c ON c.Reservations_IdReservation = res.IdReservation " +
                                                    " LEFT JOIN Payments p ON p.Reservations_IdReservation = res.IdReservation " +
                                                    " WHERE res.EndReservation < sysdate AND c.CancelationDate IS NOT NULL AND c.CancelationDate < sysdate";
        public static String SQL_SELECT_PRESENT = "SELECT res.IdReservation AS IdReservation , res.ReservationDate AS ReservationDate, res.EndReservation AS EndReservation, res.Rooms_IdRoom AS Rooms_IdRoom, res.Users_IdUser AS Users_IdUser, c.CancelationDate AS CancelationDate, c.Reason AS Reason, p.PaymentDate AS PaymentDate, NVL(res.Description,' ') AS Description " +
                                                   "FROM Reservations res" +
                                                   " LEFT JOIN Cancelations c ON c.Reservations_IdReservation = res.IdReservation " +
                                                   " LEFT JOIN Payments p ON p.Reservations_IdReservation = res.IdReservation " +
                                                   " WHERE res.ReservationDate > sysdate";
        public static String SQL_SELECT_ID = "SELECT res.IdReservation AS IdReservation, res.ReservationDate AS ReservationDate, res.EndReservation AS EndReservation, res.Rooms_IdRoom AS Rooms_IdRoom, res.Users_IdUser AS Users_IdUser, c.CancelationDate AS CancelationDate, " +
                                                "c.Reason AS Reason, p.PaymentDate AS PaymentDate, p.Reservations_IdReservation AS Reservations_IdReservation, NVL(res.Description,' ') AS Description " +
                                                "FROM Reservations res" +
                                                 " LEFT JOIN Cancelations c ON c.Reservations_IdReservation = res.IdReservation " +
                                                     "LEFT JOIN Payments p ON p.Reservations_IdReservation = res.IdReservation " +
                                                "WHERE IdReservation=:idReservation";
        public static String SQL_SELECT_LAST_ID_RESERVATION = "SELECT MAX(idReservation) AS LAST_ID FROM Reservations ";
        public static String SQL_SELECT_REPORT_RESERVATIONS = "SELECT RoomNumber, " +
                                                            " (Select COUNT(*) FROM Reservations WHERE Rooms.idRoom = Reservations.Rooms_idRoom AND Reservations.ReservationDate > sysdate ) as pocet_buducich_rezervacii, " +
                                                            " (Select COUNT(*) FROM Reservations JOIN Payments ON Reservations.IdReservation = Payments.Reservations_IdReservation WHERE Rooms.idRoom = Reservations.Rooms_idRoom " +
                                                            " AND Reservations.ReservationDate > sysdate AND Payments.PaymentDate IS NOT NULL) as pocet_buducich_zaplatenych " +
                                                            " FROM Rooms Where Rooms.RoomTypes_IdRoomType=:idRoomType ";
        public static String SQL_INSERT = "INSERT INTO Reservations (IdReservation, ReservationDate, EndReservation, Rooms_IdRoom, Users_IdUser, Description) VALUES (:idReservation, :reservationDate, :endReservation, :idRoom, :idUser, :description)";
        public static String SQL_DELETE_ID = "DELETE FROM Reservations WHERE IdReservation=:idReservation";
        public static String SQL_UPDATE = "UPDATE Reservations SET ReservationDate=:reservationDate, EndReservation=:endReservation, Rooms_IdRoom=:idRoom," +
            "Users_IdUser=:idUser, Description=:description WHERE IdReservation=:idReservation";
       




        public int insert(Reservation reservation, Database pDb = null)
        {
            Database db = new Database();
            db.Connect();

            reservation.IdReservation = createIdReservation();

            OracleCommand command = db.CreateCommand(SQL_INSERT);
            PrepareCommand(command, reservation);
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            return ret;
        }

        
        public int update(Reservation reservation, Database pDb = null)
        {
            Database db = new Database();
            db.Connect();

            OracleCommand command = db.CreateCommand(SQL_UPDATE);
            PrepareCommand(command, reservation);
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            return ret;
        }


     

        public Collection<ReportReservation> selectReportReservations(int idRoomType)
        {

            Database db = new Database();
            db.Connect();

            OracleCommand command = db.CreateCommand(SQL_SELECT_REPORT_RESERVATIONS);

            command.Parameters.AddWithValue(":idRoomType", idRoomType);
            //command.Parameters.AddWithValue(":v_date1", DateTime.Now);
            //command.Parameters.AddWithValue(":v_date2", DateTime.Now);

            OracleDataReader reader = db.Select(command);
            Collection<ReportReservation> reportReservations = new Collection<ReportReservation>();

            while (reader.Read())
            {
                ReportReservation reportReservation = new ReportReservation();
                reportReservation.RoomNumber = reader.GetInt32(reader.GetOrdinal("RoomNumber"));
                reportReservation.Report1 = reader.GetInt32(reader.GetOrdinal("pocet_buducich_rezervacii"));
                reportReservation.Report2 = reader.GetInt32(reader.GetOrdinal("pocet_buducich_zaplatenych"));

                reportReservations.Add(reportReservation);
            }
            


            reader.Close();

            return reportReservations;
        }

        

        
        public Reservation select(int IdReservation, Database pDb = null)
        {
            Database db = new Database();
            db.Connect();
            OracleCommand command = db.CreateCommand(SQL_SELECT_ID);

            command.Parameters.AddWithValue(":idReservation", IdReservation);
            OracleDataReader reader = db.Select(command);

            Collection<Reservation> reservations = Read(reader);
            Reservation reservation = null;
            if (reservations.Count == 1)
            {
                reservation = reservations[0];
            }
            reader.Close();
            db.Close();
            return reservation;
        }

      
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

        public Collection<Reservation> select(Database pDb = null)
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

            Collection<Reservation> reservations = Read(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return reservations;
        }

        public Collection<Reservation> selectHistorical(Database pDb = null)
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

            OracleCommand command = db.CreateCommand(SQL_SELECT_HISTORICAL);
            OracleDataReader reader = db.Select(command);

            Collection<Reservation> reservations = Read(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return reservations;
        }

         public Collection<Reservation> selectPresent(Database pDb = null)
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

             OracleCommand command = db.CreateCommand(SQL_SELECT_PRESENT);
             OracleDataReader reader = db.Select(command);

             Collection<Reservation> reservations = Read(reader);
             reader.Close();

             if (pDb == null)
             {
                 db.Close();
             }

             return reservations;
         }


        public int createIdReservation(Database pDb = null)
        {
            Database db = new Database();
            db.Connect();
            OracleCommand command = db.CreateCommand(SQL_SELECT_LAST_ID_RESERVATION);
            OracleDataReader reader = db.Select(command);

            int lastId = 0;

            Object lastIdScalar = db.CreateCommand(SQL_SELECT_LAST_ID_RESERVATION).ExecuteScalar();

            if (lastIdScalar != null)
            {
                lastId = Convert.ToInt32(lastIdScalar);
            }
            db.Close();

            return ++lastId;
        }


        //volania procedur
        // 3.1 Vytvorenie rezervácie - transakcia

        public int CreateNewReservation(Reservation reservation, string idUsers_list)
        {
            Database db = new Database();           
            db.Connect();

            OracleCommand command = db.CreateCommand("CreateReservation");
           // command.Connection = db.getConnection();
           // command.CommandText = "CreateReservation";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("v_idreservation", OracleDbType.Int32).Value = reservation.IdReservation;
            command.Parameters.Add("v_reservationdate", OracleDbType.Date).Value = reservation.ReservationDate;
            command.Parameters.Add("v_endreservation", OracleDbType.Date).Value = reservation.EndReservation;
            command.Parameters.Add("v_iduser", OracleDbType.Int32).Value = reservation.IdUser;
            command.Parameters.Add("v_idroom", OracleDbType.Int32).Value = reservation.IdRoom;
            command.Parameters.Add("v_description", OracleDbType.Varchar2).Value = reservation.Description;
            command.Parameters.Add("v_idUsers_list", OracleDbType.Varchar2).Value = idUsers_list;            

            db.ExecuteNonQuery(command);
        
            db.Close();
        
            return 1;
        }


        // 3.5 Zrušenie rezervácií a odstránenie platieb - transakcia

        public int CancelationOfReserAndPayments(DateTime dateFrom, DateTime dateTo)
        {
        
            Database db = new Database();           
            db.Connect();
        
             OracleCommand command = db.CreateCommand("CancelationOfReservationAndPayments");

             command.CommandType = System.Data.CommandType.StoredProcedure;
             command.Parameters.Add("v_reservationFrom", OracleDbType.Date).Value = dateFrom;
             command.Parameters.Add("v_reservationTo", OracleDbType.Date).Value = dateTo;
             command.Parameters.Add("v_date", OracleDbType.Date).Value = DateTime.Now;
                

             db.ExecuteNonQuery(command);
             db.Close();
        
             return 1;
        }


       // 3.4 Zrušenie budúcich rezervácií užívateľa


        public int CancelationOfFutureReservations(int idUser)
        {
            Database db = new Database();           
            db.Connect();
        
            OracleCommand command = db.CreateCommand("CancelationOfFutureReservations");
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("v_IdUser", OracleDbType.Int32).Value = idUser;
            command.Parameters.Add("v_currentDate", OracleDbType.Date).Value = DateTime.Now;
        
            db.ExecuteNonQuery(command);
            db.Close();
             
            return 1;
        }


         // 3.6 Overenie dostupnosti 

        public bool VerificationOfAblity(DateTime v_reservationFrom, DateTime v_reservationTo, int v_idRoom)
        { 
            bool result = false;

            Database db = new Database();           
            db.Connect();
            
            OracleCommand command = db.CreateCommand("SELECT VerificationOfAbility(:dateFrom,:dateRoom,:idRoom) AS RESULT from dual");
            command.Parameters.AddWithValue(":dateFrom", v_reservationFrom);
            command.Parameters.AddWithValue(":dateTo", v_reservationTo);
            command.Parameters.AddWithValue(":idRoom", v_idRoom);

            OracleDataReader reader = db.Select(command);

            while (reader.Read())
            {
                result = bool.Parse(reader.GetString(reader.GetOrdinal("RESULT")));
            }

            reader.Close();
                    
            return result;
        }

        private static void PrepareCommand(OracleCommand command, Reservation reservation)
        {
            command.BindByName = true;
            command.Parameters.AddWithValue(":idReservation", reservation.IdReservation);
            command.Parameters.AddWithValue(":reservationDate", reservation.ReservationDate);
            command.Parameters.AddWithValue(":endReservation", reservation.EndReservation);
            command.Parameters.AddWithValue(":idRoom", reservation.IdRoom);
            command.Parameters.AddWithValue(":idUser", reservation.IdUser);
            command.Parameters.AddWithValue(":description", reservation.Description == null ? DBNull.Value : (object)reservation.Description);
            
        }

        private static Collection<Reservation> Read(OracleDataReader reader)
        {
            Collection<Reservation> reservations = new Collection<Reservation>();

            while (reader.Read())
            {
                Reservation reservation = new Reservation();
                reservation.IdReservation = reader.GetInt32(reader.GetOrdinal("IdReservation"));
                reservation.ReservationDate = reader.GetDateTime(reader.GetOrdinal("ReservationDate"));
                reservation.EndReservation = reader.GetDateTime(reader.GetOrdinal("EndReservation"));
                reservation.IdRoom = reader.GetInt32(reader.GetOrdinal("Rooms_IdRoom"));
                reservation.IdUser = reader.GetInt32(reader.GetOrdinal("Users_IdUser"));
                reservation.Cancelation = !reader.IsDBNull(reader.GetOrdinal("CancelationDate")) ? new Cancelation(reader.GetDateTime(reader.GetOrdinal("CancelationDate")), !reader.IsDBNull(reader.GetOrdinal("Reason")) ? reader.GetString(reader.GetOrdinal("Reason")) : "") : null;
                reservation.Payment = !reader.IsDBNull(reader.GetOrdinal("PaymentDate")) ? new Payment(reader.GetDateTime(reader.GetOrdinal("PaymentDate"))) : null;
                reservation.Description = reader.GetString(reader.GetOrdinal("Description"));
              
                /* if (!reader.IsDBNull(++i))
                {
                    User.LastVisit = reader.GetDateTime(i);
                }
                User.Type = reader.GetString(++i);*/

                reservations.Add(reservation);
            }

           return reservations;
        }   
    }  

}


