using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using AirplanePlanner.Models;

namespace AirplanePlanner.Tests
{

[TestClass]
public class FlightTest : IDisposable
  {
    public FlightTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=3306;database=todo_test;";
    }
    public void Dispose()
    {
      Flight.DeleteAll();
      Category.DeleteAll();
    }

    [TestMethod]
    public void GetAll_DatabaseEmptyAtFirst_0()
    {
      //Arrange, Act
      int result = Flight.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_TrueForSameDescription_Flight()
    {
      //Arrange, Act
      Flight firstFlight = new Flight("Seattle", "Portland", "On Time");
      Flight secondFlight = new Flight("Seattle", "Portland", "On Time");

      //Assert
      Assert.AreEqual(firstFlight, secondFlight);
    }

    [TestMethod]
    public void Save_FlightSavesToDatabase_FlightList()
    {
      //Arrange
      Flight testFlight = new Flight("Seattle", "Portland", "On Time");
      testFlight.Save();

      //Act
      List<Flight> result = Flight.GetAll();
      List<Flight> testList = new List<Flight>{testFlight};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Save_AssignsIdToObject_id()
    {
      //Arrange
      Flight testFlight = new Flight(16:39:00, "Seattle", "Portland", "On Time");
      testFlight.Save();

      //Act
      Flight savedFlight = Flight.GetAll()[0];

      int result = savedFlight.GetId();
      int testId = testFlight.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void Find_FindsFlightInDatabase_Flight()
    {
      //Arrange
      Flight testFlight = new Flight(16:39:00, "Seattle", "Portland", "On Time");
      testFlight.Save();

      //Act
      Flight result = Flight.Find(testFlight.GetId());

      //Assert
      Assert.AreEqual(testFlight, result);
    }

  }
}
