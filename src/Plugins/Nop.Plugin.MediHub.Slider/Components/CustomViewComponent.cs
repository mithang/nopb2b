using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.MediHub.Slider.Domains;
using Nop.Plugin.MediHub.Slider.Services;
using Nop.Web.Framework.Components;
using System;

namespace Nop.Plugin.MediHub.Slider.Components
{
    [ViewComponent(Name = "Custom")]
    public class CustomViewComponent : NopViewComponent
    {
        private readonly ICustomService _CustomService;
        private readonly IWorkContext _workContext;
        public CustomViewComponent(IWorkContext workContext,
        ICustomService CustomService)
        {
            _workContext = workContext;
            _CustomService = CustomService;
        }
        public IViewComponentResult Invoke()
        {
          
          
                //Setup the Custom to save
                var record = new CustomTable();
                //record.Id = CustomId;
                record.CustomName = "DEMO";
               
                _CustomService.SaveCustom(record);
           
            return Content("");
        }
    }
}
