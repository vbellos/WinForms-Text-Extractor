using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tesseract_test3
{
    class Language
    {
        public String Name { get; set; }
        public String Path { get; set; }

        public void setLangFromFile(String fname)
        {
            String[] f = fname.Split(':');
            this.Path = f[0];
            this.Name = f[1];
        }
    }
}
