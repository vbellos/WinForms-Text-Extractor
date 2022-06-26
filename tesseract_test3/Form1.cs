using DocumentFormat.OpenXml.Wordprocessing;
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
using Color = System.Drawing.Color;

namespace tesseract_test3
{
    public partial class Form1 : Form
    {
        String cur_file = "";
        JsonSerializer jser = new JsonSerializer();
        List<TagImage> ImageList = new List<TagImage>();
        BindingSource source = new BindingSource();
        TagImage cur_img;
        Ocr ocr;
        List<TagImage> sel_images = new List<TagImage>();
        List<Language> Languages;
        

        public Form1()
        {
            InitializeComponent();

            Languages = GetLang(".\\lang.cfg" , ".\\tessdata\\");

            lang_label();

            
            
            initDataGrid();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            initDataGrid();
            
        }

        public void initDataGrid()
        {
            var source = new BindingSource();
            source.DataSource = ImageList;
            dataGridView1.DataSource = source;
        }

        private void addImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";
                dlg.Multiselect = true;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    foreach(String file in dlg.FileNames)
                    { 

                    TagImage tagImage = new TagImage(file);
                   
                    ImageList.Add(tagImage);
                }
                initDataGrid();
                }
            }
  
        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
           
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells[1].Value != null) {

                edit_btn.Enabled = true;

            cur_img = (TagImage)dataGridView1.CurrentRow.DataBoundItem;
                richTextBox1.Text = cur_img.text;
                textBox1.Text = Path.GetFileName(cur_img.path);
                Bitmap img = new Bitmap(cur_img.path);
                pictureBox1.Image = img;

                if (cur_img.isconv) { conv_label.Text = "Converted";conv_label.BackColor = Color.Green; }
                else { conv_label.Text = "Not Converted"; conv_label.BackColor = Color.Red; }
                conv_label.Visible = true;

                sel_images.Clear();

            foreach(DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    sel_images.Add((TagImage)row.DataBoundItem);
                }
  
            }
            else { ClearPanels(); }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (sel_images.Count >0)
            {
                

                foreach(TagImage image in sel_images)
                {
                    this.Text = "Converting " + (sel_images.IndexOf(image) + 1).ToString() + " / " + sel_images.Count().ToString() +"...";
                    
                    if(image.isconv == false) { 
                    ocr = new Ocr(image.path,Properties.Settings.Default.Lang);
                    ocr.Convert();
                    image.text = ocr.result;
                    image.isconv = true;
                    }
                }
                this.Text = "Text Extractor";
                
            }

            initDataGrid();

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(sel_images.Count > 0)
            {
                foreach(TagImage image in sel_images)
                {
                    ImageList.Remove(image);
                }
            }
            
            initDataGrid();
        }

        private void button3_Click(object sender, EventArgs e)
        {
         
        }

        private void button4_Click(object sender, EventArgs e)
        {
           
        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open File";
                dlg.Filter = "json files (*.json)|*.json";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    ImageList = jser.ReadFromJsonFile<List<TagImage>>(dlg.FileName);
                    cur_file = dlg.FileName;
                    initDataGrid();
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Title = "Save File";
            saveFileDialog1.Filter = "json files (*.json)|*.json";
            saveFileDialog1.FilterIndex = 2;
            

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    myStream.Close();
                    jser.WriteToJsonFile<List<TagImage>>(saveFileDialog1.FileName, ImageList);

                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
                jser.WriteToJsonFile<List<TagImage>>(cur_file, ImageList);
             
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cur_file = "";
            ImageList.Clear();
            initDataGrid();

            ClearPanels();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        void ClearPanels() {
            save_btn.Enabled = false;
            save_btn.Enabled = false;
            richTextBox1.Text = "";
            textBox1.Text = "";
            pictureBox1.Image = null;
            conv_label.Visible = false;
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void exportToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            export_form export_Form = new export_form(ImageList);
            export_Form.Text = "Export to Excel";
            export_Form.ShowDialog();
          
        }

     

        private void languageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
        String lang_text()
        {
                return this.Languages.Find(lang => lang.Path == Properties.Settings.Default.Lang).Name;
        }

        List<Language> GetLang(String l_path,String tess_path)
        {
            List<Language> languages = new List<Language>();
            Language language;

            string line;
 
            System.IO.StreamReader file =
                new System.IO.StreamReader(l_path);
            while ((line = file.ReadLine()) != null)
            {
                language = new Language();
                language.setLangFromFile(line);
                if (File.Exists(tess_path+language.Path+ ".traineddata")) { languages.Add(language); }
            }
            return languages;
        }

        private void BuildMenuItems()
        {
            ToolStripMenuItem[] items = new ToolStripMenuItem[Languages.Count()]; // You would obviously calculate this value at runtime
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new ToolStripMenuItem();
                items[i].Name = "lang" + i.ToString();
                items[i].Tag = Languages[i].Path;
                items[i].Text = Languages[i].Name;
                items[i].Click += new EventHandler(MenuItemClickHandler);
            }

            languageToolStripMenuItem.DropDownItems.AddRange(items);
        }

        private void MenuItemClickHandler(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            Properties.Settings.Default.Lang = clickedItem.Tag.ToString();
            label2.Text = "Current Language: " + clickedItem.Text.ToString();
            // Take some action based on the data in clickedItem
        }

        private void downloadLanguagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void settingsToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            languageToolStripMenuItem.DropDownItems.Clear();
            Languages = GetLang(".\\lang.cfg", ".\\tessdata\\");
            BuildMenuItems();
        }

        private void languageToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            languageToolStripMenuItem.DropDownItems.Clear();
            Languages = GetLang(".\\lang.cfg", ".\\tessdata\\");
            BuildMenuItems();
        }

        private void fileToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            if (cur_file == "") { saveToolStripMenuItem.Enabled = false; }
            else { saveToolStripMenuItem.Enabled = true; }
            if (cur_file == "" && ImageList.Count == 0) { closeToolStripMenuItem.Enabled = false; }
            else { closeToolStripMenuItem.Enabled = true; }
            if (ImageList.Count == 0) { saveAsToolStripMenuItem.Enabled = false; }
            else { saveAsToolStripMenuItem.Enabled = true; }
        }

        private void exportToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            if (ImageList.Count == 0) { exportToExcelToolStripMenuItem.Enabled = false; }
            else { exportToExcelToolStripMenuItem.Enabled = true; }
        }

        private void downloadLanguagesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            lang_dl_form dl_Form = new lang_dl_form();
            dl_Form.ShowDialog();
        }
        void lang_label()
        {
            Language l = Languages.Find(i => i.Path == Properties.Settings.Default.Lang);
            if (l != null)
            { label2.Text = "Current Language: " + l.Name; }
            else if (Languages.Count > 0){ Properties.Settings.Default.Lang = Languages[1].Path; lang_label(); }
            else { label2.Text = "Install a language to continue";
                dataGridView1.Enabled = false;
                conv_btn.Enabled = false;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            addImage_btn.Enabled = false;
            conv_btn.Enabled = false;
            del_btn.Enabled = false;
            menuStrip1.Enabled = false;
            dataGridView1.Enabled = false;
            richTextBox1.ReadOnly = false; 
            edit_btn.Enabled = false;
            save_btn.Enabled = true;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            cur_img.text = richTextBox1.Text;

            addImage_btn.Enabled = true;
            conv_btn.Enabled = true;
            del_btn.Enabled = true;
            menuStrip1.Enabled = true;
            dataGridView1.Enabled = true;
            richTextBox1.ReadOnly = true;
            edit_btn.Enabled = true;
            save_btn.Enabled = false;
        }
    }
    }

