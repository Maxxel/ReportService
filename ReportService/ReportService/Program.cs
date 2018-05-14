using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ReportService
{
    public class Program
    {
        public static void Main(string[] args)
        {
			BuildWebHost(args).Run();	        
		}

		public static void StartServiceAsync(string[] args)
	    {
		    Host = BuildWebHost(args);
		    Host.Run();
	    }

	    public static void StopServiceAsync()
	    {
		    Host.StopAsync();
	    }

	    private static IWebHost Host { get; set; }

		public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
