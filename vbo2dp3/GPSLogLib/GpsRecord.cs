using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vbo2dp3.GPSLogLib
{
    public class GpsRecord
    {

        /// <summary>
        /// 日時
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 速度[km/h]
        /// </summary>
        public double Speed { get; set; }
        /// <summary>
        /// 経度[度]
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// 緯度[度]
        /// </summary>
        public double Latitude { get; set; }

    }
}
