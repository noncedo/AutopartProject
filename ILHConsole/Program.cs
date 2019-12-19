using Autofac;
using DataAccess;
using Service;

namespace ILHConsole
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
                var ilhService = scope.Resolve<IILHService>();
                var repository = scope.Resolve<IRepository>();
                //ilhService.RunProcess(repository);
            }
        }
    }
}
