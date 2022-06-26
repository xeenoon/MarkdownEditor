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
            //string result = "<style>ol,ul { padding: 0 0 100 0; margin: 0 0 100 0;}</style>"; //css
            string result = "";
            result += rawdata;
            result = Regex.Replace(result, "\n\n", "<br>");

            result = result.RemoveBlockQuotes();
            result = result.ReplaceLists();
            result = result.RemoveAsterixs();
            result = result.RemoveHeadings();
            result = result.RemoveCode();
            result = result.RemoveLinks();
            result = result.RemoveImages();
            //Remove \'s
            result = result.RemoveSlashes(); ;

            return result;
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
        public static string ReplaceLast(this string text, string search, string replace)
        {
            int pos = text.LastIndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
        public static int ClosingTag(this string text, string tag, int idx)
        {
            string closingtag = string.Format("</{0}>", tag.Substring(1, tag.Length-2));
            var openings = Regex.Matches(text, tag).Cast<Match>().ToList().Where(m=>m.Index > idx);
            var closings = Regex.Matches(text, closingtag).Cast<Match>().ToList().Where(m => m.Index > idx);

            Dictionary<int, bool> tagidxs = new Dictionary<int, bool>(); ;
            foreach (var opening in openings)
            {
                tagidxs.Add(opening.Index, true);
            }
            foreach (var closing in closings)
            {
                tagidxs.Add(closing.Index, false);
            }

            int totalopenings = 1;
            int totalclosings = 0;
            foreach (var item in tagidxs.OrderBy(t=>t.Key)) //Order all the tags by the order they appear in the string
            {
                if (item.Value == true) //Is it an opening
                {
                    ++totalopenings;
                }
                else
                {
                    ++totalclosings;
                }
                if (totalopenings == totalclosings)
                {
                    return item.Key; //Return the final closing one
                }
            }
            return text.Length-1; //No closing one found
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
                if (lastthree.Contains("***") && (i == 2 || processString[i-3] != '\\')) //All asterixes, bold and italics. Also check for escaping '\'
                {
                    string before = processString.Substring(0, i - 2); //Find everything before the asterixes
                    string after = processString.Substring(i + 1); //Get the stuff that comes afterwards
                    if (after.Contains("***")) //Will there be a closing set?
                    {
                        return RemoveAsterixs((before + "<b><i>" + after).ReplaceTrailingAsterixs("***", "</b></i>"));
                    }
                    //Continue, check to see if there is a closing set for the other types
                }
                if (lastthree.Contains("**") && processString[i + 1] != '*' && processString[i-2] != '\\')
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
                if (c == '*' && processString[i + 1] != '*' && processString[i-1] != '\\')
                //Is it just this one that is an asterix?
                {
                    string before = processString.Substring(0, i); //Find everything before the asterix
                    string after = processString.Substring(i + 1); //Get the stuff that comes afterwards
                    if (after.Contains("*")) //Will there be a closing set?
                    {
                        return RemoveAsterixs((before + "<i>" + after).ReplaceTrailingAsterixs("*", "</i>"));
                    }
                }

                if (lastthree.Contains("~~") && processString[i-2] != '\\')
                //Two crossouts, must be at end. Will have already fired if it was at the start
                {
                    string before = processString.Substring(0, i - 1); //Find everything before the two asterixes
                    string after = processString.Substring(i + 1); //Get the stuff that comes afterwards
                    if (after.Contains("~~")) //Will there be a closing set?
                    {
                        return RemoveAsterixs((before + "<strike>" + after).ReplaceTrailingAsterixs("~~", "</strike>"));
                    }
                    //Continue, check to see if there is a closing set for the other types
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

                string before = text.Substring(0, startidx+1);
                int hashes = before.TrailingHashes();

                string removeHashes = before.Substring(0, before.Length - hashes);

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
                    after += string.Format("</h{0}>\n", hashes);
                }
                return RemoveHeadings(removeHashes + after);

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
                    if (after.Contains("\n> ") && after.IndexOf("\n") == after.IndexOf("\n> ")) //Is the quote multiline, is the next quote part in the very next line
                    {
                       int[] positions = new int[2] { after.IndexOf("\n", after.LastQuote() + 1), after.IndexOf("<br>", after.LastQuote() + 1) };
                       for (int idx = 0; idx < positions.Length; idx++)
                       {
                           int item = positions[idx];
                           if (item == -1)
                           {
                               positions[idx] = after.Length;
                           }
                       }
                        var end = positions.OrderBy(it => it).FirstOrDefault();

                        string quote = "<blockquote style=\"background: #f9f9f9; border-left: 10px solid #ccc; margin: 1.5em 10px; padding: 0.2em 10px 0.1em 10px;\">" + Regex.Replace(after.Substring(0, end), "\n> ", "<br>") + "</blockquote>";
                        return RemoveBlockQuotes(before + quote + after.Substring(end));
                    }
                    else
                    {
                        //One line quote?
                        int[] positions = new int[2] { after.IndexOf("\n"), after.IndexOf("<br>") };
                        for (int idx = 0; idx < positions.Length; idx++)
                        {
                            int item = positions[idx];
                            if (item == -1)
                            {
                                positions[idx] = after.Length;
                            }
                        }
                        var end = positions.OrderBy(it => it).FirstOrDefault();


                        string quote = "<blockquote style=\"background: #f9f9f9; border-left: 10px solid #ccc; margin: 0.5em 10px; padding: 0.2em 10px 0.1em 10px;\">" + Regex.Replace(after.Substring(0, end), "\n> ", "<br>") + "</blockquote>";
                        return RemoveBlockQuotes(before + quote + after.Substring(end));
                    }
                }
            }

            //No change?
            return text; //Just return the input
        }
        public static int LastQuote(this string text, int startidx = 0)
        {
            int lastquotestart = 0;
            for(int i = startidx; i < text.Length-2; ++i)
            {
                if (text[i] == '\n')
                {
                    if (text[i+1] == '>' && text[i+2] == ' ')
                    {
                        lastquotestart = i;
                    }
                    else
                    {
                        return lastquotestart;
                    }
                }
                if (text.Length-i >= 4 &&  text.Substring(i,4) == "<br>")
                {
                    return lastquotestart;
                }
            }
            return lastquotestart;
        }
        #endregion
        #region List
        public enum ListType
        {
            Numbered,
            Bullets
        }
        public static string ReplaceLists(this string text)
        {
            if (text.Contains("- ")) //Contains bullets?
            {
                int startidx = 0;
                RetryStartIdx:
                startidx = text.IndexOf("- ", startidx);
                if (startidx == -1)
                {
                    goto NumberList;
                }
                if (startidx == text.Length-3 || !char.IsLetter(text[startidx+2]))
                {
                    ++startidx;
                    goto RetryStartIdx;
                }
                int endoflist = EndOfList(ref text, ListType.Bullets, startidx);
                string before = text.Substring(0, startidx);
                string after = text.Substring(endoflist);
                string list = text.Substring(startidx, endoflist - startidx).RemoveBullettedListItems();
                list = before + "<ul style=\"margin: 0 0 0 0;padding: 5 0 0 20\">" + list + "</ul>" + after;
                return ReplaceLists(list);
            }
            NumberList:
            if (text.Contains("1. ")) //Contains a numbered list?
            {
                int startidx = text.IndexOf("1. ");
                int endoflist = EndOfList(ref text, ListType.Numbered, startidx);
                string before = text.Substring(0, startidx);
                string after = text.Substring(endoflist);
                string list = text.Substring(startidx, endoflist - startidx).RemoveNumberedListItems();
                list = before + "<ol style=\"margin: 0 0 0 0;padding: 5 0 0 20\">" + list + "</ol>" + after;
                return ReplaceLists(list);
            }
            return text;
        }
        public static int EndOfList(ref string text, ListType listType, int startidx)
        {
            int lastidx = text.Length;
            for (int i = startidx; i < text.Length - 2; i++)
            {
                char c = text[i];
                if (char.IsNumber(c) && text[i + 1] == '.' && text[i + 2] == ' ') //Is it a list item?
                {
                    if (listType == ListType.Bullets)
                    {
                        break;
                    }
                    lastidx = i;
                }
                else if (c == '-' && text[i + 1] == ' ')
                {
                    lastidx = i;
                }
                else if (c == '<' && text.Length - i >= 5 && text.Substring(i, 5) == "</ul>")
                {
                    lastidx = i;
                }
                else if (c == '\\') //Indicating end of a list?
                {
                    lastidx = i;
                    text = text.Substring(0,i) + "\n" + text.Substring(i+1); //Remove the '\'
                    break; //End immediatly
                }
            }
            int[] positions = new int[2] { text.IndexOf("\n", lastidx), text.IndexOf("<br>", lastidx) };
            for (int idx = 0; idx < positions.Length; idx++)
            {
                int item = positions[idx];
                if (item == -1)
                {
                    positions[idx] = text.Length;
                }
            }
            lastidx = positions.OrderBy(it => it).FirstOrDefault();
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
                    int[] positions = new int[2] { text.IndexOf("\n", i), text.IndexOf("<br>", i) };
                    for (int idx = 0; idx < positions.Length; idx++)
                    {
                        int item = positions[idx];
                        if (item == -1)
                        {
                            positions[idx] = text.Length;
                        }
                    }
                    var end = positions.OrderBy(it=>it).FirstOrDefault();
                    string after = text.Substring(end);
                    string replace = "<li>" + text.Substring(i + 2, end - i - 2) + "</li>";
                    return RemoveNumberedListItems(before + replace + after);
                }
            }
            return text;
        }
        public static string RemoveBullettedListItems(this string text)
        {
            for (int i = 0; i < text.Length - 2; i++)
            {
                char c = text[i];
                if (c == '-' && text[i + 1] == ' ') //Is it a list item?
                {
                    string before = text.Substring(0, i);
                    int[] positions = new int[2] { text.IndexOf("\n", i), text.IndexOf("<br>", i) };
                    for (int idx = 0; idx < positions.Length; idx++)
                    {
                        int item = positions[idx];
                        if (item == -1)
                        {
                            positions[idx] = text.Length;
                        }
                    }
                    var end = positions.OrderBy(it => it).FirstOrDefault();
                    string after = text.Substring(end);
                    string replace = "<li>" + text.Substring(i + 2, end - i - 2) + "</li>";
                    return RemoveBullettedListItems(before + replace + after);
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
        #region Links
        public static string RemoveLinks(this string text, int beginat = 0)
        {
            if (text.Contains("https://"))
            {
                int startidx = 0;
                retrypoint:
                startidx = text.IndexOf("https://", startidx);
                if (startidx != 0 && (text[startidx-1] == '(' || text[startidx - 1] == '\"' || (startidx >= 2 && text[startidx - 2] == '\"')))
                {
                    int ProcessedLinks = Regex.Matches(text, "\\(https://").Count + Regex.Matches(text, "\"https://").Count + Regex.Matches(text, "\">https://").Count;
                    int AllLinks = Regex.Matches(text, "https://").Count;
                    if (AllLinks - ProcessedLinks >= 1) //Is there actually a real link?
                    {
                        startidx = text.IndexOf("https://", startidx + 1);
                        goto retrypoint;
                    }
                    else
                    {
                        goto embedlinks;
                    }
                }
                int endidx;
                int[] positions = new int[3] { text.IndexOf("\n", startidx), text.IndexOf(" ", startidx), text.IndexOf("<br>", startidx) };
                for (int i = 0; i < positions.Length; i++)
                {
                    int item = positions[i];
                    if (item == -1)
                    {
                        positions[i] = text.Length;
                    }
                }
                endidx = positions.OrderBy(i=>i).FirstOrDefault();
                string link = text.Substring(startidx, endidx-startidx);
                string html = string.Format("<a href=\"{0}\">{0}</a>", link);
                string before = text.Substring(0,startidx);
                string after = text.Substring(endidx);
                return RemoveLinks(before + html + after);
            }
            embedlinks:
            if (text.Contains("[") && text.Contains("](") && text.Contains(")"))
            {
                int imageidx = text.IndexOf("[", beginat);
                if (imageidx == -1)
                {
                    return text;
                }
                if (imageidx != 0 && text[imageidx-1]=='!')
                {
                    //Its an image
                    return RemoveLinks(text, imageidx+2);
                }
                int nameEndIdx = text.IndexOf("](", imageidx);
                string name = text.Substring(imageidx + 1, nameEndIdx - imageidx - 1);

                int closingBracketIdx = text.IndexOf(")", nameEndIdx);
                string imagelink = text.Substring(nameEndIdx + 2, closingBracketIdx - nameEndIdx - 2);
                if (!imagelink.StartsWith("https"))
                {
                    imagelink = "https://" + imagelink;
                }

                string result = string.Format("<a href=\"{0}\">{1}</a>", imagelink, name);
                string before = text.Substring(0, imageidx);
                string after = text.Substring(closingBracketIdx + 1);
                return RemoveImages(before + result + after);
            }
            return text;
        }
        #endregion
        public static string RemoveSlashes(this string s)
        {
            string buffer = "";
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (c != '\\')
                {
                    buffer += c;
                }
                else if (i != s.Length-1 && s[i+1] == '\\') //Double slashes?
                {
                    buffer += c;
                }
            }
            return buffer;
        }
    }
}