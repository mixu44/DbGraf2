using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DbGraf2.viewmodels
{
    public class ChartViewModel
    {
        public IEnumerable<DbGraf2.Models.MeteorologiData> Data { get; set; }

        public bool ShowWindSpeed { get; set; }
        public bool ShowWindDirection { get; set; }
        public bool ShowTemperature { get; set; }
        public bool ShowHumidity { get; set; }
        public bool ShowRadiation { get; set; }
        public bool ShowPressure { get; set; }
    }
}