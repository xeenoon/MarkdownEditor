﻿using System;
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
            string result = rawdata.RemoveAsterixs();
            result = Regex.Replace(result, "\n", "<br>");
            result = result.RemoveHeadings();
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

        #region Asterix
        public static string RemoveAsterixs(this string processString)
        {
            processString = "  " + processString; //Add two on the start of it, to allow iterations with 'lastthree'

            //Start at 2 to avoid an error
            //Finish one before the end to stop overflow
            for (int i = 2; i < processString.Length-1; i++)
            {
                char c = processString[i];

                string lastthree = processString.Substring(i - 2, 3);
                if (lastthree.Contains("***")) //All asterixes, bold and italics
                {
                    string before = processString.Substring(0, i - 2); //Find everything before the asterixes
                    string after = processString.Substring(i+1); //Get the stuff that comes afterwards
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
                    string after = processString.Substring(i+1); //Get the stuff that comes afterwards
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
                    string after = processString.Substring(i+1); //Get the stuff that comes afterwards
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

                string before = text.Substring(0, startidx+1);
                int hashes = before.TrailingHashes();

                string removeHashes = before.Substring(0, before.Length - hashes);
                if (removeHashes == "" || removeHashes.EndsWith("<br>")) //Is this a new line? Or is it the first line
                {
                    removeHashes = removeHashes + string.Format("<h{0} style=\"margin:0\">",hashes); //Add the first tag
                    string after = text.Substring(startidx + 2).ReplaceFirst("<br>", string.Format("</h{0}>", hashes)); //Remove the '# '
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
                if (text[text.Length-i] != '#')
                {
                    return i-1;
                }
            }
            return text.Length;
        }
        #endregion
    }
}
