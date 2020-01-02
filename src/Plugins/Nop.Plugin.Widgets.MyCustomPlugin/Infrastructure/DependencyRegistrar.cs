using Autofac;
using Autofac.Core;
using Nop.Core.Configuration;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure.Extensions;

namespace Nop.Plugin.Widgets.MyCustomPlugin.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        private static string CONTEXT_NAME = "Nop.Plugin.Widgets.MyCustomPlugin_object_context";


        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            //builder.RegisterType<PluginsInfo>().As<IPluginsInfo>().InstancePerLifetimeScope();


            ////data context
            //builder.RegisterPluginDataContext<CustomObjectContext>(CONTEXT_NAME);

            ////override required repository with our custom context
            //builder.RegisterType<EfRepository<CustomTable>>()
            //    .As<IRepository<CustomTable>>()
            //    .WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
            //    .InstancePerLifetimeScope();
        }

        public int Order => 1;
    }
}
