using Nop.Core.Configuration;

namespace Nop.Plugin.Payment.VnPay
{
    public class MyCustomPluginSettings : ISettings
    {
        public bool UseSandbox { get; set; }
        public string Message { get; set; } 
    }
}