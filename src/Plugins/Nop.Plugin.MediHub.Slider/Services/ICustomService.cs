using System;
using System.Collections.Generic;
using System.Text;
using Nop.Plugin.MediHub.Slider.Domains;

namespace Nop.Plugin.MediHub.Slider.Services
{
    public interface ICustomService
    {
        void SaveCustom(CustomTable record);
    }
}
