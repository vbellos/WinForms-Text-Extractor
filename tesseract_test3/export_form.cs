using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tesseract_test3
{
    public partial class export_form : Form
    {
        List<TagImage> imagelist;
        ExcelExtractor excel = new ExcelExtractor();

        public export_form(List<TagImage> imagelist)
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            this.imagelist = imagelist;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex == 0) { textBox2.Text = "C:\\Users\\Vaggelis\\Pictures\\image.jpg"; textBox1.Visible = false; }
            if (comboBox1.SelectedIndex == 1) { textBox2.Text = "image.jpg"; textBox1.Visible = false; }
            if (comboBox1.SelectedIndex == 2) { textBox2.Text = "image.jpg"; textBox1.Visible = true; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != 0)
            {
                foreach (TagImage img in imagelist)
                {
                    if(comboBox1.SelectedIndex == 1) { img.path = Path.GetFileName(img.path); }
                    else { img.path = textBox1.Text + Path.GetFileName(img.path); }
                    
                }
            }
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Title = "Save File";
            saveFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx";
            saveFileDialog1.FilterIndex = 2;


            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    myStream.Close();
                    excel.Create(imagelist, saveFileDialog1.FileName);
                }
            }
            
        }
    }
}
