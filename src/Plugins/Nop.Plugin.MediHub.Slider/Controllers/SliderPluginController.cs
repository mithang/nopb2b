using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Nop.Plugin.MediHub.Slider.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]//Mặt định nó sẽ lấy trong Area Admin.
    public class SliderPluginController : BasePluginController
    {
        public ActionResult Configure()
        {
            return View("~/Plugins/Nop.Plugin.MediHub.Slider/Views/SliderPlugin/Configure.cshtml");
        }
    }
}
