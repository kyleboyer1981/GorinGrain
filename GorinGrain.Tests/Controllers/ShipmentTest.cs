using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GorinGrain.Controllers;
using System.Web.Mvc;
using GorinGrain_BLL;

namespace GorinGrain.Tests.Controllers
{
    [TestClass]
    public class ShipmentTest
    {
        [TestMethod]
        public void AddShipTest()
        {
            //arrange
            ShipmentController test = new ShipmentController();

            //act
            var result = test.AddShipment() as ViewResult;

            //assert
            Assert.AreEqual("AddShipment", result);
        }

        [TestMethod]
        public void LogicTest()
        {
            //arrange
            ShipmentLogicLayer test = new ShipmentLogicLayer();


            //act

            //assert
        }
    }
}
