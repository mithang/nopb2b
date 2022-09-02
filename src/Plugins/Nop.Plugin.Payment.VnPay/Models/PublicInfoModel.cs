using Nop.Web.Framework.Models;

namespace Nop.Plugin.Payment.VnPay.Models
{
    public class PublicInfoModel : BaseNopModel
    {
        public bool UseSandbox { get; set; }
        public string Message { get; set; }
    }
}