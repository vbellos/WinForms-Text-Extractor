using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace tesseract_test3
{
    class Ocr
    {
        public String lang { get; set; }
        public String path { get; set; }
        public String result { get; set; }
        

        

        public Ocr(String path, String lang) { this.path = path; this.lang = lang; }

        public void Convert()
        {
            Bitmap img = new Bitmap(this.path);
            TesseractEngine engine = new TesseractEngine("./tessdata", lang, EngineMode.Default);
            Page page = engine.Process(img, PageSegMode.Auto);
            string res = page.GetText();
            this.result = res.ToUpper();
        }
    }
}
