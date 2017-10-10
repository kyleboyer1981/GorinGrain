using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GorinGrain.Custom;
using GorinGrain_DAL;
using GorinGrain.Models;
using GorinGrain_DAL.Models;
using GorinGrain.Interfaces;
using System.Collections.Generic;
using GorinGrain_BLL.Interfaces;
using GorinGrain_BLL.Models;

namespace GorinGrain.Tests
{
    [TestClass]
    public class MappingTest
    {
        [TestMethod]
        public void MapDOtoPO()
        {
            //arange
            ILocationInfoDO mock = new LocationDO();
            mock.LocationID = 99;
            mock.LocationName = "Home";
            mock.Address = "123 Main";
            mock.Phone = "999-999-9999";
            mock.MaxCapacity = 50000;

            //act
            var result = LocationMap.MapDOtoPO(mock);

            //assert
            Assert.AreEqual(99, result.LocationID);
            Assert.AreEqual("Home", result.LocationName);
            Assert.AreEqual("123 Main", result.Address);
            Assert.AreEqual("999-999-9999", result.Phone);
            Assert.AreEqual(50000, result.MaxCapacity);

            //assert for full method
            // Assert.AreEqual(result, mock);


        }
    }
}
