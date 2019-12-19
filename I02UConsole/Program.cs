using Autofac;
using DataAccess;
using Service;

namespace I02Console
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
        }

        public static void DoStuff()
        {
            // Create the scope, resolve your Interfaces,
            // use it, then dispose of the scope.
            using (var scope = Container.BeginLifetimeScope())
            {
                var io2uService = scope.Resolve<II02UService>();
                var repository = scope.Resolve<IRepository>();
                var ftp = scope.Resolve<IFTP>();
                var logger = scope.Resolve<ILogger>();

                io2uService.RunProcess(repository, ftp, logger);
            }
        }
    }
}
