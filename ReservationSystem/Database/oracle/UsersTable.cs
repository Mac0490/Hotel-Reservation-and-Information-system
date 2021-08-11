using System;
using System.Collections.ObjectModel;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;

namespace AuctionSystem.ORM.Oracle
{ 
    public class UsersTable
    {
        public static String SQL_SELECT = "SELECT u.IDUSER AS IDUSER, u.EMAIL AS EMAIL, u.ROLES_IDROLE AS ROLES_IDROLE, r.ROLENAME AS ROLENAME, u.LOGIN AS LOGIN, u.PHONENUMBER AS PHONENUMBER, NAME AS NAME, SURNAME AS SURNAME, u.\"IdCard-NO\" AS IDCARDNO, NVL(u.CANCELATIONCOUNT,0) AS CANCELATIONCOUNT, " +
                                              " res.idReservation AS idReservation, res.reservationDate AS reservationDate, res.EndReservation AS EndReservation,  res.Description AS Description " +
                                              " FROM Users u " +
                                              " JOIN ROLES r ON r.IDROLE = u.ROLES_IDROLE " +
                                              " LEFT JOIN Reservations res on res.Users_IDUser = u.idUser ";
        public static String SQL_SELECT_EMPLOYEES = "SELECT u.IDUSER AS IDUSER, u.EMAIL AS EMAIL, u.ROLES_IDROLE AS ROLES_IDROLE, r.ROLENAME AS ROLENAME, u.LOGIN AS LOGIN, u.PHONENUMBER AS PHONENUMBER, NAME AS NAME, SURNAME AS SURNAME, u.\"IdCard-NO\" AS IDCARDNO, NVL(u.CANCELATIONCOUNT,0) AS CANCELATIONCOUNT, " +
                                              " res.idReservation AS idReservation, res.reservationDate AS reservationDate, res.EndReservation AS EndReservation,  res.Description AS Description " +
                                              " FROM Users u " +
                                              " JOIN ROLES r ON r.IDROLE = u.ROLES_IDROLE " +
                                              " LEFT JOIN Reservations res on res.Users_IDUser = u.idUser  " +
                                              "WHERE u.ROLES_IDROLE = 2 ";
        public static String SQL_SELECT_ID = "SELECT u.IDUSER AS IDUSER, u.EMAIL AS EMAIL, u.ROLES_IDROLE AS ROLES_IDROLE, r.ROLENAME AS ROLENAME, u.LOGIN AS LOGIN, u.PHONENUMBER AS PHONENUMBER, NAME AS NAME, SURNAME AS SURNAME, u.\"IdCard-NO\" AS IDCARDNO, NVL(u.CANCELATIONCOUNT,0) AS CANCELATIONCOUNT, " +
                                              " res.idReservation AS idReservation, res.reservationDate AS reservationDate, res.EndReservation AS EndReservation,  res.Description AS Description " + 
                                              " FROM Users u " +
                                              " JOIN ROLES r ON r.IDROLE = u.ROLES_IDROLE " +
                                              " LEFT JOIN Reservations res on res.Users_IDUser = u.idUser " + 
                                              " WHERE IdUser=:idUser";
        public static String SQL_SELECT_LAST_ID_USER = "SELECT MAX(idUser) AS LAST_ID FROM Users ";
        public static String SQL_INSERT = "INSERT INTO Users (idUser,email,ROLES_IDROLE, Login, Password,PhoneNumber,Name,Surname,\"IdCard-NO\",CancelationCount) VALUES (:idUser, :email, :idrole, :login, :password, :phonenumber, :name, :surname, :idCardNo, :cancelationcount)";
        public static String SQL_DELETE_ID = "DELETE FROM Users WHERE idUser=:idUser";
        public static String SQL_UPDATE = "UPDATE Users SET login=:login, name=:name, surname=:surname," +
                                          "Password=:password, PhoneNumber=:phonenumber, EMAIL=:email," +
                                          "ROLES_IDROLE=:idrole WHERE idUser=:idUser";

        
        
