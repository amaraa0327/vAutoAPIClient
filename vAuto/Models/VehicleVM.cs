using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vAuto.Models
{
    public class VehicleVM
    {
        public int vehicleId { get; set; }
        public int year { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public int dealerId { get; set; }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + vehicleId.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is VehicleVM)
                return vehicleId == ((VehicleVM)obj).vehicleId;

            return false;
        }
    }
}