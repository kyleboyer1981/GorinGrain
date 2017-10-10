using GorinGrain.Interfaces;
using GorinGrain.Models;
using GorinGrain.ViewModels;
using GorinGrain_BLL.Interfaces;
using GorinGrain_DAL.Interfaces;
using GorinGrain_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

using GorinGrain_BLL.Models;

namespace GorinGrain.Custom
{
    public class ShipmentMap
    {
        public static ShipmentPO MapDOtoPO(IShipmentInfoDO iFrom)
        {
            ShipmentPO newshipment = new ShipmentPO();

            newshipment.ShipmentID = iFrom.ShipmentID;
            newshipment.LocationID = iFrom.LocationID;
            newshipment.Product = iFrom.Product;
            newshipment.ProducerID = iFrom.ProducerID;
            newshipment.QuantityInBu = iFrom.QuantityInBu;
            newshipment.PricePerBushel = iFrom.PricePerBushel;
            return newshipment;
        }

        public static IShipmentInfoDO MapPOtoDO(ShipmentPO iFrom)
        {
            IShipmentInfoDO newshipment = new ShipmentDO();

            newshipment.ShipmentID = iFrom.ShipmentID;
            newshipment.LocationID = iFrom.LocationID;
            newshipment.Product = iFrom.Product;
            newshipment.ProducerID = iFrom.ProducerID;
            newshipment.QuantityInBu = iFrom.QuantityInBu;
            newshipment.PricePerBushel = iFrom.PricePerBushel;

            return newshipment;
        }

        public static List<ShipmentPO> MapDOtoPO(List<IShipmentInfoDO> iInfo)
        {
            List<ShipmentPO> vMapInfo = new List<ShipmentPO>();
            foreach (IShipmentInfoDO info in iInfo)
            {
                ShipmentPO map = MapDOtoPO(info);
                vMapInfo.Add(map);
            }


            return vMapInfo;
        }
        
        public static List<IShipmentInfoBO> MapDOtoBO(List<IShipmentInfoDO> iInfo)
        {
            List<IShipmentInfoBO> vMapInfo = new List<IShipmentInfoBO>();
            foreach (IShipmentInfoDO info in iInfo)
            {
                IShipmentInfoBO map = MapDOtoBO(info);
                vMapInfo.Add(map);
            }
            return vMapInfo;
        }

        public static IShipmentInfoBO MapDOtoBO(IShipmentInfoDO iFrom)
        {
            IShipmentInfoBO newshipment = new ShipmentBO();

            newshipment.ShipmentID = iFrom.ShipmentID;
            newshipment.LocationID = iFrom.LocationID;
            newshipment.Product = iFrom.Product;
            newshipment.ProducerID = iFrom.ProducerID;
            newshipment.QuantityInBu = iFrom.QuantityInBu;
            newshipment.PricePerBushel = iFrom.PricePerBushel;
            return newshipment;
        }
    }
}