using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Services.Authentication.External;

namespace Nop.Plugin.ExternalAuth.Database.Infrastructure
{
    /// <summary>
    /// Represents registrar of Database authentication service
    /// </summary>
    public class DatabaseAuthenticationRegistrar : IExternalAuthenticationRegistrar
    {
        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="builder">Authentication builder</param>
        public void Configure(AuthenticationBuilder builder)
        {
            builder.AddCookie(DatabaseAuthenticationDefaults.SystemName, options => {

            });
            //builder.AddFacebook(FacebookDefaults.AuthenticationScheme, options =>
            //{
            //    //set credentials
            //    var settings = EngineContext.Current.Resolve<DatabaseExternalAuthSettings>();
            //    options.AppId = settings.ClientKeyIdentifier;
            //    options.AppSecret = settings.ClientSecret;

            //    //store access and refresh tokens for the further usage
            //    options.SaveTokens = true;

            //    //set custom events handlers
            //    options.Events = new OAuthEvents
            //    {
            //        //in case of error, redirect the user to the specified URL
            //        OnRemoteFailure = context =>
            //        {
            //            context.HandleResponse();

            //            var errorUrl = context.Properties.GetString(DatabaseAuthenticationDefaults.ErrorCallback);
            //            context.Response.Redirect(errorUrl);

            //            return Task.FromResult(0);
            //        }
            //    };
            //});
        }
    }
}