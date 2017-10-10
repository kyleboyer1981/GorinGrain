using GorinGrain_DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorinGrain_DAL.Models
{
    public class ProducerDO : IProducerInfoDO
    {
        public long ProducerID { get; set; }

        public string CompanyName { get; set; }

        public string ContactName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }
    }
}
