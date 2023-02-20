using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.SQLite;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Document_Manager_Project.Models
{
    public class DataBase_Connetion : DbContext
    {
        public DataBase_Connetion() : base("Database_Docs") { }
        public DbSet<User_Details> Users { get; set; }

        //public DbSet<User_Details_Aprroval> Users { get; set; }
        public DbSet<Project_Details> ProjectDetails { get; set; }
        public DbSet<Approval_Details> Approval_Details { get; set; }
        public DbSet<PI_Details> PIDetails { get; set; }

        public DbSet<PI_Details_Project> PIProjectDetails { get; set; }

        public DbSet<Collaborators_Details>CollaboratorDetails { get; set; }




        //New Implementation

        public DbSet<Project_Details_Table>project_Details_Table { get; set; }
        public DbSet<Researcher_Table>researcher_Table { get; set; }

        public DbSet<Project_Researcher_Table> project_Researcher_Tables { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Database.SetInitializer<demoEntities>(null);
            modelBuilder.Entity<User_Details>().ToTable("Users");
            modelBuilder.Entity<Project_Details>().ToTable("ProjectDetails");
            modelBuilder.Entity<Approval_Details>().ToTable("Approval");
            modelBuilder.Entity<PI_Details_Project>().ToTable("PI_Project");
            modelBuilder.Entity<Collaborators_Details>().ToTable("Collaborators");


            //New Implementation
            modelBuilder.Entity<Project_Details_Table>().ToTable("Project_Details_Table");
            modelBuilder.Entity<Researcher_Table>().ToTable("Researcher_Table");
            modelBuilder.Entity<Project_Researcher_Table>().ToTable("Project_Researcher_Table");

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);


        }

        static SQLiteConnection CreateConnection()
        {

            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            sqlite_conn = new SQLiteConnection("Data Source= database.db; Version = 3; New = True; Compress = True; ");
         // Open the connection:
         try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {

            }
            return sqlite_conn;
        }

        static void CloseConnection(SQLiteConnection conn)
        {
            conn.Close();
        }
    }
}
