using Autofac;
using DataAccess;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            System.Console.ReadLine();
        }

        public static void DoStuff()
        {
            // Create the scope, resolve your Interfaces,
            // use it, then dispose of the scope.
            using (var scope = Container.BeginLifetimeScope())
            {
                var service = scope.Resolve<II02Service>();
                var repository = scope.Resolve<IRepository>();
                var logger = scope.Resolve<ILogger>();
                service.RunProcess(repository, logger);
            }
        }
    }
}
