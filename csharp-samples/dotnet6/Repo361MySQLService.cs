using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Text;

namespace dotnet6
{
    public class Repo361MySQLService : IDisposable
    {
        private readonly Repo361Settings _settings;
        private readonly MySqlConnection _connection;

        public Repo361MySQLService(IOptions<Repo361Settings> opts)
        {
            _settings = opts.Value;
            ArgumentNullException.ThrowIfNull(_settings.MySqlConnection);
            _connection = new MySqlConnection(_settings.MySqlConnection);
        }

        public async Task<int> ImportAsync(LocalCsvFile file)
        {
            if (file.name != "analytics_conversations") return 0;
            if (_connection.State != System.Data.ConnectionState.Open) await _connection.OpenAsync();
            var sCommand = new StringBuilder("INSERT INTO User (FirstName, LastName) VALUES ");
            var sqlCmd = new MySqlCommand($"LOAD DATA LOCAL INFILE '{file.path}' INTO TABLE {file.name} FIELDS TERMINATED BY ',' IGNORE 1 LINES;", _connection);
            return await sqlCmd.ExecuteNonQueryAsync();            
        }

        public void Dispose()
        {
            try
            {
                _connection.Dispose();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
