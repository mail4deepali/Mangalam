using Microsoft.Extensions.Options;
using MMB.Mangalam.Web.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Helpers
{
    public static class IOptionsHelper
    {
        public static IOptions<AppSettings> Get()
        {
            var settings = new AppSettings()
            {
                Secret = "ML Mangalam progressive web application"
            };
            
            IOptions<AppSettings> appSettingsOptions = Options.Create(settings);

            return appSettingsOptions;
        }

    }
}
