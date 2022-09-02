using Autofac;
using Autofac.Core;
using Nop.Core.Configuration;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using Nop.Plugin.MediHub.Slider.Data;
using Nop.Plugin.MediHub.Slider.Domains;
using Nop.Plugin.MediHub.Slider.Services;
using Nop.Web.Framework.Infrastructure.Extensions;

namespace Nop.Plugin.MediHub.Slider.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        private static string CONTEXT_NAME = "Nop.Plugin.MediHub.Slider_object_context";

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            builder.RegisterType<CustomService>().As<ICustomService>().InstancePerLifetimeScope();


            //data context
            builder.RegisterPluginDataContext<CustomObjectContext>(CONTEXT_NAME);

            //override required repository with our custom context
            builder.RegisterType<EfRepository<CustomTable>>()
                .As<IRepository<CustomTable>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
                .InstancePerLifetimeScope();
        }

        public int Order => 1;
    }
}
