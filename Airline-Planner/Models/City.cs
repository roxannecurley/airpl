using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace AirplanePlanner.Models
{
    public class City
    {
        private string _name;
        private int _id;

        public City(string name, int id = 0)
        {
            _name = name;
            _id = id;
        }

        public override bool Equals(System.Object otherCity)
        {
            if (!(otherCity is City))
            {
                return false;
            }
            else
            {
                City newCity = (City) otherCity;
                bool idEquality = (this.GetId() == newCity.GetId());
                bool nameEquality =(this.GetName() == newCity.GetName());
                return (idEquality && nameEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.GetId().GetHashCode();
        }

        public string GetName()
        {
            return _name;
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
            cmd.CommandText = @"INSERT INTO cities (name) VALUES (@name);";

            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._name;
            cmd.Parameters.Add(name);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

        }

        public static List<City> GetAll()
        {
            List<City> allCities = new List<City> {};

            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM cities;";

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
              int cityId = rdr.GetInt32(0);
              string cityName = rdr.GetString(1);
              City newCity = new City(cityName, cityId);
              allCities.Add(newCity);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return allCities;
        }

        public static City Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM cities WHERE id = (@findId);";

            MySqlParameter findId = new MySqlParameter();
            findId.ParameterName = "@findId";
            findId.Value = id;
            cmd.Parameters.Add(findId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int cityId = 0;
            string cityName = "";

            while(rdr.Read())
            {
              cityId = rdr.GetInt32(0);
              cityName = rdr.GetString(1);
            }
            City newCity = new City(cityName, cityId);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return newCity;
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM cities;";

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Flight> GetFlights()
        {
            List<Flight> allCityFlights = new List<Flight> {};

            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT flight_id FROM cities_flights WHERE city_id = @city_id;";

            MySqlParameter cityIdParameter = new MySqlParameter();
            cityIdParameter.ParameterName = "@city_id";
            cityIdParameter.Value = this._id;
            cmd.Parameters.Add(cityIdParameter);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<int> flightIds = new List<int> {};

            while(rdr.Read())
            {
                int flightId = rdr.GetInt32(0);
                flightIds.Add(flightId);
            }
            rdr.Dispose();

            foreach (int flightId in flightIds)
            {
                var flightQueryCmd = conn.CreateCommand() as MySqlCommand;
                flightQueryCmd.CommandText = @"SELECT * FROM flights where id = @flightId;";

                MySqlParameter flightIdParameter = new MySqlParameter();
                flightIdParameter.ParameterName = "@flightId";
                flightIdParameter.Value = flightId;
                flightQueryCmd.Parameters.Add(flightIdParameter);

                var flightQueryRdr = flightQueryCmd.ExecuteReader() as MySqlDataReader;

                while(flightQueryRdr.Read())
                {
                  int foundFlightId = flightQueryRdr.GetInt32(0);
                  DateTime flightDepartureTime = flightQueryRdr.GetDateTime(1);
                  string flightDepartureCity = flightQueryRdr.GetString(2);
                  string flightArrivalCity = flightQueryRdr.GetString(3);
                  string flightFlightStatus = flightQueryRdr.GetString(4);
                  Flight foundFlight = new Flight(flightDepartureTime, flightDepartureCity, flightArrivalCity, flightFlightStatus, foundFlightId);
                  allCityFlights.Add(foundFlight);
                }
                flightQueryRdr.Dispose();

            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return allCityFlights;
        }
    }
}
