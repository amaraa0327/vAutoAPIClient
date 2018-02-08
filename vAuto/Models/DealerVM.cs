using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vAuto.Models
{
    public class DealerVM
    {
        public int dealerId { get; set; }
        public string name { get; set; }
        public string datasetId { get; set; }
        public List<VehicleVM> vehicles { get; set; }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + dealerId.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is DealerVM)
                return dealerId == ((DealerVM)obj).dealerId;

            return false;
        }
    }
}