        public int insert(User user, Database pDb = null)
        {
            Database db = new Database();
            db.Connect();

            user.IdUser = createIdUser(db);

            OracleCommand command = db.CreateCommand(SQL_INSERT);
            PrepareCommand(command, user);
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            return ret;
        }

        
        public int update(User user, Database pDb = null)
        {
            Database db = new Database();
            db.Connect();

            OracleCommand command = db.CreateCommand(SQL_UPDATE);
            PrepareCommand(command, user);
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            return ret;
        }


      
        public Collection<User> select(Database pDb = null)
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

            Collection<User> users = Read(reader);

            Dictionary<int,User> usersDict = new Dictionary<int,User>();

            foreach (User userItem in users)
            {
                if (!usersDict.ContainsKey(userItem.IdUser))
                {
                    usersDict.Add(userItem.IdUser, userItem);
                }
            }

            Collection<User> uniqueUsers = new Collection<User>();

            foreach (KeyValuePair<int, User> userDictItem in usersDict)
            {
                User user = userDictItem.Value;
                Collection<Reservation> reservations = new Collection<Reservation>();

                foreach (User userItem in users) {

                    if (userItem.IdUser == userDictItem.Key && userItem.ReservationTemp != null) 
                    { 
                        Reservation reservation = new Reservation();
                        reservation.IdReservation = userItem.ReservationTemp.IdReservation;
                        reservation.ReservationDate = userItem.ReservationTemp.ReservationDate;
                        reservation.EndReservation = userItem.ReservationTemp.EndReservation;
                        reservation.Description = userItem.ReservationTemp.Description;

                        reservations.Add(reservation);                    
                    }                  

                }

                user.Reservations = reservations;
                user.ReservationTemp = null;

                uniqueUsers.Add(user);
            }

            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return uniqueUsers;
        }

       
        public Collection<User> selectEmployees(Database pDb = null)
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

            OracleCommand command = db.CreateCommand(SQL_SELECT_EMPLOYEES);
            OracleDataReader reader = db.Select(command);

            Collection<User> users = Read(reader);

            Dictionary<int, User> usersDict = new Dictionary<int, User>();

            foreach (User userItem in users)
            {
                if (!usersDict.ContainsKey(userItem.IdUser))
                {
                    usersDict.Add(userItem.IdUser, userItem);
                }
            }

            Collection<User> uniqueUsers = new Collection<User>();

