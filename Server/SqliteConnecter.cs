using System.Configuration;
using System.Data;
using System.Data.SQLite;
using Dapper;

namespace Server
{
    /// <summary>
    /// Чтение и сохрание информации в локальной базе данных
    /// </summary>
    public class SqliteConnecter
    {
        /// <summary>
        /// Вывовидит всю информацию из базы данных
        /// </summary>
        public static List<Student> Load()
        {
            using IDbConnection dbConnection = new SQLiteConnection(LoadConnectionString());
            var output = dbConnection.Query<Student>("SELECT * FROM Students", new DynamicParameters());
            return output.ToList();
        }

        /// <summary>
        /// Добавляет запись в базу данных
        /// </summary>
        public static void Save(Student student)
        {
            using IDbConnection dbConnection = new SQLiteConnection(LoadConnectionString());
            dbConnection.Execute(
                "INSERT INTO Students (FirstName,LastName,BirthYear,Course) VALUES (@FirstName,@LastName,@BirthYear,@Course)", student);
        }

        /// <summary>
        /// Использует конфигурацию App.config для подключение к базе данных
        /// </summary>
        private static string LoadConnectionString(string name = "MainDB")
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
