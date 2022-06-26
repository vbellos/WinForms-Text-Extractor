using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tesseract_test3
{
    public partial class lang_dl_form : Form
    {
        List<Language> languages;
        BindingSource source = new BindingSource();
        List<Language> sel_languages = new List<Language>();
        WebClient webClient = new WebClient();

        public lang_dl_form()
        {
            InitializeComponent();
            languages = GetLang(".\\lang.cfg", ".\\tessdata\\");
            initDataGrid();
        }

        private void lang_dl_form_Load(object sender, EventArgs e)
        {

        }

        public void initDataGrid()
        {
            var source = new BindingSource();
            source.DataSource = languages;
            dataGridView1.DataSource = source;
        }

            List<Language> GetLang(String l_path, String tess_path)
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
                    if (!(File.Exists(tess_path + language.Path + ".traineddata"))) { languages.Add(language); }
                }
                return languages;
            }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells[1].Value != null)
            {

                sel_languages.Clear();

                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    sel_languages.Add((Language)row.DataBoundItem);
                }
                dlbtn.Enabled = true;
            }
            else { dlbtn.Enabled = false; }
        
        }

        private void dlbtn_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            this.Enabled = false;
            panel1.Enabled = true;
            

            foreach (Language lang in this.sel_languages)
            {
                if (true)

                {
                    String path = ".\\tessdata\\" + lang.Path + ".traineddata";
                    String url = "http://raw.githubusercontent.com/tesseract-ocr/tessdata/3.04.00/" + lang.Path + ".traineddata";
                    if (checkifexists(url)) { dl(path, url); }
                    else 
                    { 
                        MessageBox.Show("File not found! \n Please check your internet conetction.");
                        this.Enabled = true;
                        progressBar1.Visible = false;
                    }
                }
            }
        }
        private void dl(String path, String url)
        {
            
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            webClient.DownloadFileAsync(new Uri(url),path);
            
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            
            progressBar1.Value = e.ProgressPercentage;

        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            this.Enabled = true;
            progressBar1.Visible = false;
            MessageBox.Show("Download completed!");
        }

        private void cancbtn_Click(object sender, EventArgs e)
        {
            this.webClient.CancelAsync();
        }

        bool checkifexists(String url)
        {
            WebRequest serverRequest = WebRequest.Create(url);
            WebResponse serverResponse;
            try //Try to get response from server  
            {
                serverResponse = serverRequest.GetResponse();
            }
            catch //If could not obtain any response  
            {
                return false;
            }
            serverResponse.Close();
            return true;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Download whatever language pack you want. \n Just Drop it in the \\tessdata folder inside the program directory.");
            System.Diagnostics.Process.Start("https://tesseract-ocr.github.io/tessdoc/Data-Files.html#data-files-for-version-304305");
        }
    }
    }


   


