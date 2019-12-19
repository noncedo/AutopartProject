using Autofac;

namespace Service
{
    public class ServiceModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new ILHService()).As(typeof(IILHService));
            base.Load(builder);

            builder.Register(c => new I02UService()).As(typeof(II02UService));
            base.Load(builder);

            builder.Register(c => new DMService()).As(typeof(IDMService));
            base.Load(builder);

            builder.Register(c => new I02Service()).As(typeof(II02Service));
            base.Load(builder);
        }
    }
}
