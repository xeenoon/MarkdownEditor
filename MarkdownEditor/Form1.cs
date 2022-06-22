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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var md = richTextBox1.Text;
            //var html = Markdig.Markdown.ToHtml(md);
            CustomMarkdown customMarkdown = new CustomMarkdown(md);
            webBrowser1.DocumentText = customMarkdown.GetHtml();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser1.Document.Window.ScrollTo(0, webBrowser1.Document.Body.ScrollRectangle.Height);
        }
    }
}
