using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadExercise1
{
    class Program
    {
        static TestDbContext db = new TestDbContext();
        static void Main(string[] args)
        {
            //TestDbContext db = new TestDbContext();

            //foreach(Club c in db.Clubs )
            //{
            //    Console.WriteLine(c.Info);
            //}
            //foreach (Student s in db.Students)
            //{
            //    Console.WriteLine(s.FirstName  + " " +  s.SecondName);
            //}

            db.AddEvent("ITS FC", new ClubEvent
            {
                attendees = new List<Member>(),
                Venue = "The Arena",
                EventName = "Indoor soccer",
                StartDateTime = new DateTime(2016, 12, 12, 16, 00, 00, DateTimeKind.Local),
                EndDateTime = new DateTime(2016,12, 12, 18, 00, 00, DateTimeKind.Local),
            });
            db.AddEvent("ITS FC", new ClubEvent
            {
                attendees = new List<Member>(),
                Venue = "IT Astro Soccer ",
                EventName = "Trials",
                StartDateTime = new DateTime(2016,12, 09,  13, 00, 00, DateTimeKind.Local),
                EndDateTime = new DateTime(2016, 12, 09,  15, 00, 00, DateTimeKind.Local),
            });
            
            string error;
            if (!db.addMember("ITS FC", new Student { StudentId =Guid.NewGuid(),
                                        FirstName ="Bill", SecondName = "Bloggs" }, out error))
            {
                Console.WriteLine(error);
            }
            else Console.WriteLine(error);




            foreach (ClubEvent c in Question2(new DateTime(2016, 12, 09, 13, 00, 00), new DateTime(2016, 12, 09, 15, 00, 00)))
            {
                Console.WriteLine("Question 2 {0} Event on at {1} sarts {2} and finishes at {3}", c.EventName, c.Venue, c.StartDateTime, c.EndDateTime);
            }
            foreach (ClubEvent c in Question3("ITS FC"))
            {
                Console.WriteLine("Question 3 {0} Event on at {1} sarts {2} and finishes at {3}", c.EventName, c.Venue, c.StartDateTime, c.EndDateTime);
            }

            var members = Question4("ITS FC");
            foreach (var item in members)
            {
                Console.WriteLine(" {0} is a member of club {1}", item.Name, "ITS FC");
            }

            Console.ReadKey();
            
             
        }
        static List<Club> Question1()
        {
            return db.Clubs;
        }

        // This Question does not have a very standard solution so I've done it
        static public List<ClubEvent> Question2(DateTime start, DateTime end )
        {
            // Get the events across all the clubs
            List<ClubEvent> AllClubEvents = 
                (List<ClubEvent>)db.Clubs.SelectMany(c =>
                    c.ClubEvents).ToList();
            // Get those events in Range
            return AllClubEvents
            .Where(e => e.StartDateTime >= start &&
            e.EndDateTime <= end).ToList();

        }

        static List<ClubEvent> Question3(string ClubName)
        {
            //var q3 = (from club in db.Clubs
            //          where club.ClubName == ClubName
            //          select club.ClubEvents
            //          ).ToList();

            return db.Clubs.Where(c => c.ClubName == ClubName)
                           .SelectMany(c => c.ClubEvents).ToList();
        }

        static public dynamic Question4(string clubName) {

            List<Member> clubMembers = db.Clubs
                .FirstOrDefault(c => c.ClubName == clubName)
                .ClubMembers;

            var NamedMembers = (from clubmember in clubMembers
                       join student in db.Students 
                       on clubmember.StudentID equals student.StudentId
                       select new {Name = student.FirstName + " " + student.SecondName })
                       .ToList();

            return NamedMembers;
        }

    }
}
