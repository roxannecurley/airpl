using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using AirplanePlanner.Models;

namespace AirplanePlanner.Tests
{
    [TestClass]
    public class CityTest : IDisposable
    {
        public CityTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=airline_test;";
        }

        public void Dispose()
        {
            City.DeleteAll();
            //Flights.DeleteAll();
        }

        [TestMethod]
        public void GetAll_DatabaseEmptyAtFirst_0()
        {
            //Arrange, Action
            int result = City.GetAll().Count;
            int expected = 0;

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Equals_TrueForSameCityName_City()
        {
            //Arrange, Act
            City firstCity = new City("Seattle");
            City secondCity = new City("Seattle");

            //Assert
            Assert.AreEqual(firstCity, secondCity);
        }


    }

}
