using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MarkdownEditor
{
    public partial class Form1 : Form
    {
        List<Button> formattingButtons = new List<Button>();
        System.Timers.Timer autosavetimer = new System.Timers.Timer(60000);
        public Form1()
        {
            InitializeComponent();
            var md = Form2.filecontents;
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

            if (Form2.creating)
            {
                Save(false);
            }

            autosavetimer.Interval = 60000;
            autosavetimer.Elapsed += new System.Timers.ElapsedEventHandler(Autosave);
            autosavetimer.AutoReset = true;
            autosavetimer.Start();
        }

        private void Autosave(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (checkBox1.Checked)
            {
                this.Invoke((MethodInvoker)delegate { Save(false); label1.Text = "Saved at " + DateTime.Now.ToShortTimeString(); });
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //var md = richTextBox1.Text;
            //CustomMarkdown customMarkdown = new CustomMarkdown(md);
            //string v = customMarkdown.GetHtml();
            //webBrowser1.DocumentText = v;
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
            HFormatting(H0_button);

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
                if (last == null)
                {
                    break;
                }
                last = last.ParentNode;
                if (last == null)
                {
                    break;
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
                    case "h1":
                        HFormatting(H1_button);
                        break;
                    case "h2":
                        HFormatting(H2_button);
                        break;
                    case "h3":
                        HFormatting(H3_button);
                        break;
                }
            }
            toconvert = richTextBox1.Text;
            toconvert = toconvert.Insert(start, "<div id=\"startofselection\"></div>"); //Create a div that we can easily find with its ID

            customMarkdown = new CustomMarkdown(toconvert);
            html = customMarkdown.GetHtml();
            doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            elementA = doc.GetElementbyId("startofselection");

            if (elementA != null && elementA.PreviousSibling != null && elementA.PreviousSibling.Name == "p")
            {
                FormattingClicked(Font_button, null);
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
            if (richTextBox1.Text.Length == 0)
            {
                return;
            }

            FormattingClicked(sender, e);
            string finaltext = richTextBox1.Text;
            var start = richTextBox1.SelectionStart;
            var length = richTextBox1.SelectionLength;

            int origin_start = start;
            int origin_length = length;
            string origin_text = finaltext;

            start = MoveOutsideTag(finaltext, start);
            length = MoveOutsideTag(finaltext, start + length) - start;

            if (length == 0)
            {
                return;
            }

            if (finaltext.Length >= start+2 && finaltext.Substring(start, 2) == "**") //Selected the asterix's as well?
            {
                start += 2;
                length -= 2;
            }
            if (finaltext.Length >= start + 2 && start + length - 2 >= 0 && finaltext.Substring(start + length - 2, 2) == "**") //Selected the asterix's as well?
            {
                length -= 2;
            }

            if (((GetStyles(start) & Style.Strikethrough) == Style.Strikethrough) && ((GetStyles(start + length) & Style.Strikethrough) == Style.Strikethrough)) //All inside an underline?
            {
                finaltext = finaltext.Insert(start, "**");
                finaltext = finaltext.Insert(start + length + 2, "**");
                start += 2;
            }
            else if (((GetStyles(start) & Style.Strikethrough) == Style.Strikethrough) && !((GetStyles(start + length) & Style.Strikethrough) == Style.Strikethrough)) //First inside, but not second
            {
                //Make everything underlined
                string textselected = finaltext.Substring(start, length);
                var boldMatches = Regex.Matches(textselected, @"\*\*").Cast<Match>().ToList();
                textselected = textselected.Replace("**", ""); //Remove all the bold stuff inside the selection
                textselected += "**"; //Add a closing tag

                finaltext = finaltext.Substring(0, start) + textselected + finaltext.Substring(start + length);

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
            else if (!((GetStyles(start) & Style.Strikethrough) == Style.Strikethrough) && ((GetStyles(start + length) & Style.Strikethrough) == Style.Strikethrough)) //First outside, second inside
            {
                //Make everything bold
                string textselected = finaltext.Substring(start, length);
                var boldMatches = Regex.Matches(textselected, @"\*\*").Cast<Match>().ToList();
                textselected = textselected.Replace("**", "");
                textselected = "**" + textselected; //Add a opening tag

                finaltext = finaltext.Substring(0, start) + textselected + finaltext.Substring(start + length);

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
                finaltext = finaltext.Insert(start, "**");
                finaltext = finaltext.Insert(start + length + 2, "**");
                start += 2;
            }

            var doublebolds = Regex.Matches(finaltext, @"\*\*\*\*").Cast<Match>().ToList();
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
                finaltext = finaltext.Substring(0, match.Index - totalremoved) + finaltext.Substring(match.Index + 4 - totalremoved);
                totalremoved += 4;
            }

            richTextBox1.Text = finaltext;
            richTextBox1.Select(start, length);
            richTextBox1.Focus();

            if (origin_text != finaltext) //Text changed?
            {
                if (ctrlZ_idx != ctrlZ_data.Count()) //Inserting item?
                {
                    ctrlZ_data.RemoveRange(ctrlZ_idx, ctrlZ_data.Count() - ctrlZ_idx);
                    ctrlZ_idx = ctrlZ_data.Count();
                }
                ctrlZ_data.Add(new TextboxData(origin_text, origin_start, origin_length));
                ++ctrlZ_idx;
            }
        }

        private void ItalicsClicked(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length == 0)
            {
                return;
            }

            FormattingClicked(sender, e);
            string finaltext = richTextBox1.Text;
            var start = richTextBox1.SelectionStart;
            var length = richTextBox1.SelectionLength;

            int origin_start = start;
            int origin_length = length;
            string origin_text = finaltext;

            if (length == 0)
            {
                return;
            }

            //Check if we are selecting the asterix's as well
            if (finaltext.Length >= start + 1 && finaltext[start] == '*')
            {
                start++;
                length--;
            }
            if (finaltext.Length >= start + 1 && start + length - 1 >= 0 && finaltext[start+length-1] == '*')
            {
                length--;
            }


            int add = 0;
            if (start == 0 || finaltext[start-1] != '*' || (start >=2 && finaltext[start-2] == '*')) //Cannot already be italics, but can be bold
            {
                if (((GetStyles(start) & Style.Italics) == Style.Italics) && ((GetStyles(start + length) & Style.Italics) != Style.Italics)) //Is the start already in italics, and the end ISN'T?
                {
                    //Dont bother
                    ++start;
                    add--;
                }
                else if (((GetStyles(start) & Style.Italics) != Style.Italics) && ((GetStyles(start + length) & Style.Italics) == Style.Italics)) //Is the start NOT in italics, and the end is?
                {
                    add = 1;
                }
                else
                {
                    finaltext = finaltext.Insert(start, "*");
                    add = 1;
                }
            }
            else //It is already italics
            {
                finaltext = finaltext.Substring(0, start-1) + finaltext.Substring(start); //Remove the italics thing
                add = -1;
            }
            int endidx = start + length + add;
            if (endidx == finaltext.Length || finaltext[endidx] != '*' || (endidx <= finaltext.Length-2 && finaltext[endidx+1] == '*')) //Cannot be italics, but can be bold
            {
                if (((GetStyles(start) & Style.Italics) != Style.Italics) && ((GetStyles(start + length) & Style.Italics) == Style.Italics)) //Is the start NOT in italics, and the end is?
                {
                    finaltext = finaltext.Insert(start, "*");
                }
                else
                {
                    finaltext = finaltext.Insert(endidx, "*");
                }
            }
            else
            {
                finaltext = finaltext.Substring(0, endidx) + finaltext.Substring(endidx+1);
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
            var replace = finaltext.Replace("****", "**");
            if (finaltext != replace) //Moving stuffs
            {
                start -= 2;
                finaltext = replace;
                richTextBox1.Select(start, length);
            }
            var selectedtext = finaltext.Substring(start, length);
            selectedtext = selectedtext.Replace("**", "PLACEHOLDER_FOR_BOLD_TEXT_THINGY_LOTS_MORE_TEXT_THAT_THE_USER_WILL_HOPEFULLY_NEVER_TYPE");
            selectedtext = selectedtext.Replace("*", "");
            selectedtext = selectedtext.Replace("PLACEHOLDER_FOR_BOLD_TEXT_THINGY_LOTS_MORE_TEXT_THAT_THE_USER_WILL_HOPEFULLY_NEVER_TYPE", "**");
            finaltext = finaltext.Substring(0,start) + selectedtext + finaltext.Substring(start+length);
            
            length = selectedtext.Length;
            richTextBox1.Text = finaltext;
            richTextBox1.Select(start, length);

            richTextBox1.Focus();

            if (origin_text != finaltext) //Text changed?
            {
                if (ctrlZ_idx != ctrlZ_data.Count()) //Inserting item?
                {
                    ctrlZ_data.RemoveRange(ctrlZ_idx, ctrlZ_data.Count() - ctrlZ_idx);
                    ctrlZ_idx = ctrlZ_data.Count();
                }
                ctrlZ_data.Add(new TextboxData(origin_text, origin_start, origin_length));
                ++ctrlZ_idx;
            }
        }
        private void StrikeClicked(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length == 0)
            {
                return;
            }

            FormattingClicked(sender, e);
            string finaltext = richTextBox1.Text;
            var start = richTextBox1.SelectionStart;
            var length = richTextBox1.SelectionLength;

            int origin_start = start;
            int origin_length = length;
            string origin_text = finaltext;

            start = MoveOutsideTag(finaltext, start);
            length = MoveOutsideTag(finaltext, start + length) - start;

            if (length == 0)
            {
                return;
            }

            if (finaltext.Length >= start + 2 && finaltext.Substring(start, 2) == "~~") //Selected the asterix's as well?
            {
                start += 2;
                length -= 2;
            }
            if (finaltext.Length >= start + 2 && start + length - 2 >= 0 && finaltext.Substring(start + length - 2, 2) == "~~") //Selected the asterix's as well?
            {
                length -= 2;
            }

            if (((GetStyles(start) & Style.Strikethrough) == Style.Strikethrough) && ((GetStyles(start + length) & Style.Strikethrough) == Style.Strikethrough)) //All inside an underline?
            {
                finaltext = finaltext.Insert(start, "~~");
                finaltext = finaltext.Insert(start + length + 2, "~~");
                start += 2;
            }
            else if (((GetStyles(start) & Style.Strikethrough) == Style.Strikethrough) && !((GetStyles(start + length) & Style.Strikethrough) == Style.Strikethrough)) //First inside, but not second
            {
                //Make everything underlined
                string textselected = finaltext.Substring(start, length);
                var strikeMatches = Regex.Matches(textselected, @"\~\~").Cast<Match>().ToList();
                textselected = textselected.Replace("~~", ""); //Remove all the strike stuff inside the selection
                textselected += "~~"; //Add a closing tag

                finaltext = finaltext.Substring(0, start) + textselected + finaltext.Substring(start + length);

                foreach (Match match in strikeMatches)
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
            else if (!((GetStyles(start) & Style.Strikethrough) == Style.Strikethrough) && ((GetStyles(start + length) & Style.Strikethrough) == Style.Strikethrough)) //First outside, second inside
            {
                //Make everything strike
                string textselected = finaltext.Substring(start, length);
                var strikeMatches = Regex.Matches(textselected, @"\~\~").Cast<Match>().ToList();
                textselected = textselected.Replace("~~", "");
                textselected = "~~" + textselected; //Add a opening tag

                finaltext = finaltext.Substring(0, start) + textselected + finaltext.Substring(start + length);

                foreach (Match match in strikeMatches)
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
                finaltext = finaltext.Insert(start, "~~");
                finaltext = finaltext.Insert(start + length + 2, "~~");
                start += 2;
            }

            var doublestrikes = Regex.Matches(finaltext, @"\~\~\~\~").Cast<Match>().ToList();
            int totalremoved = 0;
            foreach (Match match in doublestrikes)
            {
                if (match.Index <= start)
                {
                    start -= 4;
                }
                else if (match.Index >= start && match.Index < start + length)
                {
                    length -= 4;
                }
                finaltext = finaltext.Substring(0, match.Index - totalremoved) + finaltext.Substring(match.Index + 4 - totalremoved);
                totalremoved += 4;
            }
            richTextBox1.Text = finaltext;

            richTextBox1.Select(start, length);
            richTextBox1.Focus();

            if (origin_text != finaltext) //Text changed?
            {
                if (ctrlZ_idx != ctrlZ_data.Count()) //Inserting item?
                {
                    ctrlZ_data.RemoveRange(ctrlZ_idx, ctrlZ_data.Count() - ctrlZ_idx);
                    ctrlZ_idx = ctrlZ_data.Count();
                }
                ctrlZ_data.Add(new TextboxData(origin_text, origin_start, origin_length));
                ++ctrlZ_idx;
            }

        }
        private void CodeClicked(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length == 0)
            {
                return;
            }

            FormattingClicked(sender, e);
            string finaltext = richTextBox1.Text;
            var start = richTextBox1.SelectionStart;
            var length = richTextBox1.SelectionLength;

            int origin_start = start;
            int origin_length = length;
            string origin_text = finaltext;

            start = MoveOutsideTag(finaltext, start);
            length = MoveOutsideTag(finaltext, start + length) - start;

            if (length == 0)
            {
                return;
            }

            if (finaltext.Length >= start + 3 && finaltext.Substring(start, 3) == "```") //Selected the asterix's as well?
            {
                start += 3;
                length -= 3;
            }
            if (finaltext.Length >= start + 3 && start + length - 3 >= 0 && finaltext.Substring(start + length - 3, 3) == "```") //Selected the asterix's as well?
            {
                length -= 3;
            }

            if (((GetStyles(start) & Style.Code) == Style.Code) && ((GetStyles(start + length) & Style.Code) == Style.Code)) //All inside an underline?
            {
                finaltext = finaltext.Insert(start, "```");
                finaltext = finaltext.Insert(start + length + 3, "```");
                start += 3;
            }
            else if (((GetStyles(start) & Style.Code) == Style.Code) && !((GetStyles(start + length) & Style.Code) == Style.Code)) //First inside, but not second
            {
                //Make everything underlined
                string textselected = finaltext.Substring(start, length);
                var codeMatches = Regex.Matches(textselected, @"\`\`\`").Cast<Match>().ToList();
                textselected = textselected.Replace("```", ""); //Remove all the code stuff inside the selection
                textselected += "```"; //Add a closing tag

                finaltext = finaltext.Substring(0, start) + textselected + finaltext.Substring(start + length);

                foreach (Match match in codeMatches)
                {
                    if (match.Index < start)
                    {
                        start -= 3;
                    }
                    else if (match.Index >= start && match.Index < start + length)
                    {
                        length -= 3;
                    }
                }//Modify cursor pos
                start += 3;
                length -= 3;
            }
            else if (!((GetStyles(start) & Style.Code) == Style.Code) && ((GetStyles(start + length) & Style.Code) == Style.Code)) //First outside, second inside
            {
                //Make everything code
                string textselected = finaltext.Substring(start, length);
                var codeMatches = Regex.Matches(textselected, @"\`\`\`").Cast<Match>().ToList();
                textselected = textselected.Replace("```", "");
                textselected = "```" + textselected; //Add a opening tag

                finaltext = finaltext.Substring(0, start) + textselected + finaltext.Substring(start + length);

                foreach (Match match in codeMatches)
                {
                    if (match.Index < start)
                    {
                        start -= 3;
                    }
                    else if (match.Index >= start && match.Index < start + length)
                    {
                        length -= 3;
                    }
                } //Modify cursor pos
                start += 3;
            }
            else //Neither are inside 
            {
                finaltext = finaltext.Insert(start, "```");
                finaltext = finaltext.Insert(start + length + 3, "```");
                start += 3;
            }

            var doublecodes = Regex.Matches(finaltext, @"\`\`\`\`\`\`").Cast<Match>().ToList();
            int totalremoved = 0;
            foreach (Match match in doublecodes)
            {
                if (match.Index <= start)
                {
                    start -= 6;
                }
                else if (match.Index >= start && match.Index < start + length)
                {
                    length -= 6;
                }
                finaltext = finaltext.Substring(0, match.Index - totalremoved) + finaltext.Substring(match.Index + 6 - totalremoved);
                totalremoved += 6;
            }
            richTextBox1.Text = finaltext;

            richTextBox1.Select(start, length);
            richTextBox1.Focus();

            if (origin_text != finaltext) //Text changed?
            {
                if (ctrlZ_idx != ctrlZ_data.Count()) //Inserting item?
                {
                    ctrlZ_data.RemoveRange(ctrlZ_idx, ctrlZ_data.Count() - ctrlZ_idx);
                    ctrlZ_idx = ctrlZ_data.Count();
                }
                ctrlZ_data.Add(new TextboxData(origin_text, origin_start, origin_length));
                ++ctrlZ_idx;
            }

        }
        private void UnderlineClicked(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length == 0)
            {
                return;
            }
            FormattingClicked(sender, e);
            string finaltext = richTextBox1.Text;
            var start = richTextBox1.SelectionStart;
            var length = richTextBox1.SelectionLength;

            int origin_start = start;
            int origin_length = length;
            string origin_text = finaltext;

            start = MoveOutsideTag(finaltext, start);
            length = MoveOutsideTag(finaltext, start+length)-start;
            if (length == 0)
            {
                return;
            }

            if (finaltext.Length >= start + 3 && finaltext.Substring(start, 3) == "<u>") //Have we selected the start of a tag?
            {
                start += 3;
                length -= 3;
            }

            if (((GetStyles(start) & Style.Underline) == Style.Underline) && ((GetStyles(start + length) & Style.Underline) == Style.Underline)) //All inside an underline?
            {
                finaltext = finaltext.Insert(start, "</u>");
                finaltext = finaltext.Insert(start + length + 4, "<u>");
                start += 4;
            }
            else if (((GetStyles(start) & Style.Underline) == Style.Underline) && !((GetStyles(start + length) & Style.Underline) == Style.Underline)) //First inside, but not second
            {
                //Make everything underlined
                string textselected = finaltext.Substring(start, length);
                var openingMatches = Regex.Matches(textselected, "<u>").Cast<Match>().ToList();
                var closingMatches = Regex.Matches(textselected, "</u>").Cast<Match>().ToList();
                textselected = textselected.Replace("<u>", "");
                textselected = textselected.Replace("</u>", ""); //Remove all the underline stuff inside the selection
                textselected += "</u>"; //Add a closing tag

                finaltext = finaltext.Substring(0, start) + textselected + finaltext.Substring(start + length);

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
                string textselected = finaltext.Substring(start, length);
                var openingMatches = Regex.Matches(textselected, "<u>").Cast<Match>().ToList();
                var closingMatches = Regex.Matches(textselected, "</u>").Cast<Match>().ToList();
                textselected = textselected.Replace("<u>", "");
                textselected = textselected.Replace("</u>", ""); //Remove all the underline stuff inside the selection
                textselected = "<u>" + textselected; //Add a closing tag

                finaltext = finaltext.Substring(0, start) + textselected + finaltext.Substring(start + length);

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
                finaltext = finaltext.Insert(start, "<u>");
                finaltext = finaltext.Insert(start + length + 3, "</u>");
                start += 3;
            }

            var matches = Regex.Matches(finaltext, "<u></u>").Cast<Match>().ToList();
            matches.AddRange(Regex.Matches(finaltext, "</u><u>").Cast<Match>().ToList());
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
            finaltext = finaltext.Replace("<u></u>", "");
            finaltext = finaltext.Replace("</u><u>", "");

            int totalremoved = 0;
            foreach (var match in UnusedTags(finaltext))
            {
                if (match.Index <= start)
                {
                    start -= match.length;
                }
                else if (match.Index >= start && match.Index < start + length)
                {
                    length -= match.length;
                }
                finaltext = finaltext.Substring(0,match.Index-totalremoved) + finaltext.Substring(match.Index + match.length-totalremoved);
                totalremoved += match.length;
            }
            richTextBox1.Text = finaltext;

            richTextBox1.Select(start, length);
            RemoveUnusedTags();
            richTextBox1.Focus();

            if (origin_text != finaltext) //Text changed?
            {
                if (ctrlZ_idx != ctrlZ_data.Count()) //Inserting item?
                {
                    ctrlZ_data.RemoveRange(ctrlZ_idx, ctrlZ_data.Count() - ctrlZ_idx);
                    ctrlZ_idx = ctrlZ_data.Count();
                }
                ctrlZ_data.Add(new TextboxData(origin_text, origin_start, origin_length));
                ++ctrlZ_idx;
            }
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
            try
            {
                toconvert = toconvert.Insert(start, "<div id=\"startofselection\"></div>"); //Create a div that we can easily find with its ID
            }
            catch
            {
                return Style.None;
            }
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
                        result |= Style.Strikethrough;
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
        private void QuoteClicked(object sender, EventArgs e)
        {
            FormattingClicked(sender, e);
            string finaltext = richTextBox1.Text;
            var start = richTextBox1.SelectionStart;
            var length = richTextBox1.SelectionLength;

            if (finaltext.Length >= start + 2 && finaltext.Substring(start, 2) == "> ") //Did the user select the quote thingy
            {
                start += 2; //de-select the quote thingy
                length -= 2;
            }
            if (finaltext.Length >= start + 3 && start >= 1 && finaltext.Substring(start-1, 2) == "> ") //Did the user select the quote thingy
            {
                start += 1; //de-select the quote thingy
                length -= 1;
            }

            if (((GetStyles(start) & Style.Quote) == Style.Quote) && ((GetStyles(start + length) & Style.Quote) == Style.Quote)) //All inside a quote?
            {
                bool selectedstart = false;
                if (start >= 2 && finaltext.Substring(start -2, 2) == "> ") //Just before us is a quote thingy?
                {
                    finaltext = finaltext.Substring(0, start - 2) + finaltext.Substring(start);
                    start-=2;
                    selectedstart = true;
                }

                string textselected = finaltext.Substring(start, length);
                textselected = textselected.Replace("> ", "");

                if (start + length == finaltext.Length)
                {
                    textselected = textselected + "\n";
                }
                else if (finaltext[start + length] != '\n')
                {
                    textselected = textselected + "\n> ";
                }

                if (!selectedstart)
                {
                    textselected = "\n" + textselected;
                }
                finaltext = finaltext.Substring(0, start) + textselected + finaltext.Substring(start + length);
                if (!selectedstart)
                {
                    ++start;
                }
            }
            else if (!((GetStyles(start) & Style.Quote) == Style.Quote) && !((GetStyles(start + length) & Style.Quote) == Style.Quote))//Neither are inside 
            {
                string textselected = finaltext.Substring(start, length); //Make everything part of the quote

                bool addedline = false;
                start -= Regex.Matches(textselected, "\n> ").Count*2;
                textselected = textselected.Replace("\n> ", "\n");
                start += Regex.Matches(textselected, "\n").Count*2;
                textselected = textselected.Replace("\n", "\n> ");
                if (start == 0 || finaltext[start - 1] == '\n') //At start of document or on a new line?
                {
                    textselected = "> " + textselected;
                }
                else
                {
                    textselected = "\n> " + textselected;
                    addedline = true;
                }

                finaltext = finaltext.Substring(0, start) + textselected + finaltext.Substring(start + length);
                if (addedline)
                {
                    start += 3;
                }
                else
                {
                    start += 2;
                }
            }
            richTextBox1.Text = finaltext;
            richTextBox1.Select(start, length);
            richTextBox1.Focus();
        }

        private int MoveOutsideTag(string text, int start)
        {
            bool insideHtml = false;
            bool inside_MD = false;

            for (int i = 0; i < start; ++i)
            {
                var c = text[i];
                if (c == '<') //Beggining inside tag
                {
                    insideHtml = true;
                }
                if (c=='>')
                {
                    insideHtml = false;
                }
                if (c=='*' && i <= text.Length-2 && text[i + 1] == '*') //Asterix after
                {
                    inside_MD = true;
                }
                else if (c == '~' && i <= text.Length - 2 && text[i + 1] == '~') //Asterix after
                {
                    inside_MD = true;
                }
                else if (c == '`' && i <= text.Length - 2 && text[i + 1] == '`') //Asterix after
                {
                    inside_MD = true;
                }
                else
                {
                    inside_MD = false;
                }
            }
            if (insideHtml)
            {
                return MoveOutsideTag(text, start-1); //Keep going until we are outside
            }
            if (inside_MD)
            {
                return MoveOutsideTag(text, start + 1);
            }
            return start;
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.B:
                        BoldClicked(Bold_button, null);
                        break;
                    case Keys.U:
                        UnderlineClicked(Underline_button, null);
                        break;
                    case Keys.I:
                        ItalicsClicked(Italics_button, null);
                        e.SuppressKeyPress = true;
                        break;
                    case Keys.Z:
                        Undo();
                        break;
                    case Keys.Y:
                        Redo();
                        break;
                    case Keys.S:
                        if (!e.Shift)
                        {
                            Save();
                        }
                        break;

                }
                if (e.Shift)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.S:
                            StrikeClicked(Strikethrough_button, null);
                            break;
                        case Keys.Q:
                            QuoteClicked(Quote_button, null);
                            break;
                        case Keys.C:
                            CodeClicked(Code_button, null);
                            break;
                    }
                }
            }
            else if (!e.Alt)
            {
               if (ctrlZ_idx != ctrlZ_data.Count()) //Inserting item?
                {
                    ctrlZ_data.RemoveRange(ctrlZ_idx, ctrlZ_data.Count() - ctrlZ_idx);
                    ctrlZ_idx = ctrlZ_data.Count();
                }
                ctrlZ_data.Add(new TextboxData(richTextBox1.Text, richTextBox1.SelectionStart, richTextBox1.SelectionLength));
                ++ctrlZ_idx;
            }
        }

        private void Save(bool showbox = true)
        {
            File.WriteAllBytes(Form2.filepath, Encryption.Encrypt(richTextBox1.Text, Form2.key));
            if (showbox) 
            {
                MessageBox.Show("File Saved");
            }
        }

        private void HeadingClicked(object sender, EventArgs e)
        {
            H0_button.Visible = true;
            H1_button.Visible = true;
            H2_button.Visible = true;
            H3_button.Visible = true;
            HeadingBackpanel.Visible = true;
        }

        private void H_button_Click(object sender, EventArgs e)
        {
            string number = HFormatting(sender);

            //Actually adding the headings MD
            int hashes = int.Parse(number.Substring(1));
            string toadd = new string('#', hashes) + ' ';
            if (hashes == 0)
            {
                toadd = "";
            }
            if (richTextBox1.Text.Length == 0)//Empty?
            {

                if (ctrlZ_idx != ctrlZ_data.Count()) //Inserting item?
                {
                    ctrlZ_data.RemoveRange(ctrlZ_idx, ctrlZ_data.Count() - ctrlZ_idx);
                    ctrlZ_idx = ctrlZ_data.Count();
                }
                ctrlZ_data.Add(new TextboxData(richTextBox1.Text, richTextBox1.SelectionStart, richTextBox1.SelectionLength));
                ++ctrlZ_idx;

                richTextBox1.Text = toadd + "Heading";
                richTextBox1.Select(toadd.Length, 7);
                richTextBox1.Focus();
                return;
            }

            var start = richTextBox1.SelectionStart;
            var length = richTextBox1.SelectionLength - (MoveToEndOfHeading(richTextBox1.Text, start) - start);

            int origin_start = start;
            int origin_length = length;
            string origin_text = richTextBox1.Text;

            start = MoveToEndOfHeading(richTextBox1.Text, start);

            if (length <= 0)
            {
                if (ctrlZ_idx != ctrlZ_data.Count()) //Inserting item?
                {
                    ctrlZ_data.RemoveRange(ctrlZ_idx, ctrlZ_data.Count() - ctrlZ_idx);
                    ctrlZ_idx = ctrlZ_data.Count();
                }
                ctrlZ_data.Add(new TextboxData(richTextBox1.Text, richTextBox1.SelectionStart, richTextBox1.SelectionLength));
                ++ctrlZ_idx;

                richTextBox1.Text = richTextBox1.Text.Insert(start, toadd);
                return;
            }
            if (richTextBox1.Text[start + length - 1] == '\n')
            {
                length--;
            }
            if (richTextBox1.Text.Substring(start, length).Count(s => s == '\n') >= 1)
            {
                if (ctrlZ_idx != ctrlZ_data.Count()) //Inserting item?
                {
                    ctrlZ_data.RemoveRange(ctrlZ_idx, ctrlZ_data.Count() - ctrlZ_idx);
                    ctrlZ_idx = ctrlZ_data.Count();
                }
                ctrlZ_data.Add(new TextboxData(richTextBox1.Text, richTextBox1.SelectionStart, richTextBox1.SelectionLength));
                ++ctrlZ_idx;

                return;
            }

            if (richTextBox1.Text.Length >= 2 && start >= 1 && richTextBox1.Text[start] == ' ' && richTextBox1.Text[start - 1] == '#') //Selected heading bit
            {
                ++start; //Deselct it
                --length;
            }
            if (start + length == richTextBox1.Text.Length || richTextBox1.Text[start + length + 1] == '\n') //Entire line highlighted?
            {
                if (start >= 2 && (richTextBox1.Text[start - 1] == ' ' && richTextBox1.Text[start - 2] == '#')) //Selected an entire heading? Modifying existing heading
                {
                    var lastline = richTextBox1.Text.LastIndexOf('\n', start); //Find where the \n is so that we can count the hashes
                    richTextBox1.Text = richTextBox1.Text.Substring(0, lastline + 1) + toadd + richTextBox1.Text.Substring(start);
                }
                else
                {
                    richTextBox1.Text = richTextBox1.Text.Insert(start, toadd);
                }
            }

            if (origin_text != richTextBox1.Text) //Text changed?
            {
                if (ctrlZ_idx != ctrlZ_data.Count()) //Inserting item?
                {
                    ctrlZ_data.RemoveRange(ctrlZ_idx, ctrlZ_data.Count() - ctrlZ_idx);
                    ctrlZ_idx = ctrlZ_data.Count();
                }
                ctrlZ_data.Add(new TextboxData(origin_text, origin_start, origin_length));
                ++ctrlZ_idx;
            }
        }

        private string HFormatting(object sender)
        {
            H0_button.Visible = false;
            H1_button.Visible = false;
            H2_button.Visible = false;
            H3_button.Visible = false;
            HeadingBackpanel.Visible = false;

            var button = ((Button)sender);
            string number = button.Name.Split('_')[0];
            HeadingLabel.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject(number);
            switch (number)
            {
                case "H0":
                    H0_button.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("H0_select");
                    H1_button.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("H1_button");
                    H2_button.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("H2_button");
                    H3_button.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("H3_button");
                    break;
                case "H1":
                    H0_button.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("H0_button");
                    H1_button.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("H1_select");
                    H2_button.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("H2_button");
                    H3_button.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("H3_button");
                    break;
                case "H2":
                    H0_button.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("H0_button");
                    H1_button.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("H1_button");
                    H2_button.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("H2_select");
                    H3_button.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("H3_button");
                    break;
                case "H3":
                    H0_button.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("H0_button");
                    H1_button.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("H1_button");
                    H2_button.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("H2_button");
                    H3_button.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("H3_select");
                    break;
            }

            return number;
        }

        public int MoveToEndOfHeading(string text, int start)
        {
            if (text[start] == '#') //In a heading?
            {
                return MoveToEndOfHeading(text, start+1);
            }
            return start;
        }

        private void Image_button_Click(object sender, EventArgs e)
        {
            if (ctrlZ_idx != ctrlZ_data.Count()) //Inserting item?
            {
                ctrlZ_data.RemoveRange(ctrlZ_idx, ctrlZ_data.Count() - ctrlZ_idx);
                ctrlZ_idx = ctrlZ_data.Count();
            }
            ctrlZ_data.Add(new TextboxData(richTextBox1.Text, richTextBox1.SelectionStart, richTextBox1.SelectionLength));
            ++ctrlZ_idx;


            var start = richTextBox1.SelectionStart;
            richTextBox1.Text = richTextBox1.Text.Insert(start, "\n![caption](address)\n");
            start += 12;
            richTextBox1.Select(start, 7);
            richTextBox1.Focus();
        }

        private void Link_button_Click(object sender, EventArgs e)
        {
            if (ctrlZ_idx != ctrlZ_data.Count()) //Inserting item?
            {
                ctrlZ_data.RemoveRange(ctrlZ_idx, ctrlZ_data.Count() - ctrlZ_idx);
                ctrlZ_idx = ctrlZ_data.Count();
            }
            ctrlZ_data.Add(new TextboxData(richTextBox1.Text, richTextBox1.SelectionStart, richTextBox1.SelectionLength));
            ++ctrlZ_idx;

            var start = richTextBox1.SelectionStart;
            richTextBox1.Text = richTextBox1.Text.Insert(start, "\n[text](link)\n");
            start += 8;
            richTextBox1.Select(start, 4);
            richTextBox1.Focus();
        }

        private void Font_button_Click(object sender, EventArgs e)
        {
            FormattingClicked(sender, e);
            var result = fontDialog1.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }
            var font = fontDialog1.Font;

            string toplace = "<div>";
            if (font.Underline)
            {
                toplace += "<u>";
            }
            if (font.Strikeout)
            {
                toplace += "~~";
            }
            toplace += string.Format("<p style=\"font-family:{0};font-size:{1}px;font-weight:{2};font-style:{3}\">", font.Name, font.Size, font.Bold ? "bold" : "normal", font.Italic ? "italic" : "normal");

            var start = richTextBox1.SelectionStart;
            var length = richTextBox1.SelectionLength;

            int origin_start = start;
            int origin_length = length;
            string origin_text = richTextBox1.Text;

            var selectedfont = CustomFont();
            int toshift = toplace.Length;

            if (selectedfont != null) //Is the user modifying an existing thingy
            {
                string before = richTextBox1.Text.Substring(0,start);
                int startidx = before.LastIndexOf("<div>");
                int newstart = richTextBox1.Text.LastIndexOf("<div>", start) + ((toplace.Length + richTextBox1.Text.LastIndexOf("<div>", start)) - startidx);

                int endIndex = richTextBox1.Text.LastIndexOf("\">", start) + 2;
                string usertext = richTextBox1.Text.Substring(endIndex, richTextBox1.Text.IndexOf("</p>", start)-endIndex);
                toplace += usertext;
                toplace += "</p>";

                if (font.Strikeout)
                {
                    toplace += "~~";
                }
                if (font.Underline)
                {
                    toplace += "</u>";
                }
                toplace += "</div>";

                richTextBox1.Text = richTextBox1.Text.Substring(0, startidx) + toplace + richTextBox1.Text.Substring(richTextBox1.Text.IndexOf("</div>", startidx)+6);

                start = newstart;
            }
            else if (length == 0)
            {
                toplace += "text goes here</p>";
                if (font.Strikeout)
                {
                    toplace += "~~";
                }
                if (font.Underline)
                {
                    toplace += "</u>";
                }
                toplace += "</div>";
                richTextBox1.Text = richTextBox1.Text.Insert(start, toplace);

                start += toshift;
                length = 14;
            }
            else
            {
                toplace += richTextBox1.Text.Substring(start, length); //Add the users text
                toplace += "</p>";
                if (font.Strikeout)
                {
                    toplace += "~~";
                }
                if (font.Underline)
                {
                    toplace += "</u>";
                }
                toplace += "</div>";

                richTextBox1.Text = richTextBox1.Text.Substring(0, start) + toplace + richTextBox1.Text.Substring(start + length);

                start += toshift;
            }
            richTextBox1.Select(start, length);
            richTextBox1.Focus();

            if (origin_text != richTextBox1.Text) //Text changed?
            {
                if (ctrlZ_idx != ctrlZ_data.Count()) //Inserting item?
                {
                    ctrlZ_data.RemoveRange(ctrlZ_idx, ctrlZ_data.Count() - ctrlZ_idx);
                    ctrlZ_idx = ctrlZ_data.Count();
                }
                ctrlZ_data.Add(new TextboxData(origin_text, origin_start, origin_length));
                ++ctrlZ_idx;
            }
        }
        public HtmlAgilityPack.HtmlNode CustomFont()
        {
            var toconvert = richTextBox1.Text;
            toconvert = toconvert.Insert(richTextBox1.SelectionStart, "<div id=\"startofselection\"></div>"); //Create a div that we can easily find with its ID

            var customMarkdown = new CustomMarkdown(toconvert);
            var html = customMarkdown.GetHtml();
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            var elementA = doc.GetElementbyId("startofselection");

            if (elementA != null && elementA.PreviousSibling != null && elementA.PreviousSibling.Name == "p")
            {
                return elementA.PreviousSibling;
            }
            return null;
        }

        private void HeadingBackpanel_Click(object sender, EventArgs e)
        {
            H0_button.Visible = false;
            H1_button.Visible = false;
            H2_button.Visible = false;
            H3_button.Visible = false;
            HeadingBackpanel.Visible = false;
        }

        #region ControlZHandling
        public class TextboxData
        {
            public string Text;
            public int SelectionStart;
            public int SelectionLength;

            public TextboxData(string text, int selectionStart, int selectionLength)
            {
                Text = text;
                SelectionStart = selectionStart;
                SelectionLength = selectionLength;
            }
        }
        public List<TextboxData> ctrlZ_data = new List<TextboxData>();
        int ctrlZ_idx = 0;

        public void Undo()
        {
            if (ctrlZ_idx == 0)
            {
                return;
            }
            ctrlZ_data.Add(new TextboxData(richTextBox1.Text, richTextBox1.SelectionStart, richTextBox1.SelectionLength));
            ctrlZ_idx--;
            var item = ctrlZ_data[ctrlZ_idx];
            richTextBox1.Text = item.Text;
            richTextBox1.Select(item.SelectionStart, item.SelectionLength);
            richTextBox1.Focus();
        }
        public void Redo()
        {
            if (ctrlZ_idx >= ctrlZ_data.Count()-1)
            {
                return;
            }

            ctrlZ_idx++;
            var item = ctrlZ_data[ctrlZ_idx];
            richTextBox1.Text = item.Text;
            richTextBox1.Select(item.SelectionStart, item.SelectionLength);
            richTextBox1.Focus();
        }
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            var md = richTextBox1.Text;
            CustomMarkdown customMarkdown = new CustomMarkdown(md);
            string v = customMarkdown.GetHtml();
            webBrowser1.DocumentText = v;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }
    }
}