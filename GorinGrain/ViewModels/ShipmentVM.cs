using GorinGrain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GorinGrain.ViewModels
{
    public class ShipmentVM
    {
        public ShipmentVM()
        {
            Shipment = new ShipmentPO();
            ShipmentList = new List<ShipmentPO>();
            LocationList = new List<LocationPO>();
            ProducerList = new List<ProducerPO>();
        }

        public ShipmentPO Shipment { get; set; }

        public List<ShipmentPO> ShipmentList { get; set; }

        public string ErrorMessage { get; set; }
        
        public List<LocationPO> LocationList { get; set; } 

        public List<ProducerPO> ProducerList { get; set; }
    }
}