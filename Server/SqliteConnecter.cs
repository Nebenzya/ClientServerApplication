using Dapper;
using System.Configuration;
using System.Data;
using System.Data.SQLite;

namespace Server
{
    public class SqliteConnecter
    {
        public static List<Student> Load()
        {
            using (IDbConnection dbConnection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = dbConnection.Query<Student>("SELECT * FROM Students", new DynamicParameters());
                return output.ToList();
            }
        }
        public static void Save(Student student)
        {
            using (IDbConnection dbConnection = new SQLiteConnection(LoadConnectionString()))
            {
                dbConnection.Execute(
                    "INSERT INTO Students (FirstName,LastName,BirthYear,Course) VALUES (@FirstName,@LastName,@BirthYear,@Course)", student);
            }
        }

        private static string LoadConnectionString(string name = "MainDB")
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
