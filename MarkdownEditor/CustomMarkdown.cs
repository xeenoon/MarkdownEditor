using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MarkdownEditor
{
    public class CustomMarkdown
    {
        public string rawdata;

        public CustomMarkdown(string rawdata)
        {
            this.rawdata = rawdata;
        }

        public string GetHtml()
        {
            string result = "<head>" + rawdata;
            result = Regex.Replace(result, "\n\n", "<br>");

            result = result.RemoveBlockQuotes();
            result = result.ReplaceLists();
            result = result.RemoveAsterixs();
            result = result.RemoveHeadings();
            result = result.RemoveCode();
            result = result.RemoveImages();
            return result + "</head>";
        }
    }

    public static class CustomMarkdownExtensions
    {
        public static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        #region Asterix
        public static string RemoveAsterixs(this string processString)
        {
            processString = "  " + processString; //Add two on the start of it, to allow iterations with 'lastthree'

            //Start at 2 to avoid an error
            //Finish one before the end to stop overflow
            for (int i = 2; i < processString.Length - 1; i++)
            {
                char c = processString[i];

                string lastthree = processString.Substring(i - 2, 3);
                if (lastthree.Contains("***")) //All asterixes, bold and italics
                {
                    string before = processString.Substring(0, i - 2); //Find everything before the asterixes
                    string after = processString.Substring(i + 1); //Get the stuff that comes afterwards
                    if (after.Contains("***")) //Will there be a closing set?
                    {
                        return RemoveAsterixs((before + "<b><i>" + after).ReplaceTrailingAsterixs("***", "</b></i>"));
                    }
                    //Continue, check to see if there is a closing set for the other types
                }
                if (lastthree.Contains("**") && processString[i + 1] != '*')
                //Two asterixes, must be at end. Will have already fired if it was at the start
                //Next character CANNOT be a *, if it is then the top event should be fired next time around
                {
                    string before = processString.Substring(0, i - 1); //Find everything before the two asterixes
                    string after = processString.Substring(i + 1); //Get the stuff that comes afterwards
                    if (after.Contains("**")) //Will there be a closing set?
                    {
                        return RemoveAsterixs((before + "<b>" + after).ReplaceTrailingAsterixs("**", "</b>"));
                    }
                    //Continue, check to see if there is a closing set for the other types
                }
                if (c == '*' && processString[i + 1] != '*')
                //Is it just this one that is an asterix?
                {
                    string before = processString.Substring(0, i); //Find everything before the asterix
                    string after = processString.Substring(i + 1); //Get the stuff that comes afterwards
                    if (after.Contains("*")) //Will there be a closing set?
                    {
                        return RemoveAsterixs((before + "<i>" + after).ReplaceTrailingAsterixs("*", "</i>"));
                    }
                }
            }

            //No change?
            return processString.Trim(); //Return the original text
        }
        public static string ReplaceTrailingAsterixs(this string processString, string lookfor, string closingTag)
        {
            var result = processString.ReplaceFirst(lookfor, closingTag); //Remove the closing set of asterixs and replace it with the leading tags
            return result;
        }
        #endregion
        #region Headings
        public static string RemoveHeadings(this string text)
        {
            if (text.Contains("# ")) //Does it have headings?
            {
                int startidx = text.IndexOf("# ");

                string before = text.Substring(0, startidx + 1);
                int hashes = before.TrailingHashes();

                string removeHashes = before.Substring(0, before.Length - hashes);
                if (removeHashes == "" || removeHashes.EndsWith("<br>") || removeHashes.EndsWith("\n")) //Is this a new line? Or is it the first line
                {
                    string after = "";
                    if (removeHashes.EndsWith("<br>"))
                    {
                        removeHashes = removeHashes + string.Format("<h{0} style=\"margin:0\">", hashes); //Add the first tag
                        after = text.Substring(startidx + 2).ReplaceFirst("<br>", string.Format("</h{0}>", hashes)); //Remove the '# '
                    }
                    else
                    {
                        removeHashes = removeHashes + string.Format("<h{0} style=\"margin:0\">", hashes); //Add the first tag
                        after = text.Substring(startidx + 2).ReplaceFirst("\n", string.Format("</h{0}>", hashes)); //Remove the '# '
                    }
                    if (!after.Contains("</h")) //Has the end heading tag not been added?
                    {
                        //This means this is the last line
                        //Add trailing heading tag at the end instead
                        after += string.Format("</h{0}>", hashes);
                    }
                    return RemoveHeadings(removeHashes + after);
                }
            }

            //No change?
            return text; //Just return the input
        }
        public static int TrailingHashes(this string text)
        {
            for (int i = 1; i < text.Length; i++)
            {
                if (text[text.Length - i] != '#')
                {
                    return i - 1;
                }
            }
            return text.Length;
        }
        #endregion
        #region BlockQuote
        public static string RemoveBlockQuotes(this string text)
        {
            if (text.Contains("> ")) //Does it have blockquotes?
            {
                int startidx = text.IndexOf("> ");
                string before = text.Substring(0, startidx);
                string after = text.Substring(startidx + 2);

                if (before == "" || before.EndsWith("<br>") || before.EndsWith("\n")) //Is this a new line? Or is it the first line
                {
                    //This is a valid quoteblock
                    if (after.Contains("\n> ")) //Is the quote multiline
                    {
                        var end = after.IndexOf("\n", after.LastIndexOf("\n> ") + 1);
                        if (end == -1) //Is there no '\n'
                        {
                            end = after.Length; //Quote is at the end, just add it to the end
                        }
                        string quote = "<blockquote style=\"background: #f9f9f9; border-left: 10px solid #ccc; margin: 1.5em 10px; padding: 0.2em 10px 0.1em 10px;\">" + Regex.Replace(after.Substring(0, end), "\n> ", "<br>") + "</blockquote>";
                        return before + RemoveBlockQuotes(quote) + after.Substring(end);
                    }
                    else
                    {
                        //One line quote?
                        var end = after.IndexOf("\n");
                        if (end == -1) //Is there no '\n'
                        {
                            end = after.Length; //Quote is at the end, just add it to the end
                        }

                        string quote = "<blockquote style=\"background: #f9f9f9; border-left: 10px solid #ccc; margin: 0.5em 10px; padding: 0.2em 10px 0.1em 10px;\">" + Regex.Replace(after.Substring(0, end), "\n> ", "<br>") + "</blockquote>";
                        return before + RemoveBlockQuotes(quote) + after.Substring(end);
                    }
                }
            }

            //No change?
            return text; //Just return the input
        }
        #endregion
        #region List
        public static string ReplaceLists(this string text)
        {
            if (text.Contains("1. ")) //Contains a numbered list?
            {
                int startidx = text.IndexOf("1. ");
                int endoflist = EndOfList(text);
                string before = text.Substring(0, startidx);
                string after = text.Substring(endoflist);
                string list = text.Substring(startidx, endoflist - startidx).RemoveNumberedListItems();
                list = before + "<ol style=\"margin: 0 0 0 0;padding: 5 0 0 40\">" + list + "</ol>" + after;
                return ReplaceLists(list);
            }
            return text;
        }
        public static int EndOfList(string text)
        {
            int lastidx = text.Length;
            for (int i = 0; i < text.Length - 2; i++)
            {
                char c = text[i];
                if (char.IsNumber(c) && text[i + 1] == '.' && text[i + 2] == ' ') //Is it a list item?
                {
                    lastidx = i;
                }
            }
            if (text.IndexOf("\n", lastidx) == -1)
            {
                lastidx = text.IndexOf("<br>", lastidx);
                if (lastidx == -1)
                {
                    lastidx = text.Length;
                }
            } //Search for both "<br>" and "\n"
            else
            {
                lastidx = text.IndexOf("\n", lastidx);
            }
            return lastidx;
        }
        public static string RemoveNumberedListItems(this string text)
        {
            for (int i = 0; i < text.Length - 2; i++)
            {
                char c = text[i];
                if (char.IsNumber(c) && text[i + 1] == '.' && text[i + 2] == ' ') //Is it a list item?
                {
                    string before = text.Substring(0, i).Trim(new char[10] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' }); //Remove trailing numbers
                    int end = text.IndexOf("\n", i); //Find the end of the line
                    if (end == -1) //End of text?
                    {
                        end = text.Length;
                    }
                    string after = text.Substring(end);
                    string replace = "<li>" + text.Substring(i + 2, end - i - 2) + "</li>";
                    return RemoveNumberedListItems(before + replace + after);
                }
            }
            return text;
        }
        #endregion
        #region CodeBlock
        public static string RemoveCode(this string processString)
        {
            processString = "  " + processString; //Add two on the start of it, to allow iterations with 'lastthree'

            //Start at 2 to avoid an error
            //Finish one before the end to stop overflow
            for (int i = 2; i < processString.Length - 1; i++)
            {
                char c = processString[i];

                string lastthree = processString.Substring(i - 2, 3);
                if (lastthree.Contains("```")) //Is it code?
                {
                    string before = processString.Substring(0, i - 2); //Find everything before the tildas
                    string after = processString.Substring(i + 1); //Get the stuff that comes afterwards
                    if (after.Contains("```")) //Will there be a closing set?
                    {
                        return RemoveCode((before + "<code style=\"color: #000000; background-color: #E3E6E8; border-radius: 50; font-family: SFMono-Regular,Menlo,Monaco,Consolas,\"Liberation Mono\",\"Courier New\",monospace\"; word-wrap: break-word;\">" + after).ReplaceTrailingAsterixs("```", "</code>"));
                    }
                    //Continue, check to see if there is a closing set for the other types
                }
            }

            //No change?
            return processString.Trim(); //Return the original text
        }
        #endregion
        #region Images
        public static string RemoveImages(this string text)
        {
            if (text.Contains("![") && text.Contains("](") && text.Contains(")"))
            {
                int imageidx = text.IndexOf("![");
                int nameEndIdx = text.IndexOf("](", imageidx);
                string name = text.Substring(imageidx+2, nameEndIdx-imageidx-2);

                int closingBracketIdx = text.IndexOf(")", nameEndIdx);
                string imagelink = text.Substring(nameEndIdx+2, closingBracketIdx-nameEndIdx-2);

                string result = string.Format("<img src=\"{0}\" alt=\"{1}\">", imagelink, name);
                string before = text.Substring(0,imageidx);
                string after = text.Substring(closingBracketIdx+1);
                return RemoveImages(before + result + after);
            }
            return text;
        }
        #endregion
    }
}