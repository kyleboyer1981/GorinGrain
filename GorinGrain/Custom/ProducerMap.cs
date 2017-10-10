using GorinGrain.Models;
using GorinGrain_DAL.Interfaces;
using GorinGrain_DAL.Models;
using System.Collections.Generic;

namespace GorinGrain.Custom
{
    public class ProducerMap
    {
        //map producer data obj to producer presentation obj
        public static ProducerPO MapDOtoPO(IProducerInfoDO iFrom)
        {
            //instantiate new PO to fill
            ProducerPO newproducer = new ProducerPO();

            newproducer.ProducerID = iFrom.ProducerID;
            newproducer.CompanyName = iFrom.CompanyName;
            newproducer.ContactName = iFrom.ContactName;
            newproducer.Address = iFrom.Address;
            newproducer.Phone = iFrom.Phone;

            return newproducer;
        }

        public static IProducerInfoDO MapPOtoDO(ProducerPO iFrom)
        {
            IProducerInfoDO newproducer = new ProducerDO();

            newproducer.ProducerID = iFrom.ProducerID;
            newproducer.CompanyName = iFrom.CompanyName;
            newproducer.ContactName = iFrom.ContactName;
            newproducer.Address = iFrom.Address;
            newproducer.Phone = iFrom.Phone;

            return newproducer;
        }

        public static List<ProducerPO> MapDOtoPO(List<IProducerInfoDO> iInfo)
        {
            List<ProducerPO> vMapInfo = new List<ProducerPO>();
            foreach (IProducerInfoDO info in iInfo)
            {
                ProducerPO map = MapDOtoPO(info);
                vMapInfo.Add(map);
            }
            return vMapInfo;
        }
    }
}