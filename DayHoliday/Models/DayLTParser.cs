using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DayHoliday.Models
{
    public class DayLTParser
    {
        public HtmlDocument doc { get; set; }
        /// <summary>
        /// Loads page with specified encoding.
        /// </summary>
        /// <param name="page">page to load.</param>
        /// <param name="encoding">encoding to use.</param>
        private void loadPage(string page, string encoding)
        {
            HtmlWeb web = new HtmlWeb();
            // Set encoding.
            web.OverrideEncoding = Encoding.GetEncoding(encoding);
            // Load page.
            doc = web.Load(page);
        }
        /// <summary>
        /// Parse holiday names from "day.lt" website at the specified date.
        /// </summary>
        /// <param name="date">date.</param>
        /// <returns>List of Strings which represent holidays at the specified date.</returns>
        public List<String> parseHolidays(string date)
        {
            loadPage("http://day.lt/diena/" + date, "windows-1257");
            List<String> holidays = new List<String>();
            try
            {
                // Find nodes.
                HtmlNode node = doc.DocumentNode.SelectSingleNode("//p[@class='sventes']");
                foreach (HtmlNode node2 in node.SelectNodes(".//span"))
                {
                    string holiday = node2.InnerText;
                    // Add parsed text.
                    holidays.Add(holiday);
                }
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }
            // Return found holidays.
            return holidays;
        }
    }
}