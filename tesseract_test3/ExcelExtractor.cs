using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace tesseract_test3
{
    class ExcelExtractor
    {
        public void Create(List<TagImage> imagelist ,String filePath)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Image List");
            worksheet.Cell("A1").Value = "Path";
            worksheet.Cell("B1").Value = "Text";

            int i = 1;

            foreach (TagImage img in imagelist)
            {
                i++;
                worksheet.Cell("A"+ i.ToString()).Value = img.path;
                worksheet.Cell("B" + i.ToString()).Value = img.text;

            }

            workbook.SaveAs(filePath);
        }
    }
}
