using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vbo2dp3.GPSLogLib
{
    public  class FileCheckUtil
    {

        public static bool CheckExistsAndExtension(string path ,string extension)
        {
            if (!File.Exists(path)) { throw new FileNotFoundException(); }
            if (Path.GetExtension(path) != extension)
            {
                throw new ArgumentException("Invalid file.");
            }
            return true;
        }
    }
}
