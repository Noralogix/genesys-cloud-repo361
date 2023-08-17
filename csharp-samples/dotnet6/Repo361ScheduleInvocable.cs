using Coravel.Invocable;

namespace dotnet6
{
    public class Repo361ScheduleInvocable : IInvocable
    {
        private readonly Repo361Api _api;
        private readonly Repo361MySQLService _mysql;

        public Repo361ScheduleInvocable(Repo361Api api, Repo361MySQLService mysql)
        {
            _api = api;
            _mysql = mysql;
        }

        public async Task Invoke()
        {
            var dt = DateTime.Now.AddDays(-2);
            await foreach (var file in _api.DownloadAsync(dt))
            {
                await _mysql.ImportAsync(file);
            }

        }
    }
}
