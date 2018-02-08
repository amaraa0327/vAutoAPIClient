using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vAuto.Models
{
    public class DataSetVM
    {
        public string datasetId { get; set; }
        public List<DealerVM> dealers { get; set; }
    }
}