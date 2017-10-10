using GorinGrain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GorinGrain.ViewModels
{
    public class LocationVM
    {
        public LocationVM()
        {
            Location = new LocationPO();
            LocationList = new List<LocationPO>();
        }

        public LocationPO Location { get; set; }

        public List<LocationPO> LocationList { get; set; }

        public string ErrorMessage { get; set; }

    }
}