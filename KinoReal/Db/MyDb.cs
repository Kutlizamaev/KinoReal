using KinoReal.Models;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KinoReal.DB
{
    public class MyDb
    {
        private static string ConnectionString =
            "User ID=postgres; Server=localhost; port=5432; Database=KinoReal; Password=Wopland1; Pooling=true;";

        private static NpgsqlConnection Connection = new NpgsqlConnection(ConnectionString);

        private static string UserProperties = "id, username, email, password";
        private static string ProfileProperties = "id, fullname, city, birthday";

        private static string UserTable = "Users";
        private static string ProfileTable = "Profile";

        public static async Task Add(User user)
        {
            await Connection.OpenAsync();

            var userValues = GetValues(user);
            var comm = $"INSERT INTO \"{UserTable}\" ({UserProperties}) VALUES ({userValues});";
            var cmd = new NpgsqlCommand(comm, Connection);
            await cmd.ExecuteNonQueryAsync();

            await Connection.CloseAsync();
        }

        public static async Task Add(Profile profile)
        {
            await Connection.OpenAsync();

            var profileValues = GetValues(profile);
            var comm = $"INSERT INTO \"{ProfileTable}\" ({ProfileProperties}) VALUES ({profileValues});";
            var cmd = new NpgsqlCommand(comm, Connection);
            await cmd.ExecuteNonQueryAsync();

            await Connection.CloseAsync();
        }

        public static async Task AddRange(IEnumerable<User> users)
        {
            await Connection.OpenAsync();

            foreach (var user in users)
            {
                var userValues = GetValues(user);
                var comm = $"INSERT INTO \"{UserTable}\" ({UserProperties}) VALUES ({userValues});";
                var cmd = new NpgsqlCommand(comm, Connection);
                await cmd.ExecuteNonQueryAsync();
            }

            await Connection.CloseAsync();
        }

        public static async Task<List<User>> GetAllUsers()
        {
            await Connection.OpenAsync();

            var users = new List<User>();

            var cmd = new NpgsqlCommand($"SELECT * FROM \"{UserTable}\"", Connection);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                users.Add(new User()
                {
                    Id = reader.GetGuid(0),
                    Username = reader.GetString(1),
                    Email = reader.GetString(2),
                    Password = reader.GetString(3),
                });
            }
            await Connection.CloseAsync();

            return users;
        }

        public static async Task<List<Profile>> GetAllProfiles()
        {
            await Connection.OpenAsync();

            var profiles = new List<Profile>();

            var cmd = new NpgsqlCommand($"SELECT * FROM \"{ProfileTable}\"", Connection);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                profiles.Add(new Profile()
                {
                    Id = reader.GetGuid(0),
                    Fullname = reader.GetString(1),
                    City = reader.GetString(2),
                    Birthday = reader.GetString(3)
                });
            }
            await Connection.CloseAsync();

            return profiles;
        }

        public static async Task Update(Profile profile)
        {
            await Connection.OpenAsync();

            var profileValues = GetValues(profile);
            var comm = $"UPDATE \"{ProfileTable}\" SET ({ProfileProperties}) = ({profileValues}) WHERE id='{profile.Id}'";
            var cmd = new NpgsqlCommand(comm, Connection);
            await cmd.ExecuteNonQueryAsync();

            await Connection.CloseAsync();
        }

        private static string GetValues(User user) =>
            $"'{user.Id}', '{user.Username}', '{user.Email}', '{user.Password}'";
        private static string GetValues(Profile profile) =>
            $"'{profile.Id}', '{profile.Fullname}', '{profile.City}', '{profile.Birthday}'";
    }
}
