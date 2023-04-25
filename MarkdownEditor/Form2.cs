using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
                button1.Text = file.Split('\\').Last();
                filepath = file;
            }
        }
        public static string filecontents;
        public static string key = "";
        private void button2_Click(object sender, EventArgs e)
        {
            if (maskedTextBox1.Text != "") //Did they provide an encryption key?
            {
                key = maskedTextBox1.Text;
                filecontents = Encryption.DecryptFile(filepath,maskedTextBox1.Text); //Attempt to decrypt the file
                Hide();
                var form = new Form1();
                form.ShowDialog();
                Show();
            }
        }
    }
}