            foreach (KeyValuePair<int, User> userDictItem in usersDict)
            {
                User user = userDictItem.Value;
                Collection<Reservation> reservations = new Collection<Reservation>();

                foreach (User userItem in users)
                {

                    if (userItem.IdUser == userDictItem.Key && userItem.ReservationTemp != null)
                    {
                        Reservation reservation = new Reservation();
                        reservation.IdReservation = userItem.ReservationTemp.IdReservation;
                        reservation.ReservationDate = userItem.ReservationTemp.ReservationDate;
                        reservation.EndReservation = userItem.ReservationTemp.EndReservation;
                        reservation.Description = userItem.ReservationTemp.Description;

                        reservations.Add(reservation);
                    }

                }

                user.Reservations = reservations;
                user.ReservationTemp = null;

                uniqueUsers.Add(user);
            }

            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return uniqueUsers;
        }

        public User select(int Id, Database pDb = null)
        {
            Database db = new Database();
            db.Connect();
            OracleCommand command = db.CreateCommand(SQL_SELECT_ID);

            command.Parameters.AddWithValue(":idUser", Id);
            OracleDataReader reader = db.Select(command);

            Collection<User> users = Read(reader);
            User user = null;

            if (users.Count == 1)
            {
                user = users[0];
                if (user.ReservationTemp != null) {
                    Collection<Reservation> reservations = new Collection<Reservation>();
                    reservations.Add(user.ReservationTemp);
                    user.Reservations = reservations;
                    user.ReservationTemp = null;
                }
            } 
            else if (users.Count > 1)
            {
                user = users[0];

                Collection<Reservation> reservations = new Collection<Reservation>();

                foreach (User userItem in users) {
                    reservations.Add(userItem.ReservationTemp);
                }

                user.Reservations = reservations;
                user.ReservationTemp = null;

            }



            reader.Close();
            db.Close();
            return user;
        }

 
        public int delete(int id, Database pDb = null)
        {
            Database db = new Database();
            db.Connect();
            OracleCommand command = db.CreateCommand(SQL_DELETE_ID);

            command.Parameters.AddWithValue(":idUser", id);
            int ret = db.ExecuteNonQuery(command);

            db.Close();
            return ret;
        }

        public int createIdUser(Database pDb = null)
        {
            Database db = new Database();
            db.Connect();
            OracleCommand command = db.CreateCommand(SQL_SELECT_LAST_ID_USER);
            OracleDataReader reader = db.Select(command);

            int lastId = 0;

            Object lastIdScalar = db.CreateCommand(SQL_SELECT_LAST_ID_USER).ExecuteScalar();

            if (lastIdScalar != null)
            {
                lastId = Convert.ToInt32(lastIdScalar);
            }
           
            db.Close();

            return ++lastId;
        }

       
         // 2.5 Existencia uzivatela

        public bool UserExists(string v_login)
        { 
            bool result = false;

            Database db = new Database();           
            db.Connect();
            
            OracleCommand command = db.CreateCommand("SELECT UserExists(:v_login) AS RESULT from dual");
            command.Parameters.AddWithValue(":login", v_login);
           
            OracleDataReader reader = db.Select(command);

            while (reader.Read())
            {
                result = bool.Parse(reader.GetString(reader.GetOrdinal("RESULT")));
            }

            reader.Close();
                    
            return result;
        }


        private static void PrepareCommand(OracleCommand command, User user)
        {
            command.BindByName = true;
            command.Parameters.AddWithValue(":idUser", user.IdUser);
            command.Parameters.AddWithValue(":email", user.email);
            command.Parameters.AddWithValue(":idrole", user.IdRole);
            command.Parameters.AddWithValue(":login", user.Login);
            command.Parameters.AddWithValue(":password", user.Password);
            command.Parameters.AddWithValue(":phonenumber", user.PhoneNumber == null ? DBNull.Value : (object)user.PhoneNumber);
            command.Parameters.AddWithValue(":name", user.Name == null ? DBNull.Value : (object)user.Name);
            command.Parameters.AddWithValue(":surname", user.Surname);
            command.Parameters.AddWithValue(":idCardNo", user.IdCard_NO == null ? DBNull.Value : (object)user.IdCard_NO);
            command.Parameters.AddWithValue(":cancelationcount", user.CancelationCount);
        }

        private static Collection<User> Read(OracleDataReader reader)
        {
            Collection<User> users = new Collection<User>();

            while (reader.Read())
            {
                User user = new User();
                user.IdUser = reader.GetInt32(reader.GetOrdinal("IDUSER"));
                user.email = reader.GetString(reader.GetOrdinal("EMAIL"));
                user.IdRole = reader.GetInt32(reader.GetOrdinal("ROLES_IDROLE"));
                user.Role = new Role(user.IdRole, reader.GetString(reader.GetOrdinal("ROLENAME")));
                user.Login = reader.GetString(reader.GetOrdinal("LOGIN"));
                user.PhoneNumber = reader.GetString(reader.GetOrdinal("PHONENUMBER"));
                user.Name = reader.GetString(reader.GetOrdinal("NAME"));
                user.Surname = reader.GetString(reader.GetOrdinal("SURNAME"));
                user.IdCard_NO = reader.GetString(reader.GetOrdinal("IDCARDNO"));
                user.CancelationCount = reader.GetInt32(reader.GetOrdinal("CANCELATIONCOUNT"));

                if (!reader.IsDBNull(reader.GetOrdinal("idReservation"))) {
                    Reservation reservation = new Reservation();
                    reservation.IdReservation = reader.GetInt32(reader.GetOrdinal("idReservation"));
                    reservation.ReservationDate = reader.GetDateTime(reader.GetOrdinal("reservationDate"));
                    reservation.EndReservation = reader.GetDateTime(reader.GetOrdinal("endReservation"));
                    reservation.Description = reader.GetString(reader.GetOrdinal("description"));

                    user.ReservationTemp = reservation;
                }
                /* if (!reader.IsDBNull(++i))
                {
                    User.LastVisit = reader.GetDateTime(i);
                }
                User.Type = reader.GetString(++i);*/

                users.Add(user);
            }
            return users;
        }
    }
}