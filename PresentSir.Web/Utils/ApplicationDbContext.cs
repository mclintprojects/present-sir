using LiteDB;
using PresentSir.Web.Models;

namespace PresentSir.Web.Utils
{
    public class ApplicationDbContext
    {
        private static ApplicationDbContext singleton;
        private static object lockObject = new object();

        public static ApplicationDbContext Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (singleton == null)
                        singleton = new ApplicationDbContext();

                    return singleton;
                }
            }
        }

        private LiteDatabase database;

        public LiteDatabase Database => database;

        #region DBModels

        public LiteCollection<Student> Students => database.GetCollection<Student>("student");
        public LiteCollection<Teacher> Teachers => database.GetCollection<Teacher>("teacher");
        public LiteCollection<ApplicationUser> Users => database.GetCollection<ApplicationUser>("user");
        public LiteCollection<Attendance> Attendance => database.GetCollection<Attendance>("attendance");
        public LiteCollection<Institution> Institutions => database.GetCollection<Institution>("institution");
        public LiteCollection<Class> Classes => database.GetCollection<Class>("class");

        #endregion DBModels

        private ApplicationDbContext()
        {
        }

        public void SetDatabase(LiteDatabase database)
        {
            this.database = database;

            EnsureIndexes();
        }

        private void EnsureIndexes()
        {
            // Indexes for attendance
            Attendance.EnsureIndex(x => x.ClassId);
            Attendance.EnsureIndex(x => x.Student);

            // Indexes for students
            Students.EnsureIndex(x => x.User);
            Students.EnsureIndex(x => x.User.IndexNumber);
        }
    }
}