using HtmlAgilityPack;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarkdownEditor
{
    public partial class Form1 : Form
    {
        List<Button> formattingButtons = new List<Button>();
        public Form1()
        {
            InitializeComponent();
            var md = Properties.Resources.DefaultText;
            richTextBox1.Text = md;
            richTextBox1.AutoWordSelection = true;
            CustomMarkdown customMarkdown = new CustomMarkdown(md);
            webBrowser1.DocumentText = customMarkdown.GetHtml();

            formattingButtons.Add(Bold_button);
            formattingButtons.Add(Code_button);
            formattingButtons.Add(Font_button);
            formattingButtons.Add(Heading_button);
            formattingButtons.Add(Image_button);
            formattingButtons.Add(Italics_button);
            formattingButtons.Add(Link_button);
            formattingButtons.Add(Quote_button);
            formattingButtons.Add(Strikethrough_button);
            formattingButtons.Add(Underline_button);
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
            None = 0,
            Default = 1,
            Bold = 2,
            Italics = 4, 
            Strikethrough = 8,
            Code = 16,
            Quote = 32,
            Underline = 64,
        }
        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            var start = richTextBox1.SelectionStart;
            var length = richTextBox1.SelectionLength;

            string toconvert = richTextBox1.Text;
            toconvert = toconvert.Insert(start, "<div id=\"startofselection\"></div>"); //Create a div that we can easily find with its ID
            //toconvert = toconvert.Insert(start + length + 36, "<div id=\"endofselection\"></div>"); //End the div

            CustomMarkdown customMarkdown = new CustomMarkdown(toconvert);
            var html = customMarkdown.GetHtml();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            var elementA = doc.GetElementbyId("startofselection");
            HtmlNode last = elementA;

            ResetAllButtons();

            while (true)
            {
                last = last.ParentNode;
                if (last == null)
                {
                    return;
                }
                switch (last.Name)
                {
                    case "code":
                        FormattingClicked(Code_button, null);
                        break;
                    case "i":
                        FormattingClicked(Italics_button, null);
                        break;
                    case "b":
                        FormattingClicked(Bold_button, null);
                        break;
                    case "blockquote":
                        FormattingClicked(Quote_button, null);
                        break;
                    case "u":
                        FormattingClicked(Underline_button, null);
                        break;
                    case "strike":
                        FormattingClicked(Strikethrough_button, null);
                        break;
                }
            }
        }

        private void FormattingClicked(object sender, EventArgs e)
        {
            var button = ((Button)sender);
            string selectname = button.Name.Split('_')[0] + "_select";
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
        private void ResetAllButtons()
        {
            foreach (var button in formattingButtons)
            {
                button.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject(button.Name.Split('_')[0]);
                button.BackgroundImage.Tag = null;
            }
        }

        private void BoldClicked(object sender, EventArgs e)
        {
            FormattingClicked(sender, e);
            var start = richTextBox1.SelectionStart;
            var length = richTextBox1.SelectionLength;

            richTextBox1.Text = richTextBox1.Text.Insert(start, "**");
            richTextBox1.Text = richTextBox1.Text.Insert(start+length+2, "**");
            var replace = richTextBox1.Text.Replace("****", "");
            if (richTextBox1.Text == replace)
            {
                richTextBox1.Select(start+2, length);
            }
            else
            {
                richTextBox1.Text = replace;
                richTextBox1.Select(start - 2, length);
            }
            richTextBox1.Focus();
        }

        private void ItalicsClicked(object sender, EventArgs e)
        {
            FormattingClicked(sender, e);
            var start = richTextBox1.SelectionStart;
            var length = richTextBox1.SelectionLength;

            //Check if we are selecting the asterix's as well
            if (richTextBox1.Text[start] == '*')
            {
                start++;
                length--;
            }
            if (richTextBox1.Text[start+length-1] == '*')
            {
                length--;
            }

            int add = 0;
            if (start == 0 || richTextBox1.Text[start-1] != '*' || (start >=2 && richTextBox1.Text[start-2] == '*')) //Cannot already be italics, but can be bold
            {
                richTextBox1.Text = richTextBox1.Text.Insert(start, "*");
                add = 1;
            }
            else //It is already italics
            {
                richTextBox1.Text = richTextBox1.Text.Substring(0, start-1) + richTextBox1.Text.Substring(start); //Remove the italics thing
                add = -1;
            }
            int endidx = start + length + add;
            if (endidx == richTextBox1.Text.Length || richTextBox1.Text[endidx] != '*' || (endidx <= richTextBox1.Text.Length-1 && richTextBox1.Text[endidx+1] == '*')) //Cannot be italics, but can be bold
            {
                richTextBox1.Text = richTextBox1.Text.Insert(endidx, "*");
            }
            else
            {
                richTextBox1.Text = richTextBox1.Text.Substring(0, endidx) + richTextBox1.Text.Substring(endidx+1);
            }
            if (add==1)
            {
                start++;
                richTextBox1.Select(start, length);
            }
            else
            {
                start--;
                richTextBox1.Select(start, length);
            }
            var replace = richTextBox1.Text.Replace("****", "**");
            if (richTextBox1.Text != replace) //Moving stuffs
            {
                start -= 2;
                richTextBox1.Text = replace;
                richTextBox1.Select(start, length);
            }
            richTextBox1.Focus();
        }
    }
}
