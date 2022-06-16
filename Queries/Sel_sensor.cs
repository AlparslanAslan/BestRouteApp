using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BestWayWebApp.Models;
using Dapper;

namespace BestWayWebApp.Queries
{
    public class Sel_sensor
    {
        public DataTable sel_sensor()
        {
            DataTable dt = new DataTable();
            string connString = @"server=LAPTOP-0I74MJKS;database= database_test;Trusted_Connection=true;";
            string query = "select MaximumSpeed,MinimumSpeed,Geohash from SensorRecords";

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            conn.Close();
            da.Dispose();

            return dt;
        }
        public DataTable sel_tarih(DateTime date)
        {
            DataTable dt = new DataTable();
            string connString = @"server=LAPTOP-0I74MJKS;database= database_test;Trusted_Connection=true;";
            string query = "SELECT* FROM SensorRecords where date == "+date.ToString()+";";

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            conn.Close();
            da.Dispose();

            return dt;
        }
        public DataTable getRecords()
        {
            DataTable dt = new DataTable();
            string connString = @"server=LAPTOP-0I74MJKS;database= database_test;Trusted_Connection=true;";
            string query = "select * from Records";

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            conn.Close();
            da.Dispose();

            return dt;
        }
       
        public DataTable getSpeed()
        {
            DataTable dt = new DataTable();
            string connString = @"server=LAPTOP-0I74MJKS;database= database_test;Trusted_Connection=true;";
            string query = "select MaximumSpeed,MinimumSpeed,Geohash from SensorRecords";

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            conn.Close();
            da.Dispose();

            return dt;
        }

        public DataTable getVehicleNumberbyDate(DateTime date)
        {
            DataTable dt = new DataTable();
            string connString = @"server=LAPTOP-0I74MJKS;database= database_test;Trusted_Connection=true;";
            string query = "select VehicleNumber from SensorRecords where date ="+date+";";

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            conn.Close();
            da.Dispose();

            return dt;
        }

        public DataTable getSpeedByCoordinate(Coordinate coordinate)
        {
            DataTable dt = new DataTable();
            string connString = @"server=LAPTOP-0I74MJKS;database= database_test;Trusted_Connection=true;";
            string query = "select MinSpeed,MaxSpeed from SensorRecords where longitude =" + coordinate.longitude + "latitude = " + coordinate.latitude + ";";

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            conn.Close();
            da.Dispose();

            return dt;
        }
        public DataTable getCycleCoordinate()
        {
            DataTable dt = new DataTable();
            string connString = @"server=LAPTOP-0I74MJKS;database= database_test;Trusted_Connection=true;";
            string query = "select Longitude,Latitude,ParkName,Adres, kod from IsbikeParcs  ";

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            conn.Close();
            da.Dispose();

            return dt;
        }
        public DataTable getCoordinateByAdres(string cadde)
        {
            DataTable dt = new DataTable();
            string connString = @"server=LAPTOP-0I74MJKS;database= database_test;Trusted_Connection=true;";
            string query = "select Longitude,Latitude  from IsbikeParcs where Adres ="+ cadde+";";

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
