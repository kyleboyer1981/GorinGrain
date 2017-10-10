using GorinGrain.Models;
using GorinGrain_DAL;
using GorinGrain_DAL.Models;
using System.Collections.Generic;

namespace GorinGrain.Custom
{
    public class LocationMap
    {
        public static LocationPO MapDOtoPO(ILocationInfoDO iFrom)
        {
            LocationPO newlocation = new LocationPO();

            newlocation.LocationID = iFrom.LocationID;
            newlocation.LocationName = iFrom.LocationName;
            newlocation.Address = iFrom.Address;
            newlocation.Phone = iFrom.Phone;
            newlocation.MaxCapacity = iFrom.MaxCapacity;

            return newlocation;
        }

        public static ILocationInfoDO MapPOtoDO(LocationPO iFrom)
        {
            ILocationInfoDO newlocation = new LocationDO();

            newlocation.LocationID = iFrom.LocationID;
            newlocation.LocationName = iFrom.LocationName;
            newlocation.Address = iFrom.Address;
            newlocation.Phone = iFrom.Phone;
            newlocation.MaxCapacity = iFrom.MaxCapacity;

            return newlocation; 
        }

        public static List<LocationPO> MapDOtoPO(List<ILocationInfoDO> iInfo)
        {
            List<LocationPO> vMapInfo = new List<LocationPO>();
            foreach (ILocationInfoDO info in iInfo)
            {
                LocationPO map = MapDOtoPO(info);
                vMapInfo.Add(map);
            }


            return vMapInfo;
        }

    }
}