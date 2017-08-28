using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace AirplanePlanner.Models
{
    public class Flight
    {
        private DateTime _departureTime;
        private string _departureCity;
        private string _arrivalCity;
        private string _flightStatus;
        private int _id;

        public Flight(Time departureTime, string departureCity, string arrivalCity, string flightStatus, int id = 0)
        {
            _departureTime = departureTime;
            _departureCity = departureCity;
            _arrivalCity = arrivalCity;
            _flightStatus = flightStatus;
            _id = id;
        }

        public override bool Equals(System.Object otherFlight)
        {
            if (!(otherFlight is Flight))
            {
                return false;
            }
            else
            {
                Flight newFlight = (Flight) otherFlight;
                bool idEquality = this.GetId() == newFlight.GetId();
                bool departureTimeEquality = this.GetDepartureTime() == newFlight.GetDepartureTime();
                bool departureCityEquality = this.GetDepartureCity() == newFlight.GetDepartureCity();
                bool arrivalCityEquality = this.GetArrivalCity() == newFlight.GetArrivalCity();
                bool flightStatusEquality = this.GetFlightStatus() == newFlight.GetFlightStatus();
                return (idEquality && departureTimeEquality && departureCityEquality && flightStatusEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.GetDepartureCity().GetHashCode();
        }

        public string GetDepartureTime()
        {
            return _departureTime;
        }

        public string GetDepartureCity()
        {
            return _departureCity;
        }

        public string GetArrivalCity()
        {
            return _arrivalCity;
        }

        public string GetFlightStatus()
        {
            return _flightStatus;
        }

        public int GetId()
        {
            return _id;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO flights (departure_city, departure_city, arrival_city, flight_status) VALUES (@departure_time, @departure_city, @arrival_city, @flight_status);";

            MySqlParameter departureTime = new MySqlParameter();
            departureTime.ParameterName = "@departure_time";
            departureTime.Value = this._departureTime;
            cmd.Parameters.Add(departureTime);

            MySqlParameter departureCity = new MySqlParameter();
            departureCity.ParameterName = "@departure_city";
            departureCity.Value = this._departureCity;
            cmd.Parameters.Add(departureCity);

            MySqlParameter arrivalCity = new MySqlParameter();
            arrivalCity.ParameterName = "@arrival_city";
            arrivalCity.Value = this._arrivalCity;
            cmd.Parameters.Add(arrivalCity);

            MySqlParameter flightStatus = new MySqlParameter();
            flightStatus.ParameterName = "@flight_status";
            flightStatus.Value = this._flightStatus;
            cmd.Parameters.Add(flightStatus);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Flight> GetAll()
        {
            List<Flight> allFlights = new List<Flight> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM flights;";

            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            while(rdr.Read())
            {
                int flightId = rdr.GetInt32(0);
                DateTime departureTime = rdr.GetTime(1);
                string departureCity = rdr.GetString(2);
                string arrivalCity = rdr.GetString(3);
                string flightStatus = rdr.GetString(4);
                Flight newFlight = new Flight(departureTime, departureCity, arrivalCity, flightStatus, flightId);
                allFlights.Add(newFlight);
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return allFlights;
        }

        public static Flight Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM flights WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            DateTime depatureTime = DateTime.MinValue;
            string departureCity = "";
            string arrivalCity = "";
            string flightStatus = "";

            while(rdr.Read())
            {
              flightId = rdr.GetInt32(0);
              departureTime = rdr.GetTime(1);
              departureCity = rdr.GetString(2);
              arrivalCity = rdr.GetString(3);
              flightStatus = rdr.GetString(4);
            }

            Flight newFlight = new Flight(departureTime, departureCity, arrivalCity, flightStatus, flightId);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return newFlight;
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"TRUNCATE TABLE flights;";
            cmd.ExecuteNonQuery();
            
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void UpdateDescription(string newDescription)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE flights SET description = @newDescription WHERE id = @searchId;";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = _id;
            cmd.Parameters.Add(searchId);

            MySqlParameter description = new MySqlParameter();
            description.ParameterName = "@newDescription";
            description.Value = newDescription;
            cmd.Parameters.Add(description);

            cmd.ExecuteNonQuery();
            _description = newDescription;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
    }
}
