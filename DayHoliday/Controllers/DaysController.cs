using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DayHoliday.Models;
using System.Data;

namespace DayHoliday.Controllers
{
    public class DaysController : ApiController
    {
        // New handler for Database.
        private DBHandling dbHandler = new DBHandling(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\Alex\Documents\visual studio 2013\Projects\DayHoliday\DayHoliday\App_Data\Database.mdf;Integrated Security=True");
        // New day.lt parser.
        private DayLTParser dayLTparser = new DayLTParser();

        public IHttpActionResult getDay(String day)
        {
            DateTime dt;
            if (DateTime.TryParse(day, out dt))
            {
                string date = dt.ToString("yyyy.MM.dd");
                DataTable data = dbHandler.executeStatement("SELECT Holiday FROM Holidays WHERE DAY = '" + date + "'");
                // If data is null that means exception was thrown at executeStatement() function. Return NotFound();
                if (data == null)
                {
                    return NotFound();
                }
                // If there is data at that day in database.
                if (data.Rows.Count > 0)
                {
                    // Create List of days.
                    List<Day> days = new List<Day>();
                    string holiday;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        // Check if holiday in database is null.
                        if (data.Rows[i][0].ToString() == "")
                        {
                            // If it's null in database set holiday string as 'NULL'.
                            holiday = "NULL";
                        }
                        else
                        {
                            // If it's not null in database, copy string.
                            holiday = data.Rows[i][0].ToString();
                        }
                        // Add day.
                        days.Add(new Day { Date = date, Holiday = holiday });
                    }
                    // Return OK with a days list.
                    return Ok(days);
                }
                // Otherwise.
                else
                {
                    // Create string of holidays from parsing day.lt website.
                    List<String> holidays = dayLTparser.parseHolidays(date);
                    // Create new list of days.
                    List<Day> days = new List<Day>();
                    // If parsing returned more than 0 holidays at that day.
                    if (holidays.Count > 0)
                    {
                        // Go through each holiday.
                        foreach (string holiday in holidays)
                        {
                            // Add day records for the response.
                            days.Add(new Day { Date = date, Holiday = holiday });
                            // Try to insert holiday into database.
                            if (null == dbHandler.executeStatement("INSERT INTO Holidays VALUES ('" + holiday + "', '" + date + "')"))
                            {
                                // If null exception was thrown at executeStatement() function. Return NotFound().
                                return NotFound();
                            }
                        }
                    }
                    // If there are no holidays on that day.
                    else
                    {
                        // Add day record with a string 'NULL' for holiday.
                        days.Add(new Day { Date = date, Holiday = "NULL" });
                        // Try to insert holiday into database.
                        if (null == dbHandler.executeStatement("INSERT INTO Holidays VALUES (NULL, '" + date + "')"))
                        {
                            // If null exception was thrown at executeStatement() function. Return NotFound().
                            return NotFound();
                        }
                    }
                    // Return OK with a days list.
                    return Ok(days);
                }
            }
            // If the GET input was wrong return NotFound().
            return NotFound();
        }
    }
}
