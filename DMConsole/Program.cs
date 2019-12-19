using Autofac;
using DataAccess;
using Service;

namespace DMConsole
{
    class Program
    {
        private static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new ServiceModule());
            builder.RegisterModule(new DataAccessModule());
            Container = builder.Build();

            // The DoStuff method is where we'll make use
            // of our dependency injection. 
            DoStuff();
            System.Console.ReadLine();
        }

        public static void DoStuff()
        {
            // Create the scope, resolve your Interfaces,
            // use it, then dispose of the scope.
            using (var scope = Container.BeginLifetimeScope())
            {
                var dmService = scope.Resolve<IDMService>();
                var repository = scope.Resolve<IRepository>();
                var ftp = scope.Resolve<IFTP>();
                var logger = scope.Resolve<ILogger>();

                dmService.RunProcess(repository, ftp, logger);
            }
        }
    }
}
