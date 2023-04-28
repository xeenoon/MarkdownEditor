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

namespace MarkdownEditor
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public static string filepath = "";
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog();
            if (result != DialogResult.Cancel)
            {
                var file = openFileDialog.FileName;
                //button1.Text = file.Split('\\').Last();
                filepath = file;
            }
        }
        public static string filecontents;
        public static string key = "";
        public static bool creating = false;
        private void button2_Click(object sender, EventArgs e)
        {
            if (maskedTextBox1.Text != "") //Did they provide an encryption key?
            {
                if (!creating)
                {
                    filecontents = Encryption.DecryptFile(filepath, maskedTextBox1.Text); //Attempt to decrypt the file
                }
                else
                {
                    filecontents = "";
                }
                key = maskedTextBox1.Text;
                Hide();
                var form = new Form1();
                form.Text = "Editor - " + filepath.Split('\\').Last();
                form.ShowDialog();
                Show();
                creating = false;
                maskedTextBox1.Text = "";
                label1.Text = "";
                filepath = "";
            }

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            panel2.Visible = !panel2.Visible;
        }
        private void OpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog();
            if (result != DialogResult.Cancel)
            {
                var file = openFileDialog.FileName;
                //button1.Text = file.Split('\\').Last();
                filepath = file;
                label1.Text = file.Split('\\').Last();
            }
            creating = false;
        }

        private void CreateFile_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            FontDialog f = new FontDialog();
            f.ShowDialog();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() != DialogResult.Cancel)
            {
                textBox4.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBox4.Text = "";
            textBox5.Text = "";
            panel2.Visible = false;
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            filepath = textBox4.Text + "\\" + textBox5.Text;
            if (!filepath.EndsWith(".md") && !filepath.EndsWith(".txt"))
            {
                filepath = filepath + ".md"; //Automatically add the file extension
            }
            File.Create(filepath).Close();
            label1.Text = filepath.Split('\\').Last();
            creating = true;
            filecontents = "Example text:";
            panel2.Visible = false;
        }
    }
}
