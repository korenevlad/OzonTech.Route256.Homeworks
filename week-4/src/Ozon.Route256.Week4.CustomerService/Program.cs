namespace Ozon.Route256.Week4.CustomerService;

public class Program
{
    public static void Main(string[] args)
    {
        Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(builder => builder
                .UseStartup<Startup>())
            .Build()
            .Run();
    }
}