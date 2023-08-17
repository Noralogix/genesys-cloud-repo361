using Microsoft.Extensions.Options;

namespace dotnet6
{
    public static class Repo361Init
    {
        public static void AddRepo361(this IServiceCollection sc, IConfigurationSection config)
        {
            sc.AddTransient<Repo361Api>();
            sc.AddOptions<Repo361Settings>().Bind(config);
            sc.AddHttpClient(Repo361Api.Repo361, (sp, client) =>
            {
                var opt = sp.GetService<IOptions<Repo361Settings>>();
                ArgumentNullException.ThrowIfNull(opt);
                ArgumentNullException.ThrowIfNull(opt.Value.URL);
                client.Timeout = TimeSpan.FromMinutes(2);
                client.BaseAddress = new Uri(opt.Value.URL);
            });
        }
    }
}
