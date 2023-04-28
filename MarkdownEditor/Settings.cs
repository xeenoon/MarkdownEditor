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
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            panel1.Visible = !panel1.Visible;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            label6.Visible = checkBox1.Checked;
            textBox1.Visible = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            label7.Visible = checkBox2.Checked;
            label8.Visible = checkBox2.Checked;
            textBox2.Visible = checkBox2.Checked;
            textBox3.Visible = checkBox2.Checked;
        }
    }
}
