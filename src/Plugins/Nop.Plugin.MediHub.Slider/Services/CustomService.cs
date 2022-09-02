using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Data;
using Nop.Plugin.MediHub.Slider.Domains;

namespace Nop.Plugin.MediHub.Slider.Services
{
    public class CustomService : ICustomService
    {
        private readonly IRepository<CustomTable> _customeTableRepository;

        public CustomService(IRepository<CustomTable> customeTableRepository)
        {
            _customeTableRepository = customeTableRepository;
        }


        public void SaveCustom(CustomTable record)
        {
            if (record == null)
                throw new ArgumentNullException(nameof(record));
            _customeTableRepository.Insert(record);

        }
    }
}
