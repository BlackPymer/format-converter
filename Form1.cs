using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Converter
{
    public partial class FormatConverter : Form
    {
        private string _saveFilePath;
        public FormatConverter()
        {
            InitializeComponent();
            formatListBox.Items.Add("JSON");
            formatListBox.Items.Add("XML");
            formatListBox.Items.Add("YAML");
            formatListBox.Items.Add("CSV");
            formatListBox.Items.Add("HTML");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string file_path = "";
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "JSON файлы (*.json)|*.json|XML файлы (*.xml)|*.xml|CSV файлы (*.csv)|*.csv|HTML файлы (*.html)|*.html|YAML файлы (*.yaml)|*.yaml|Все файлы (*.*)|*.*";
                openFileDialog.FilterIndex = 0;
                openFileDialog.Multiselect = false;
                openFileDialog.RestoreDirectory = false;
                if(openFileDialog.ShowDialog() == DialogResult.OK)
                    file_path = openFileDialog.FileName;
            }
            if(file_path != "")
            {
                if (Program.LoadData(file_path))
                {
                    mainLabel.Text = "Choose folder, name, file format";
                }
            }

        }

        private void folderSelectButton_Click(object sender, EventArgs e)
        {
            string folderPath = "";
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                
                folderBrowserDialog.Description = "Select the directory you want to save converted file to";
                folderBrowserDialog.ShowNewFolderButton = true;

                if(folderBrowserDialog.ShowDialog() == DialogResult.OK);
                    folderPath = folderBrowserDialog.SelectedPath;
            }
            if (folderPath != "")
            {
                _saveFilePath = folderPath;
                convertButton.Enabled = true;
            }
        }

        private void convertButton_Click(object sender, EventArgs e)
        {
            if (Program.SaveData(_saveFilePath + "\\" + fileNameText.Text, formatListBox.SelectedIndex))
                mainLabel.Text = "Converted successfully";
            else
                mainLabel.Text = "Failed to convert";
        }
    }
}
