using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAccess
{
    public class DataAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new EntityFrameworkRepository<AutoPartEntities>(new AutoPartEntities())).As(typeof(IRepository));
            base.Load(builder);

            builder.Register(c => new FTP()).As(typeof(IFTP));
            base.Load(builder);

            builder.Register(c => new Logger()).As(typeof(ILogger));
            base.Load(builder);
        }
    }
}