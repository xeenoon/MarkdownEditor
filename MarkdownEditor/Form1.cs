using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
            var md = Properties.Resources.DefaultText;
            richTextBox1.Text = md;
            CustomMarkdown customMarkdown = new CustomMarkdown(md);
            webBrowser1.DocumentText = customMarkdown.GetHtml();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var md = richTextBox1.Text;
            CustomMarkdown customMarkdown = new CustomMarkdown(md);
            webBrowser1.DocumentText = customMarkdown.GetHtml();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser1.Document.Window.ScrollTo(0, webBrowser1.Document.Body.ScrollRectangle.Height);
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.AbsolutePath == "blank")
            {
                return;
            }
            Process.Start("chrome.exe", e.Url.AbsoluteUri);
            e.Cancel = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        [Flags]
        enum Style
        {
            Default=1,
            Heading1=2,
            Heading2=4,
            Heading3=8,
            Heading4=16,
            Bold=32,
            Italics=64,
            Strikethrough=128,
            Code=256,
            Quote=512,
        }
        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            var start = richTextBox1.SelectionStart;
            var length = richTextBox1.SelectionLength;

            string toconvert = richTextBox1.Text;
            toconvert += toconvert.Insert(start, "<div id=\"startofselection\">"); //Create a div that we can easily find with its ID
            toconvert += toconvert.Insert(start+length + 1, "⎳"); //Second div

            CustomMarkdown customMarkdown = new CustomMarkdown(toconvert);
            var html = customMarkdown.GetHtml();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            var elementA = doc.GetElementbyId("startofselection");
        }

        private void FormattingClicked(object sender, EventArgs e)
        {
            var button = ((Button)sender);
            string selectname = button.Name.Split('_')[0]+"_select";
            if ((string)button.BackgroundImage.Tag == null)
            {
                button.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject(selectname);
                button.BackgroundImage.Tag = "not null";
            }
            else
            {
                button.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject(selectname.Split('_')[0]);
                button.BackgroundImage.Tag = null;
            }
        }
    }
}
