
using System.Collections.Generic;
using GorinGrain.Models;

namespace GorinGrain.ViewModels
{
    public class ProducerVM
    {
        public ProducerVM()
        {
            Producer = new ProducerPO();
            ProducerList = new List<ProducerPO>();
            
        }

        public ProducerPO Producer { get; set; }
        
        public List<ProducerPO> ProducerList { get; set; }

        public string ErrorMessage { get; set; }


    }
}