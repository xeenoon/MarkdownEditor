﻿using HtmlAgilityPack;
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
            toconvert += toconvert.Insert(start, "<div id=\"startofselection\"></div>"); //Create a div that we can easily find with its ID
            toconvert += toconvert.Insert(start + length + 1, "<div id=\"endofselection\"></div>"); //End the div

            CustomMarkdown customMarkdown = new CustomMarkdown(toconvert);
            var html = customMarkdown.GetHtml();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            var elementA = doc.GetElementbyId("startofselection");
            Style applied = Style.None;
            HtmlNode last = elementA;

            ResetAllButtons();

            while (last == elementA || last.Name == "div")
            {
                last = last.ParentNode;
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
                    case "quoteblock":
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

            return;
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
    }
}
