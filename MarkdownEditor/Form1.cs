﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
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

            foreach (var button in formattingButtons)
            {
                button.Invalidate();
            }
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
        public enum Style
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

            if (richTextBox1.Text.Substring(start, 2) == "**") //Selected the asterix's as well?
            {
                start += 2;
                length -= 2;
            }
            if (richTextBox1.Text.Substring(start+length-2, 2) == "**") //Selected the asterix's as well?
            {
                length -= 2;
            }

            if (((GetStyles(start) & Style.Bold) == Style.Bold) && ((GetStyles(start + length) & Style.Bold) == Style.Bold)) //All inside an underline?
            {
                richTextBox1.Text = richTextBox1.Text.Insert(start, "**");
                richTextBox1.Text = richTextBox1.Text.Insert(start + length + 2, "**");
                start += 2;
            }
            else if (((GetStyles(start) & Style.Bold) == Style.Bold) && !((GetStyles(start + length) & Style.Bold) == Style.Bold)) //First inside, but not second
            {
                //Make everything underlined
                string textselected = richTextBox1.Text.Substring(start, length);
                var boldMatches = Regex.Matches(textselected, @"\*\*").Cast<Match>().ToList();
                textselected = textselected.Replace("**", ""); //Remove all the bold stuff inside the selection
                textselected += "**"; //Add a closing tag

                richTextBox1.Text = richTextBox1.Text.Substring(0, start) + textselected + richTextBox1.Text.Substring(start + length);

                foreach (Match match in boldMatches)
                {
                    if (match.Index < start)
                    {
                        start -= 2;
                    }
                    else if (match.Index >= start && match.Index < start + length)
                    {
                        length -= 2;
                    }
                }//Modify cursor pos
                start += 2;
                length -= 2;
            }
            else if (!((GetStyles(start) & Style.Bold) == Style.Bold) && ((GetStyles(start + length) & Style.Bold) == Style.Bold)) //First outside, second inside
            {
                //Make everything bold
                string textselected = richTextBox1.Text.Substring(start, length);
                var boldMatches = Regex.Matches(textselected, @"\*\*").Cast<Match>().ToList();
                textselected = textselected.Replace("**", "");
                textselected = "**" + textselected; //Add a opening tag

                richTextBox1.Text = richTextBox1.Text.Substring(0, start) + textselected + richTextBox1.Text.Substring(start + length);

                foreach (Match match in boldMatches)
                {
                    if (match.Index < start)
                    {
                        start -= 2;
                    }
                    else if (match.Index >= start && match.Index < start + length)
                    {
                        length -= 2;
                    }
                } //Modify cursor pos
                start += 2;
            }
            else //Neither are inside 
            {
                richTextBox1.Text = richTextBox1.Text.Insert(start, "**");
                richTextBox1.Text = richTextBox1.Text.Insert(start + length + 2, "**");
                start += 2;
            }

            var doublebolds = Regex.Matches(richTextBox1.Text, @"\*\*\*\*").Cast<Match>().ToList();
            int totalremoved = 0;
            foreach (Match match in doublebolds)
            {
                if (match.Index <= start)
                {
                    start -= 4;
                }
                else if (match.Index >= start && match.Index < start + length)
                {
                    length -= 4;
                }
                richTextBox1.Text = richTextBox1.Text.Substring(0, match.Index - totalremoved) + richTextBox1.Text.Substring(match.Index + 4 - totalremoved);
                totalremoved += 4;
            }

            richTextBox1.Select(start, length);
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
            if (endidx == richTextBox1.Text.Length || richTextBox1.Text[endidx] != '*' || (endidx <= richTextBox1.Text.Length-2 && richTextBox1.Text[endidx+1] == '*')) //Cannot be italics, but can be bold
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
        private void StrikeClicked(object sender, EventArgs e)
        {
            FormattingClicked(sender, e);
            var start = richTextBox1.SelectionStart;
            var length = richTextBox1.SelectionLength;

            richTextBox1.Text = richTextBox1.Text.Insert(start, "~~");
            richTextBox1.Text = richTextBox1.Text.Insert(start + length + 2, "~~");
            var replace = richTextBox1.Text.Replace("~~~~", "");
            if (richTextBox1.Text == replace)
            {
                richTextBox1.Select(start + 2, length);
            }
            else
            {
                richTextBox1.Text = replace;
                richTextBox1.Select(start - 2, length);
            }
            richTextBox1.Focus();
        }
        private void CodeClicked(object sender, EventArgs e)
        {
            FormattingClicked(sender, e);
            var start = richTextBox1.SelectionStart;
            var length = richTextBox1.SelectionLength;

            richTextBox1.Text = richTextBox1.Text.Insert(start, "```");
            richTextBox1.Text = richTextBox1.Text.Insert(start + length + 3, "```");
            var replace = richTextBox1.Text.Replace("``````", "");
            if (richTextBox1.Text == replace)
            {
                richTextBox1.Select(start + 3, length);
            }
            else
            {
                richTextBox1.Text = replace;
                richTextBox1.Select(start - 3, length);
            }
            richTextBox1.Focus();
        }
        private void UnderlineClicked(object sender, EventArgs e)
        {
            FormattingClicked(sender, e);
            var start = richTextBox1.SelectionStart;
            var length = richTextBox1.SelectionLength;

            if (richTextBox1.Text.Substring(start, 3) == "<u>") //Have we selected the start of a tag?
            {
                start += 3;
                length -= 3;
            }

            if (((GetStyles(start) & Style.Underline) == Style.Underline) && ((GetStyles(start + length) & Style.Underline) == Style.Underline)) //All inside an underline?
            {
                richTextBox1.Text = richTextBox1.Text.Insert(start, "</u>");
                richTextBox1.Text = richTextBox1.Text.Insert(start + length + 4, "<u>");
                start += 4;
            }
            else if (((GetStyles(start) & Style.Underline) == Style.Underline) && !((GetStyles(start + length) & Style.Underline) == Style.Underline)) //First inside, but not second
            {
                //Make everything underlined
                string textselected = richTextBox1.Text.Substring(start, length);
                var openingMatches = Regex.Matches(textselected, "<u>").Cast<Match>().ToList();
                var closingMatches = Regex.Matches(textselected, "</u>").Cast<Match>().ToList();
                textselected = textselected.Replace("<u>", "");
                textselected = textselected.Replace("</u>", ""); //Remove all the underline stuff inside the selection
                textselected += "</u>"; //Add a closing tag

                richTextBox1.Text = richTextBox1.Text.Substring(0, start) + textselected + richTextBox1.Text.Substring(start + length);

                foreach (Match match in openingMatches)
                {
                    if (match.Index < start)
                    {
                        start -= 3;
                    }
                    else if (match.Index >= start && match.Index < start + length)
                    {
                        length -= 3;
                    }
                }
                foreach (Match match in closingMatches)
                {
                    if (match.Index < start)
                    {
                        start -= 4;
                    }
                    else if (match.Index >= start && match.Index < start + length)
                    {
                        length -= 4;
                    }
                } //Modify cursor pos
                start += 4;
                length -= 4;
            }
            else if (!((GetStyles(start) & Style.Underline) == Style.Underline) && ((GetStyles(start + length) & Style.Underline) == Style.Underline)) //First outside, second inside
            {
                //Make everything underlined
                string textselected = richTextBox1.Text.Substring(start, length);
                var openingMatches = Regex.Matches(textselected, "<u>").Cast<Match>().ToList();
                var closingMatches = Regex.Matches(textselected, "</u>").Cast<Match>().ToList();
                textselected = textselected.Replace("<u>", "");
                textselected = textselected.Replace("</u>", ""); //Remove all the underline stuff inside the selection
                textselected = "<u>" + textselected; //Add a closing tag

                richTextBox1.Text = richTextBox1.Text.Substring(0, start) + textselected + richTextBox1.Text.Substring(start + length);

                foreach (Match match in openingMatches)
                {
                    if (match.Index < start)
                    {
                        start -= 3;
                    }
                    else if (match.Index >= start && match.Index < start + length)
                    {
                        length -= 3;
                    }
                }
                foreach (Match match in closingMatches)
                {
                    if (match.Index < start)
                    {
                        start -= 4;
                    }
                    else if (match.Index >= start && match.Index < start + length)
                    {
                        length -= 4;
                    }
                } //Modify cursor pos
                start += 3;
            }
            else //Neither are inside 
            {
                richTextBox1.Text = richTextBox1.Text.Insert(start, "<u>");
                richTextBox1.Text = richTextBox1.Text.Insert(start + length + 3, "</u>");
                start += 3;
            }

            var matches = Regex.Matches(richTextBox1.Text, "<u></u>").Cast<Match>().ToList();
            matches.AddRange(Regex.Matches(richTextBox1.Text, "</u><u>").Cast<Match>().ToList());
            foreach (Match match in matches)
            {
                if (match.Index < start)
                {
                    start -= 7;
                }
                else if (match.Index >= start && match.Index < start + length)
                {
                    length -= 7;
                }
            }
            richTextBox1.Text = richTextBox1.Text.Replace("<u></u>", "");
            richTextBox1.Text = richTextBox1.Text.Replace("</u><u>", "");

            int totalremoved = 0;
            foreach (var match in UnusedTags(richTextBox1.Text))
            {
                if (match.Index <= start)
                {
                    start -= match.length;
                }
                else if (match.Index >= start && match.Index < start + length)
                {
                    length -= match.length;
                }
                richTextBox1.Text = richTextBox1.Text.Substring(0,match.Index-totalremoved) + richTextBox1.Text.Substring(match.Index + match.length-totalremoved);
                totalremoved += match.length;
            }

            richTextBox1.Select(start, length);
            RemoveUnusedTags();
            richTextBox1.Focus();
        }
        public List<TagLocation> UnusedTags(string md)
        {
            List<TagLocation> result = new List<TagLocation>();

            var underlinetags = Regex.Matches(md, "<u>").Cast<Match>().ToList();
            foreach (Match match in underlinetags)
            {
                var betweenTags = md.Substring(match.Index+3, md.ClosingTag("<u>", match.Index)-match.Index-3); //Find the text between the two tags
                if (betweenTags.StartsWith("<u>")) //Double <u> tags?
                {
                    result.Add(new TagLocation(match.Index+3, 3, Style.Underline));
                }
                if (betweenTags.EndsWith("</u>")) //Double </u> tags?
                {
                    result.Add(new TagLocation(md.ClosingTag("<u>", match.Index), 4, Style.Underline));
                }
            }
            return result;
        }
        public void RemoveUnusedTags()
        {
            var start = richTextBox1.SelectionStart;
            var length = richTextBox1.SelectionLength;
            KeyValuePair<TagLocation, TagLocation> firstmatch;
            try
            {
                firstmatch = CheckTagValidity().First();
            }
            catch
            {
                return;
            }
            if (firstmatch.Key.Index < start)
            {
                start -= 1; //Removing a / so start will decrease
            }
            else if (firstmatch.Key.Index >= start && firstmatch.Key.Index < start + length)
            {
                length -= 1; //Remove a / so will decrease
            }
            if (firstmatch.Value.length == 3) //Adding a /?
            {
                if (firstmatch.Value.Index < start)
                {
                    start += 1; //Adding a / so start will increase
                }
                else if (firstmatch.Value.Index >= start && firstmatch.Value.Index < start + length)
                {
                    length += 1; //Adding a / so will increase
                }
                var val_idx = firstmatch.Value.Index;
                richTextBox1.Text = richTextBox1.Text.Substring(0, val_idx + 1) + '/' + richTextBox1.Text.Substring(val_idx + 1); //Add a slash
            }
            //Remove slash from the first one
            var key_idx = firstmatch.Key.Index;
            richTextBox1.Text = richTextBox1.Text.Substring(0, key_idx + 1) + richTextBox1.Text.Substring(key_idx + 2); //Get everything except the /
            richTextBox1.Select(start, length);
            RemoveUnusedTags(); //Keep going until we finish
        }
        public Dictionary<TagLocation, TagLocation> CheckTagValidity()
        {
            Dictionary<TagLocation, TagLocation> result = new Dictionary<TagLocation, TagLocation>();

            var openings = Regex.Matches(richTextBox1.Text, "<u>").Cast<Match>().ToList();
            var closings = Regex.Matches(richTextBox1.Text, "</u>").Cast<Match>().ToList();

            Dictionary<int, bool> tagidxs = new Dictionary<int, bool>(); ;
            foreach (var opening in openings)
            {
                tagidxs.Add(opening.Index, true);
            }
            foreach (var closing in closings)
            {
                tagidxs.Add(closing.Index, false);
            }
            int totalopenings = 0;
            int totalclosings = 0;
            bool addnext = false;
            TagLocation last = null;
            foreach (var item in tagidxs.OrderBy(t => t.Key)) //Order all the tags by the order they appear in the string
            {
                if (addnext)
                {
                    result.Add(last, new TagLocation(item.Key, item.Value == true ? 3 : 4, Style.Underline));
                    last = null;
                }

                if (item.Value == true) //Is it an opening
                {
                    ++totalopenings;
                }
                else
                {
                    ++totalclosings;
                }
                if (totalclosings > totalopenings) //Has the user started with a closing tag?
                {
                    //Be nice and change it to an opening tag
                    addnext = true;
                    last = new TagLocation(item.Key, 4, Style.Underline);
                    totalopenings++;
                    totalclosings--;
                }
            }
            return result;
        }
        public class TagLocation
        {
            public int Index;
            public int length;
            public Style style;

            public TagLocation(int Index, int length, Style style)
            {
                this.Index = Index;
                this.length = length;
                this.style = style;
            }
        }
        public Style GetStyles(int start)
        {
            Style result = Style.None;

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
                    return result;
                }
                switch (last.Name)
                {
                    case "code":
                        result |= Style.Code;
                        break;
                    case "i":
                        result |= Style.Italics;
                        break;
                    case "b":
                        result |= Style.Bold;
                        break;
                    case "blockquote":
                        result |= Style.Quote;
                        break;
                    case "u":
                        result |= Style.Underline;
                        break;
                    case "strike":
                        result |= Style.Strikethrough;
                        break;
                }
            }
        }
    }
}
