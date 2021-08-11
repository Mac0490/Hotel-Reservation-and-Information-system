using System;
using AuctionSystem.ORM;
using AuctionSystem.ORM.Oracle;
using System.Collections.ObjectModel;

namespace AuctionSystem
{
    class Program
    {
        static void Main(string[] args)
        {

            ReservationsTable reservationTable = new ReservationsTable();

            UsersTable usersTable = new  UsersTable(); 

            RoomTable roomTable = new RoomTable();

            // PROCEDURY Z FUNKCNEJ ANALYZY

            /********  3.1 Vytvorenie rezervacie - TRANSAKCIA   *******/

            
            /* Reservation reservation = new Reservation();

             reservation.IdReservation = reservationTable.createIdReservation();
             reservation.ReservationDate = new DateTime(2020,7,1);
             reservation.EndReservation = new DateTime(2020,7,10);
             reservation.IdUser = 51;
             reservation.IdRoom = 322;
             reservation.Description = "chce izbu 322 - vyhlad na jazero";   
             reservationTable.CreateNewReservation(reservation,"789,53");

             Console.WriteLine("3.1 Vytvorenie rezervacie - ExecuteProcedure: CreateReservation");
             Console.ReadLine();*/



            /******** 3.5 Zrušenie rezervácií a odstránenie platieb  - TRANSAKCIA  *******/

            
          /* DateTime dateFrom = new DateTime(2020,6,30);
             DateTime dateTo = new DateTime(2020,7,11);           

             reservationTable.CancelationOfReserAndPayments(dateFrom, dateTo);

             Console.WriteLine("3.5 Zrušenie rezervácií a odstránenie platieb - ExecuteProcedure: CancelationOfReservationAndPayments");
            Console.ReadLine();*/

            /******** 3.4 Zrušenie budúcich rezervácií užívateľa - TRANSAKCIA  *******/

            /*  int IdUser = 51;

              reservationTable.CancelationOfFutureReservations(IdUser);

              Console.WriteLine("3.4 Zrušenie budúcich rezervácií užívateľa - ExecuteProcedure: CancelationOfFutureReservations ");*/

            //FUNKCIE Z FUNKCNEJ ANALYZY

            /********  3.6 Overenie dostupnosti *******/
            
           /*  bool result = reservationTable.VerificationOfAblity(new DateTime(2020,12,3),new DateTime(2020,12,5),323);


                 Console.WriteLine("3.6 Overenie dostupnosti - Ak je dostupna tak TRUE, ak nie tak FALSE: " + result);
                 Console.ReadLine();*/

            
            /********  2.5 Existencia užívateľa *******/


            /* bool result = usersTable.UserExists("MAC0000"); //MAC0490 EXISTUJE

             Console.WriteLine("2.5 Existencia užívateľa - Ak Existuje tak TRUE, ak neexistuje tak FALSE: " + result);
             Console.ReadLine();*/

            /******** 3.8 Report počtu budúcich a zaplatených rezervácii pre typy izieb *******/

            
          /*
            Collection<ReportReservation> reportReservation = reservationTable.selectReportReservations(10);

            foreach (ReportReservation item in reportReservation)
                
            Console.WriteLine("RoomNumber: "+ item.RoomNumber + " " + "NumberFutureReservations: " + item.Report1 + "  " +  "NumberFutureReservationsAndTheirPayments" + item.Report2);

            
            Console.ReadLine();*/



            /********  *****  *******/
            /********  USERS   *******/
            /********  *****  *******/

            //jednoduche selekt, updat, insert a delete


            //Database db = new Database();
            //db.Connect();



            /********  USERS - SELECTS *******/

            // 1.1 Určenie role (Ziskanie zamestnanca/zakaznika podla id a priradenie role )

            /* 
             Collection<User> users = usersTable.select();


             User user = usersTable.select(23);
             Console.WriteLine("UserLogin: " + user.Login + " Rola: " + user.Role.RoolName);
             Console.ReadLine();*/


            /********  USERS - INSERT *******/

            //2.1 Pridanie nového užívateľa

            /* User user = new User();
             user.email = "mmcejva.l@mail.sk";
             user.IdRole = 1;
             user.Login = "M45664";
             user.Password = "UDGFjMN";
             user.PhoneNumber = "0924414090";
             user.Name = "MARIKA";
             user.Surname = "Mayurkova";
             user.IdCard_NO = "LA5455";
             user.CancelationCount = 0;
           //  usersTable.insert(user);

           int resultInsert = 0;
            resultInsert = usersTable.insert(user);

            if(resultInsert == 1) {
                Console.WriteLine("Záznam bol úspešne pridaný.");
            }
            */



            /********  USERS - DELETE *******/

            //2.3 Vymazanie užívateľa

            //usersTable.delete(791);

            /* int resultDelete = 0;
             resultDelete= usersTable.delete(791);

             if (resultDelete == 1)
             {

                 Console.WriteLine("Záznam bol úspešne vymazaný.");
             }
             Console.ReadLine();/*



             /********  USERS - UPDATE *******/

            //2.2 Úprava užívateľa

            /*  User user = new User();

              user.IdUser = 34;
              user.email = "miroslava1234@gmail.sk";
              user.IdRole = 1;
              user.Login = "MAC0001KK";
              user.Password = "ZZUDGFN";
              user.PhoneNumber = "0900214080";
              user.Name = "Mirka";
              user.Surname = "MAcejkova";
              user.IdCard_NO = "BNKF123";
              user.CancelationCount = 2;

              //usersTable.update(user);

              int resultUpdate= 0;
              resultUpdate = usersTable.update(user);

              if (resultUpdate == 1)
              {

                  Console.WriteLine("Záznam bol úspešne zmenený.");
              }
              Console.ReadLine();*/

            //2.4 Výber všetkých zamestnancov

            /*   Collection<User> users = usersTable.selectEmployees();

               foreach (User user in users ) 
               {
               Console.WriteLine("idUser: " + user.IdUser);
               Console.WriteLine("Role: " + user.Role.RoolName); 
               }
               Console.ReadLine();*/

            //2.6 Výber zamestnanca podľa zvoleného atribútu

            /* Collection<User> users = usersTable.select();

             User user = usersTable.select(23);
             Console.WriteLine("User:ID USER " + user.IdUser);
             Console.ReadLine(); */

            /********  *****  *******/
            /********  ROOMS  *******/
            /********  *****  *******/


            /********  ROOMS - SELECTS   *******/

            // 4.1 Výber izby

            //   Collection<Room> rooms = roomTable.select();

            //Room room = roomTable.select(325);
            //  Console.WriteLine();


            /********  ROOMS - INSERT  *******/

            //4.2 Vloženie izby

            /* Room room = new Room();
             room.RoomNumber = 457;
             room.IdRoomType = 30;
             room.Floor = 3;
             room.Description = "Nova izba pozor";

            // roomTable.insert(room);

             int resultInsert = 0;
             resultInsert = roomTable.insert(room);

             if (resultInsert == 1)
             {
                 Console.WriteLine("Záznam bol úspešne pridaný.");
             }
             */
            /********  ROOMS - UPDATE  *******/

            //4.3 Úprava izby

            /*
            Room room = new Room();

            room.IdRoom = 326;
            room.RoomNumber = 22;
            room.IdRoomType = 10;
            room.Floor = 9;
            room.Description = "Nova izba 123";

            int resUpdate = roomTable.update(room);
            */

            //  Console.WriteLine("update ");


            /********  ROOMS - DELETE  *******/

            //4.4 Vymazanie izby
            //    roomTable.delete(326);




            /********  *****          *******/
            /********  RESERVATIONS  *******/
            /********  *****        *******/



            /********  RESERVATIONS - SELECT *******/
            //3.10 Výber rezervácie

            /* Collection<Reservation> reservations = reservationTable.select();
             Reservation reservation = reservationTable.select(12345);

             Console.WriteLine("IdReservation: " + reservation.IdReservation);
             Console.WriteLine("Záznam bol vybraný.");
             Console.ReadLine();*/

            //Reservation reservation = reservationTable.select(5);

            /********  RESERVATIONS - INSERT *******/

            /*
            Reservation reservation = new Reservation();
              reservation.ReservationDate = new DateTime(2020,6,1);
              reservation.EndReservation = new DateTime(2020,6,11);
              reservation.IdRoom = 325;
              reservation.IdUser = 790;
              reservation.Description= "pozor izba nieje cista";

             // reservationTable.insert(reservation);

            int resultInsert = 0;
            resultInsert = reservationTable.insert(reservation);

            if (resultInsert == 1)
            {
                Console.WriteLine("Záznam bol úspešne pridaný.");
            }*/


            /********  RESERVATIONS - UPDATE *******/

            //3.2 Úprava rezervácie 

            /*     Reservation reservation = new Reservation();

                 reservation.IdReservation = 5;
                 reservation.ReservationDate = new DateTime(2020,4,5);
                 reservation.EndReservation = new DateTime(2020, 5, 5);
                 reservation.IdRoom = 325;
                 reservation.IdUser = 790;
                 reservation.Description= " ";

                 reservationTable.update(reservation);
                 Console.WriteLine("Úprava rezervácie.");
                 Console.ReadLine();*/

            /********  RESERVATIONS - DELETE*******/

            //3.7 Zrušenie rezervácie

            /* reservationTable.delete(12368);
             Console.WriteLine("Vymazanie rezervácie.");
             Console.ReadLine();*/


            //db.Close();

            //3.3 Výber všetkých rezervácií z histórie

            /*Collection<Reservation> reservations = reservationTable.selectHistorical();
            foreach (Reservation reservation in reservations)
            {
                Console.WriteLine(" Výber rezervácie z histórie - ID reservacie: " + reservation.IdReservation);
            }
            Console.ReadLine();*/

            // 3.9 Výber všetkých nadchádzajúcich rezervácií

            /*  Collection<Reservation> reservations = reservationTable.selectPresent();
              foreach (Reservation reservation in reservations)
              {
                  Console.WriteLine("Výber všetkých nadchádzajúcich rezervácií - ID reservacie:  " + reservation.IdReservation);
              }

             Console.ReadLine();
             */
        }

    }
}
