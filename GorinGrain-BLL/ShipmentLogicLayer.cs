using GorinGrain_BLL.ErrorHandling;
using GorinGrain_BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;


namespace GorinGrain_BLL
{
    public class ShipmentLogicLayer
    {
        //method to get top producer
        public long GetTopProducer(List<IShipmentInfoBO> myList)
        {
            long mostbushels = 0;
            long producerId = 0;
            try
            {
                //using LINQ functions to sort and group list data by producer, list brought in from logic layer
                IEnumerable<IGrouping<long, IShipmentInfoBO>> groupedList = myList.GroupBy(list => list.ProducerID);

                //for each Interface object in our list, sort by sum of the QuantityInBu columns for that ProducerID
                foreach (IGrouping<long, IShipmentInfoBO> producer in groupedList)
                {
                    //as we are looping, replace top sum if next ProducerID is greater than top sum
                    if (producer.Sum(x => x.QuantityInBu) > mostbushels)
                    {
                        producerId = producer.Key;
                        mostbushels = producer.Sum(x => x.QuantityInBu);
                    }

                    else
                    {
                        //keep looping, mostbushels (top producer) does not change
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogging.logError(e);
            }
            finally
            {
                //nothing needs to happen, error was caught if list did not poplulate
            }

            //this ID will be producer with larget number in QuantityInBu column
            return producerId;
        }
    }
}
