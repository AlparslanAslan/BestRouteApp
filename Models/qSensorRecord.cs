using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BestWayWebApp.Models
{
    public class qSensorRecord
    {
        public  DataTable getRecords()
        {
            DataTable dt = new DataTable();
            string connString = @"server=LAPTOP-0I74MJKS;database= database_test;Trusted_Connection=true;";
            string query = "select top(4) * from Records";

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            conn.Close();
            da.Dispose();

            return dt;
        }
    }
}
