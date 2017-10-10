using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using GorinGrain_BLL.Interfaces;
using GorinGrain_BLL.Models;
using GorinGrain_BLL;

namespace GorinGrain.Tests
{
    [TestClass]
    public class BusinessLogicTest

    {
        [TestMethod]
        public void LogicTest()
        {
            //arrange

            //instantiate new class to call our method to test
            ShipmentLogicLayer logic = new ShipmentLogicLayer();

            //method takes in list, need mock list
            List<IShipmentInfoBO> testList = new List<IShipmentInfoBO>();

            //mock list objects to fill mock list
            IShipmentInfoBO mock1 = new ShipmentBO();
            mock1.ProducerID = 1;
            mock1.QuantityInBu = 100;

            IShipmentInfoBO mock2 = new ShipmentBO();
            mock2.ProducerID = 2;
            mock2.QuantityInBu = 200;

            IShipmentInfoBO mock3 = new ShipmentBO();
            mock3.ProducerID = 3;
            mock3.QuantityInBu = 300;

            IShipmentInfoBO mock4 = new ShipmentBO();
            mock4.ProducerID = 4;
            mock4.QuantityInBu = 400;

            //add mock objects to mock list       
            testList.Add(mock1);
            testList.Add(mock2);
            testList.Add(mock3);
            testList.Add(mock4);

            //act

            //set variable to get result of method we are testing
            var result = logic.GetTopProducer(testList);

            //assert

            //4 should be top producer from list, does this equal what the method tells us??  Yes :-)
            Assert.AreEqual(4, result);

        }
    }
}
