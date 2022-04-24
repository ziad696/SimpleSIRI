using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace SitefinityWebApp.MVC.Controllers.Tools
{
    public class Slugify
    {
        // Used to set the URLs for DynamicContent items.
        public static string UrlNameCharsToReplace = @"[^\w\-\!\$\'\(\)\=\@\d_]+";
        public static string UrlNameReplaceString = "-";

        public static string Generate(string phrase)
        {
            // Remove all accents and make the string lower case.  
            string output = RemoveAccents(phrase).ToLower();

            // Remove all special characters from the string.  
            output = Regex.Replace(output, @"[^A-Za-z0-9\s-]", "");

            // Remove all additional spaces in favour of just one.  
            output = Regex.Replace(output, @"\s+", " ").Trim();

            // Replace all spaces with the hyphen.  
            output = Regex.Replace(output, @"\s", "-");

            // Return the slug.  
            return output;
        }

        public static string RemoveAccents(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            char[] chars = text
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c)
                != UnicodeCategory.NonSpacingMark).ToArray();

            return new string(chars).Normalize(NormalizationForm.FormC);
        }
    }
}