using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DayHoliday.Models
{
    public class DBHandling
    {
        private SqlConnection Con { get; set; }

        public DBHandling(string conString)
        {
            // Create new connection with connection string.
            Con = new SqlConnection(conString);
        }
        /// <summary>
        /// Executes statement and return results in DataTable or null if exception was thrown.
        /// </summary>
        /// <param name="statement">SQL statement to execute.</param>
        /// <returns>DataTable or null if exception was thrown.</returns>
        public DataTable executeStatement(string statement)
        {
            // Create SqlDataAdapter.
            SqlDataAdapter sda = new SqlDataAdapter(statement, Con);
            DataTable dt = new DataTable();

            try
            {
                // Try to execute statement and fill DataTable.
                sda.Fill(dt);
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
                // Return null if exception was thrown.
                return null;
            }
            // Return DataTable.
            return dt;
        }
    }
}