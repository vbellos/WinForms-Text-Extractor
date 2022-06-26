using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tesseract_test3
{
    public class TagImage
    {
        public String path { get; set; }
        public String text { get; set; }
        public Boolean isconv { get; set; }

        public TagImage(String Path)
        {
            
            this.path = Path;
            this.text = "";
            this.isconv = false;
        }

        

        

    }
}
